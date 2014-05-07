using System;

namespace Gamify.Service
{
    public class GamifyMessageEventArgs : EventArgs
    {
        public string UserName { get; set; }

        public string  Message { get; set; }

        public GamifyMessageEventArgs(string userName, string message)
        {
            this.UserName = userName;
            this.Message = message;
        }

        public GamifyMessageEventArgs()
        {
        }
    }
}
