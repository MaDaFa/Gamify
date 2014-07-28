using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
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
    public class GameSelectionPluginComponent<TMove, UResponse> : PluginComponentBase
    {
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService<TMove, UResponse> sessionHistoryService;
        private readonly IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory;

        public GameSelectionPluginComponent(ISessionService sessionService, ISessionHistoryService<TMove, UResponse> sessionHistoryService,
            INotificationService notificationService, IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory, ISerializer serializer)
            : base(notificationService, serializer)
        {
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.playerHistoryItemFactory = playerHistoryItemFactory;
        }

        public override bool CanHandleClientMessage(ClientContract clientContract)
        {
            return clientContract.Type == GamifyClientMessageType.OpenGame ||
                clientContract.Type == GamifyClientMessageType.GetActiveGames ||
                clientContract.Type == GamifyClientMessageType.GetFinishedGames ||
                clientContract.Type == GamifyClientMessageType.GetPendingGames;
        }

        public override bool CanHandleServerMessage(ServerContract serverContract)
        {
            return serverContract.Type == GamifyServerMessageType.GameInformationReceived ||
                serverContract.Type == GamifyServerMessageType.ActiveGamesList ||
                serverContract.Type == GamifyServerMessageType.FinishedGamesList ||
                serverContract.Type == GamifyServerMessageType.PendingGamesList;
        }

        public override void HandleClientMessage(ClientContract clientContract)
        {
            switch (clientContract.Type)
            {
                case GamifyClientMessageType.OpenGame:
                    this.HandleOpenGame(clientContract);
                    break;
                case GamifyClientMessageType.GetActiveGames:
                    this.HandleGetActiveGames(clientContract);
                    break;
                case GamifyClientMessageType.GetFinishedGames:
                    this.HandleGetFinishedGames(clientContract);
                    break;
                case GamifyClientMessageType.GetPendingGames:
                    this.HandleGetPendingGames(clientContract);
                    break;
            }
        }

        private void HandleOpenGame(ClientContract clientContract)
        {
            var openGameClientMessage = this.serializer.Deserialize<OpenGameClientMessage>(clientContract.SerializedClientMessage);
            var currentSession = this.sessionService.GetByName(openGameClientMessage.SessionName);
            var gameInformationReceivedServerMessage = this.GetGameInformationReceivedServerMessage(currentSession);

            this.notificationService.Send(GamifyServerMessageType.GameInformationReceived, gameInformationReceivedServerMessage, openGameClientMessage.UserName);
        }

        private GameInformationReceivedServerMessage GetGameInformationReceivedServerMessage(IGameSession session)
        {
            var sessionPlayer1 = session.Player1;
            var sessionPlayer2 = session.Player2;
            var sessionPlayer1History = new PlayerHistoryObject(sessionPlayer1.Information.Name);
            var sessionPlayer2History = new PlayerHistoryObject(sessionPlayer2.Information.Name);

            var sessionPlayer1Moves = this.sessionHistoryService.GetBySessionPlayer(session.Name, sessionPlayer1.Information.Name).Get();

            foreach (var sessionPlayer1Move in sessionPlayer1Moves)
            {
                var historiItem = this.playerHistoryItemFactory.Create(sessionPlayer1Move.Response);

                sessionPlayer1History.AddMove(historiItem);
            }

            var sessionPlayer2Moves = this.sessionHistoryService.GetBySessionPlayer(session.Name, sessionPlayer2.Information.Name).Get();

            foreach (var sessionPlayer2Move in sessionPlayer2Moves)
            {
                var historiItem = this.playerHistoryItemFactory.Create(sessionPlayer2Move.Response);

                sessionPlayer2History.AddMove(historiItem);
            }

            var gameInformationReceivedServerMessage = new GameInformationReceivedServerMessage
            {
                SessionName = session.Name,
                Player1History = sessionPlayer1History,
                Player2History = sessionPlayer2History
            };

            return gameInformationReceivedServerMessage;
        }

        private void HandleGetActiveGames(ClientContract clientContract)
        {
            var getActiveGamesClientMessage = this.serializer.Deserialize<GetActiveGamesClientMessage>(clientContract.SerializedClientMessage);
            var activePlayerSessions = this.sessionService.GetActives(getActiveGamesClientMessage.UserName);
            var activeGamesListServerMessage = new ActiveGamesListServerMessage
            {
                PlayerName = getActiveGamesClientMessage.UserName
            };

            foreach (var activePlayerSession in activePlayerSessions)
            {
                activeGamesListServerMessage.AddActiveGame(new GameObject
                {
                    SessionName = activePlayerSession.Name,
                    Player1Name = activePlayerSession.Player1Name,
                    Player2Name = activePlayerSession.Player2Name
                });
            }

            this.notificationService.Send(GamifyServerMessageType.ActiveGamesList, activeGamesListServerMessage, getActiveGamesClientMessage.UserName);
        }

        private void HandleGetFinishedGames(ClientContract clientContract)
        {
            var getFinishedGamesClientMessage = this.serializer.Deserialize<GetFinishedGamesClientMessage>(clientContract.SerializedClientMessage);
            var finishedPlayerSessions = this.sessionService.GetFinished(getFinishedGamesClientMessage.UserName);
            var finishedGamesListServerMessage = new FinishedGamesListServerMessage
            {
                PlayerName = getFinishedGamesClientMessage.UserName
            };

            foreach (var finishedPlayerSession in finishedPlayerSessions)
            {
                finishedGamesListServerMessage.AddFinishedGame(new GameObject
                {
                    SessionName = finishedPlayerSession.Name,
                    Player1Name = finishedPlayerSession.Player1Name,
                    Player2Name = finishedPlayerSession.Player2Name
                });
            }

            this.notificationService.Send(GamifyServerMessageType.FinishedGamesList, finishedGamesListServerMessage, getFinishedGamesClientMessage.UserName);
        }

        private void HandleGetPendingGames(ClientContract clientContract)
        {
            var getPendingGamesClientMessage = this.serializer.Deserialize<GetPendingGamesClientMessage>(clientContract.SerializedClientMessage);
            var pendingPlayerSessions = this.sessionService.GetPendings(getPendingGamesClientMessage.UserName);
            var pendingGamesListServerMessage = new PendingGamesListServerMessage
            {
                PlayerName = getPendingGamesClientMessage.UserName
            };

            foreach (var pendingPlayerSession in pendingPlayerSessions)
            {
                pendingGamesListServerMessage.AddPendingGame(new GameObject
                {
                    SessionName = pendingPlayerSession.Name,
                    Player1Name = pendingPlayerSession.Player1Name,
                    Player2Name = pendingPlayerSession.Player2Name
                });
            }

            this.notificationService.Send(GamifyServerMessageType.PendingGamesList, pendingGamesListServerMessage, getPendingGamesClientMessage.UserName);
        }
    }
}
