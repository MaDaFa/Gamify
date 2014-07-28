using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Interfaces;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using ThinkUp.Sdk;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace Gamify.Sdk.PluginComponents
{
    public class GameProgressPluginComponent<TMove, UResponse> : PluginComponentBase
    {
        private readonly IMoveService<TMove, UResponse> moveService;
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService<TMove, UResponse> sessionHistoryService;
        private readonly IMoveFactory<TMove> moveFactory;
        private readonly IMoveResultNotificationFactory moveResultNotificationFactory;

        public GameProgressPluginComponent(IMoveService<TMove, UResponse> moveService, ISessionService sessionService,
            ISessionHistoryService<TMove, UResponse> sessionHistoryService, INotificationService notificationService,
            IMoveFactory<TMove> moveFactory, IMoveResultNotificationFactory moveResultNotificationFactory, ISerializer serializer)
            : base(notificationService, serializer)
        {
            this.moveService = moveService;
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.moveFactory = moveFactory;
            this.moveResultNotificationFactory = moveResultNotificationFactory;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == GamifyClientMessageType.SendMove ||
                clientContract.Type == GamifyClientMessageType.AbandonGame;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == GamifyServerMessageType.MoveReceived ||
                serverContract.Type == GamifyServerMessageType.MoveResultReceived ||
                serverContract.Type == GamifyServerMessageType.GameFinished ||
                serverContract.Type == GamifyServerMessageType.GameAbandoned;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            switch (clientContract.Type)
            {
                case GamifyClientMessageType.SendMove:
                    this.HandleSendMove(clientContract);
                    break;
                case GamifyClientMessageType.AbandonGame:
                    this.HandleAbandonGame(clientContract);
                    break;
            }
        }

        private void HandleSendMove(ClientContract clientContract)
        {
            var sendMoveClientMessage = this.serializer.Deserialize<SendMoveClientMessage>(clientContract.SerializedClientMessage);
            var currentSession = this.sessionService.GetByName(sendMoveClientMessage.SessionName);
            var originPlayer = currentSession.GetPlayer(sendMoveClientMessage.UserName);
            var destinationPlayer = currentSession.GetVersusPlayer(originPlayer.Information.Name);
            var move = this.moveFactory.Create(sendMoveClientMessage.MoveInformation);
            var moveResponse = this.moveService.Handle(currentSession.Name, originPlayer.Information.Name, move);

            var sessionHistoryItem = new SessionHistoryItem<TMove, UResponse>(move.MoveObject, moveResponse.MoveResponseObject);

            this.sessionHistoryService.Add(currentSession.Name, originPlayer.Information.Name, sessionHistoryItem);

            if (moveResponse.IsWin)
            {
                this.sessionService.Finish(currentSession.Name);

                var gameFinishedServerMessage = new GameFinishedServerMessage
                {
                    SessionName = currentSession.Name,
                    WinnerPlayerName = originPlayer.Information.Name,
                    LooserPlayerName = destinationPlayer.Information.Name
                };

                this.notificationService.SendBroadcast(GamifyServerMessageType.GameFinished, gameFinishedServerMessage,
                    gameFinishedServerMessage.WinnerPlayerName, gameFinishedServerMessage.LooserPlayerName);
            }
            else
            {
                originPlayer.PendingToMove = false;
                destinationPlayer.PendingToMove = true;

                this.SendMoveNotification(sendMoveClientMessage, destinationPlayer.Information.Name);
                this.SendMoveResultNotification(sendMoveClientMessage, moveResponse, destinationPlayer.Information.Name);
            }
        }

        private void SendMoveNotification(SendMoveClientMessage moveRequestObject, string destinationPlayerName)
        {
            var moveReceivedServerMessage = new MoveReceivedServerMessage
            {
                SessionName = moveRequestObject.SessionName,
                PlayerName = moveRequestObject.UserName,
                MoveInformation = moveRequestObject.MoveInformation
            };

            this.notificationService.Send(GamifyServerMessageType.MoveReceived, moveReceivedServerMessage, destinationPlayerName);
        }

        private void SendMoveResultNotification(SendMoveClientMessage moveRequestObject, IGameMoveResponse moveResponse, string destinationPlayerName)
        {
            var moveResultReceivedServerMessage = this.moveResultNotificationFactory.Create(moveRequestObject, moveResponse);

            this.notificationService.Send(GamifyServerMessageType.MoveResultReceived, moveResultReceivedServerMessage, moveRequestObject.UserName);
        }

        private void HandleAbandonGame(ClientContract clientContract)
        {
            var abandonGameClientMessage = this.serializer.Deserialize<AbandonGameClientMessage>(clientContract.SerializedClientMessage);
            var currentSession = this.sessionService.GetByName(abandonGameClientMessage.SessionName);

            this.sessionService.Abandon(currentSession.Name);

            var gameAbandonedServerMessage = new GameAbandonedServerMessage
            {
                SessionName = abandonGameClientMessage.SessionName,
                PlayerName = abandonGameClientMessage.UserName
            };

            this.notificationService.SendBroadcast(GamifyServerMessageType.GameAbandoned, gameAbandonedServerMessage, 
                currentSession.Player1Name, currentSession.Player2Name);
        }
    }
}
