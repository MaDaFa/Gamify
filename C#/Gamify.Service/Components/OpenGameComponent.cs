using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public abstract class OpenGameComponent : IGameComponent
    {
        private readonly ISerializer<OpenGameRequestObject> serializer;
        private readonly IGameController gameController;
        protected readonly INotificationService notificationService;

        protected OpenGameComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<OpenGameRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.OpenGame;
        }

        public void HandleRequest(GameRequest request)
        {
            var openGameObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var currentSession = this.gameController.Sessions.First(s => s.Id == openGameObject.SessionId);

            this.SendGameInformation(openGameObject.PlayerName, currentSession);
        }

        protected abstract void SendGameInformation(string playerName, IGameSession gameSession);
    }
}
