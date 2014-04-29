using System;

namespace Gamify.Service
{
    public class GamifyMessageEventArgs : EventArgs
    {
        public IGamifyClientBase Client { get; set; }

        public string  Message { get; set; }

        public GamifyMessageEventArgs(IGamifyClientBase client, string message)
        {
            this.Client = client;
            this.Message = message;
        }

        public GamifyMessageEventArgs()
        {
        }
    }
}
