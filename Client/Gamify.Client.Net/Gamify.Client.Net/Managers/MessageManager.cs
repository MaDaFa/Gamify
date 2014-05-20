using Gamify.Client.Net.Services;
using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net.Managers
{
    public class MessageManager
    {
        private readonly IGameService<MessageRequestObject, MessageNotificationObject> messageService;
        private readonly IGameService<TypingMessageRequestObject, TypingMessageNotificationObject> typingMessageService;

        public event EventHandler<GameNotificationEventArgs<MessageNotificationObject>> MessageNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<TypingMessageNotificationObject>> TypingMessageNotificationReceived;

        public MessageManager(string playerName)
        {
            var gamifyClient = GameClientFactory.GetGameClient(playerName);

            this.messageService = new GameService<MessageRequestObject, MessageNotificationObject>(GameRequestType.Message, GameNotificationType.Message, gamifyClient);
            this.typingMessageService = new GameService<TypingMessageRequestObject, TypingMessageNotificationObject>(GameRequestType.TypingMessage, GameNotificationType.TypingMessage, gamifyClient);

            this.messageService.NotificationReceived += (sender, args) =>
            {
                this.NotifyMessge(args);
            };
            this.typingMessageService.NotificationReceived += (sender, args) =>
            {
                this.NotifyTypingMessage(args);
            };
        }

        public void SendMessage(MessageRequestObject messageRequest)
        {
            this.messageService.Send(messageRequest);
        }

        public void SendTypingMessage(TypingMessageRequestObject typingMessageRequest)
        {
            this.typingMessageService.Send(typingMessageRequest);
        }

        private void NotifyMessge(GameNotificationEventArgs<MessageNotificationObject> args)
        {
            if (this.MessageNotificationReceived != null)
            {
                this.MessageNotificationReceived(this, args);
            }
        }

        private void NotifyTypingMessage(GameNotificationEventArgs<TypingMessageNotificationObject> args)
        {
            if (this.TypingMessageNotificationReceived != null)
            {
                this.TypingMessageNotificationReceived(this, args);
            }
        }
    }
}
