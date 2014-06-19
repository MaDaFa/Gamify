using Gamify.Sdk.Services;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gamify.Sdk.UnitTests.ServiceTests
{
    [TestClass]
    public class NotificationServiceTests
    {
        private ISerializer serializer;
        private INotificationService notificationService;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            this.notificationService = new NotificationService(this.serializer);
        }

        [TestMethod]
        public void When_Send_Then_Success()
        {
            var userName = "player1";
            var notificationType = 204;
            var notificationObject = new TestNotificationObject
            {
                Name = "Test 1"
            };

            var notifiedUserName = default(string);
            var notifiedType = default(int);
            var notifiedObject = default(TestNotificationObject);

            this.notificationService.Notification += (sender, e) =>
            {
                notifiedUserName = e.Receiver;
                notifiedType = e.Notification.Type;
                notifiedObject = this.serializer.Deserialize<TestNotificationObject>(e.Notification.SerializedNotificationObject);
            };

            this.notificationService.Send(notificationType, notificationObject, userName);

            Assert.AreEqual(userName, notifiedUserName);
            Assert.AreEqual(notificationType, notifiedType);
            Assert.AreEqual(notificationObject.Name, notifiedObject.Name);
            Assert.AreEqual(notificationObject.Message, notifiedObject.Message);
        }
    }
}
