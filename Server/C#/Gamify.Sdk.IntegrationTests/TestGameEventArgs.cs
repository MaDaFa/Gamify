using System;

namespace Gamify.Sdk.IntegrationTests
{
    public class TestGameEventArgs : EventArgs
    {
        public string SerializedNotification { get; set; }

        public TestGameEventArgs(string serializedNotification)
        {
            this.SerializedNotification = serializedNotification;
        }

        public TestGameEventArgs()
        {
        }
    }
}
