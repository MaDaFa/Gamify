using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using ThinkUp.Sdk;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Contracts.ServerMessages;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace Gamify.Sdk.PluginComponents
{
    public class GameCreationPluginComponent : PluginComponentBase
    {
        private readonly IUserService userService;
        private readonly ISessionService sessionService;
        private readonly ISessionPlayerFactory sessionPlayerFactory;
        private readonly ISessionPlayerSetup sessionPlayerSetup;
        private readonly IGameInviteDecorator gameInviteDecorator;

        public GameCreationPluginComponent(IUserService userService, ISessionService sessionService, INotificationService notificationService,
            ISessionPlayerFactory sessionPlayerFactory, ISessionPlayerSetup sessionPlayerSetup, 
            IGameInviteDecorator gameInviteDecorator, ISerializer serializer)
            : base(notificationService, serializer)
        {
            this.userService = userService;
            this.sessionService = sessionService;
            this.sessionPlayerFactory = sessionPlayerFactory;
            this.sessionPlayerSetup = sessionPlayerSetup;
            this.gameInviteDecorator = gameInviteDecorator;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == GamifyClientMessageType.CreateGame ||
                clientContract.Type == GamifyClientMessageType.AcceptGame ||
                clientContract.Type == GamifyClientMessageType.RejectGame;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == GamifyServerMessageType.GameInviteReceived ||
                serverContract.Type == GamifyServerMessageType.GameCreated ||
                serverContract.Type == GamifyServerMessageType.GameRejected;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            switch (clientContract.Type)
            {
                case GamifyClientMessageType.CreateGame:
                    this.HandleCreateGame(clientContract);
                    break;
                case GamifyClientMessageType.AcceptGame:
                    this.HandleAcceptGame(clientContract);
                    break;
                case GamifyClientMessageType.RejectGame:
                    this.HandleRejectGame(clientContract);
                    break;
            }
        }

        private void HandleCreateGame(ClientContract clientContract)
        {
            var createGameClientMessage = this.serializer.Deserialize<CreateGameClientMessage>(clientContract.SerializedClientMessage);
            var connectedPlayer1 = this.userService.GetByName(createGameClientMessage.UserName);
            var sessionPlayer1 = this.sessionPlayerFactory.Create(connectedPlayer1);
            var connectedPlayer2 = this.userService.GetByName(createGameClientMessage.InvitedUserName);
            var sessionPlayer2 = this.sessionPlayerFactory.Create(connectedPlayer2);

            this.sessionPlayerSetup.GetPlayerReady(createGameClientMessage, sessionPlayer1);

            var newSession = this.sessionService.Create(sessionPlayer1, sessionPlayer2);

            var gameInviteReceivedServerMessage = new GameInviteReceivedServerMessage
            {
                SessionName = newSession.Name,
                Player1Name = newSession.Player1Name
            };

            this.gameInviteDecorator.Decorate(gameInviteReceivedServerMessage, newSession);

            this.notificationService.Send(GamifyServerMessageType.GameInviteReceived, gameInviteReceivedServerMessage, newSession.Player2Name);
        }

        private void HandleAcceptGame(ClientContract clientContract)
        {
            var acceptGameClientMessage = this.serializer.Deserialize<AcceptGameClientMessage>(clientContract.SerializedClientMessage);
            var pendingSession = this.sessionService.GetByName(acceptGameClientMessage.SessionName);

            this.sessionPlayerSetup.GetPlayerReady(acceptGameClientMessage, pendingSession.Player2);

            pendingSession.Player1.PendingToMove = true;

            this.sessionService.Start(pendingSession);

            var gameCreatedServerMessage = new GameCreatedServerMessage
            {
                SessionName = pendingSession.Name,
                Player1Name = pendingSession.Player1Name,
                Player2Name = pendingSession.Player2Name
            };

            this.notificationService.SendBroadcast(GamifyServerMessageType.GameCreated, gameCreatedServerMessage, pendingSession.Player1Name, pendingSession.Player2Name);
        }

        private void HandleRejectGame(ClientContract clientContract)
        {
            var rejectGameClientMessage = this.serializer.Deserialize<RejectGameClientMessage>(clientContract.SerializedClientMessage);
            var newSession = this.sessionService.GetByName(rejectGameClientMessage.SessionName);
            var gameRejectedServerMessage = new GameRejectedServerMessage
            {
                SessionName = newSession.Name,
                Player1Name = newSession.Player1Name,
                Player2Name = newSession.Player2Name
            };

            this.notificationService.Send(GamifyServerMessageType.GameRejected, gameRejectedServerMessage, newSession.Player1Name);
        }
    }
}
