using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class MessageComponent : IGameComponent
    {
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public MessageComponent(INotificationService notificationService, ISerializer serializer)
        {
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.Message || request.Type == (int)GameRequestType.TypingMessage;
        }

        public void HandleRequest(GameRequest request)
        {
            if (request.Type == (int)GameRequestType.Message)
            {
                var messageObject = this.serializer.Deserialize<MessageRequestObject>(request.SerializedRequestObject);
                var notification = new MessageNotificationObject
                {
                    FromPlayerName = messageObject.PlayerName,
                    Message = messageObject.Message
                };

                this.NotificationService.Send(GameNotificationType.Message, notification, messageObject.ToPlayerName);
            }
            else
            {
                var typingMessageObject = this.serializer.Deserialize<TypingMessageRequestObject>(request.SerializedRequestObject);
                var notification = new TypingMessageNotificationObject
                {
                    FromPlayerName = typingMessageObject.PlayerName,
                    Message = typingMessageObject.TypingMessage
                };

                this.NotificationService.Send(GameNotificationType.TypingMessage, notification, typingMessageObject.ToPlayerName);
            }
        }
    }
}
