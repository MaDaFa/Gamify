using Gamify.Client.Net.Managers;
using Gamify.Contracts.Notifications;
using System;

namespace Gamify.Client.Net
{
    public class GameCreationManagerTest
    {
        public void Test()
        {
            var gameCreationManager = new GameCreationManager("testPlayer");

            gameCreationManager.GameCreatedNotificationReceived += gameCreationManager_GameCreatedNotificationReceived;
            gameCreationManager.GameInviteNotificationReceived += gameCreationManager_GameInviteNotificationReceived;
            gameCreationManager.GameRejectedNotificationReceived += gameCreationManager_GameRejectedNotificationReceived;

            gameCreationManager.CreateGame(null);
            gameCreationManager.AcceptGame(null);
            gameCreationManager.RejectGame(null);
        }

        public void gameCreationManager_GameCreatedNotificationReceived(object sender, GameNotificationEventArgs<GameCreatedNotificationObject> e)
        {
            var notification = e.NotificationObject;
        }

        public void gameCreationManager_GameInviteNotificationReceived(object sender, GameNotificationEventArgs<GameInviteNotificationObject> e)
        {
            throw new NotImplementedException();
        }

        public void gameCreationManager_GameRejectedNotificationReceived(object sender, GameNotificationEventArgs<GameRejectedNotificationObject> e)
        {
            throw new NotImplementedException();
        }
    }
}
