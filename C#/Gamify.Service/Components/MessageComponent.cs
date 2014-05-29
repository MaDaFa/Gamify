using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;

namespace Gamify.Service.Components
{
    public class MessageComponent : IGameComponent
    {
        private readonly ISerializer<MessageRequestObject> messageSerializer;
        private readonly ISerializer<TypingMessageRequestObject> typingMessageSerializer;

        public INotificationService NotificationService { get; private set; }

        public MessageComponent(INotificationService notificationService)
        {
            this.messageSerializer = new JsonSerializer<MessageRequestObject>();
            this.typingMessageSerializer = new JsonSerializer<TypingMessageRequestObject>();

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
                var messageObject = this.messageSerializer.Deserialize(request.SerializedRequestObject);
                var notification = new MessageNotificationObject
                {
                    FromPlayerName = messageObject.PlayerName,
                    Message = messageObject.Message
                };

                this.NotificationService.Send(GameNotificationType.Message, notification, messageObject.ToPlayerName);
            }
            else
            {
                var typingMessageObject = this.typingMessageSerializer.Deserialize(request.SerializedRequestObject);
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
