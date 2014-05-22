using Gamify.Client.Net.Client;
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
        private readonly IGameListener<ErrorNotificationObject> errorListener;

        public event EventHandler<GameNotificationEventArgs<MessageNotificationObject>> MessageNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<TypingMessageNotificationObject>> TypingMessageNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<ErrorNotificationObject>> ErrorNotificationReceived;

        public MessageManager(string playerName, IGameClientFactory clientFactory)
        {
            var gameClient = clientFactory.GetGameClient(playerName);

            this.messageService = new GameService<MessageRequestObject, MessageNotificationObject>(GameRequestType.Message, GameNotificationType.Message, gameClient);
            this.typingMessageService = new GameService<TypingMessageRequestObject, TypingMessageNotificationObject>(GameRequestType.TypingMessage, GameNotificationType.TypingMessage, gameClient);
            this.errorListener = new GameListener<ErrorNotificationObject>(GameNotificationType.Error, gameClient);

            this.messageService.NotificationReceived += (sender, args) =>
            {
                this.NotifyMessage(args);
            };
            this.typingMessageService.NotificationReceived += (sender, args) =>
            {
                this.NotifyTypingMessage(args);
            };
            this.errorListener.NotificationReceived += (sender, args) =>
            {
                this.NotifyError(args);
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

        private void NotifyMessage(GameNotificationEventArgs<MessageNotificationObject> args)
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

        private void NotifyError(GameNotificationEventArgs<ErrorNotificationObject> args)
        {
            if (this.ErrorNotificationReceived != null)
            {
                this.ErrorNotificationReceived(this, args);
            }
        }
    }
}
