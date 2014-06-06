using Gamify.Contracts.Requests;
using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.ComponentTests
{
    [TestClass]
    public class MessageComponentTests
    {
        private readonly string requestPlayer = "player1";

        private ISerializer serializer;
        private Mock<INotificationService> notificationServiceMock;
        private IGameComponent messageComponent;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.messageComponent = new MessageComponent(notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleMessage_Then_Success()
        {
            var messageRequest = new MessageRequestObject
            {
                PlayerName = this.requestPlayer,
                ToPlayerName = "player2",
                Message = "Hello"
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.Message,
                SerializedRequestObject = this.serializer.Serialize(messageRequest)
            };

            var canHandle = this.messageComponent.CanHandleRequest(gameRequest);

            this.messageComponent.HandleRequest(gameRequest);

            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.Message),
                    It.Is<object>(o => ((MessageNotificationObject)o).FromPlayerName == this.requestPlayer),
                    It.Is<string>(x => x == "player2")));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void When_HandleTypingMessage_Then_Success()
        {
            var typingMessageRequest = new TypingMessageRequestObject
            {
                PlayerName = this.requestPlayer,
                ToPlayerName = "player2",
                TypingMessage = "Typing..."
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.TypingMessage,
                SerializedRequestObject = this.serializer.Serialize(typingMessageRequest)
            };

            var canHandle = this.messageComponent.CanHandleRequest(gameRequest);

            this.messageComponent.HandleRequest(gameRequest);

            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.TypingMessage),
                    It.Is<object>(o => ((TypingMessageNotificationObject)o).FromPlayerName == this.requestPlayer),
                    It.Is<string>(x => x == "player2")));

            Assert.IsTrue(canHandle);
        }
    }
}
