using Gamify.Client.Net.Client;
using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;

namespace Gamify.Client.Net.Services
{
    public class GameService<TRequest, UNotification> : GameListener<UNotification>, IGameService<TRequest, UNotification>
        where TRequest : IRequestObject
        where UNotification : INotificationObject
    {
        private readonly GameRequestType requestType;
        private readonly ISerializer<TRequest> requestSerializer;

        public GameService(GameRequestType requestType, GameNotificationType notificationType, IGameClient gameClient)
            : base(notificationType, gameClient)
        {
            this.requestType = requestType;
            this.requestSerializer = new JsonSerializer<TRequest>();
        }

        public void Send(TRequest request)
        {
            var serializedGameRequest = this.requestSerializer.Serialize(request);
            var gameRequest = new GameRequest
            {
                Type = (int)this.requestType,
                SerializedRequestObject = serializedGameRequest
            };

            this.gameClient.Send(gameRequest);
        }
    }
}
