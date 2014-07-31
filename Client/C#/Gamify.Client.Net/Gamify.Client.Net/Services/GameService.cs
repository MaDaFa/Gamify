using Gamify.Client.Net.Client;
using Gamify.Client.Net.Contracts.Notifications;
using Gamify.Client.Net.Contracts.Requests;

namespace Gamify.Client.Net.Services
{
    public class GameService<TRequest, UNotification> : GameListener<UNotification>, IGameService<TRequest, UNotification>
        where TRequest : IRequestObject
        where UNotification : INotificationObject
    {
        private readonly GameRequestType requestType;
        private readonly ISerializer serializer;

        public GameService(GameRequestType requestType, GameNotificationType notificationType, IGameClient gameClient)
            : base(notificationType, gameClient)
        {
            this.requestType = requestType;
            this.serializer = new JsonSerializer();
        }

        public void Send(TRequest request)
        {
            var serializedGameRequest = this.serializer.Serialize(request);
            var gameRequest = new GameRequest
            {
                Type = (int)this.requestType,
                SerializedRequestObject = serializedGameRequest
            };

            this.gameClient.Send(gameRequest);
        }
    }
}
