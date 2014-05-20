using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;

namespace Gamify.Service.Components
{
    public class MessageComponent : IGameComponent
    {
        private readonly ISerializer<MessageRequestObject> messageSerializer;
        private readonly ISerializer<TypingMessageRequestObject> typingMessageSerializer;
        private readonly INotificationService notificationService;

        public MessageComponent(INotificationService notificationService)
        {
            this.messageSerializer = new JsonSerializer<MessageRequestObject>();
            this.typingMessageSerializer = new JsonSerializer<TypingMessageRequestObject>();
            this.notificationService = notificationService;
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

                this.notificationService.Send(GameNotificationType.Message, notification, messageObject.ToPlayerName);
            }
            else
            {
                var typingMessageObject = this.typingMessageSerializer.Deserialize(request.SerializedRequestObject);
                var notification = new TypingMessageNotificationObject
                {
                    FromPlayerName = typingMessageObject.PlayerName,
                    Message = typingMessageObject.TypingMessage
                };

                this.notificationService.Send(GameNotificationType.TypingMessage, notification, typingMessageObject.ToPlayerName);
            }
        }
    }
}
