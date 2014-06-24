using Gamify.Client.Net.Contracts.Notifications;
using Gamify.Client.Net.Contracts.Requests;
using System;
using System.Threading;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Gamify.Client.Net.Client
{
    public class GameClient : IGameClient
    {
        private readonly Uri gameServerUri;
        private readonly ISerializer serializer;
        private MessageWebSocket gameWebSocketClient;
        private DataWriter gameMessageWriter;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public bool IsInitialized
        {
            get
            {
                return this.gameWebSocketClient != null;
            }
        }

        public GameClient(string gameServerUri)
        {
            this.gameServerUri = new Uri(gameServerUri);
            this.serializer = new JsonSerializer();
        }

        public async void Initialize()
        {
            var webSocketClient = this.gameWebSocketClient;

            if (this.IsInitialized)
            {
                return;
            }

            webSocketClient = new MessageWebSocket();
            
            webSocketClient.Control.MessageType = SocketMessageType.Utf8;
            webSocketClient.MessageReceived += (sender, args) =>
            {
                this.ReceiveMessage(args);
            };
            webSocketClient.Closed += (sender, args) =>
            {
                this.CloseConnection(args);
            };

            await webSocketClient.ConnectAsync(this.gameServerUri);

            this.gameWebSocketClient = webSocketClient;
            this.gameMessageWriter = new DataWriter(this.gameWebSocketClient.OutputStream);
        }

        public void Send(GameRequest gameRequest)
        {
            if (!this.IsInitialized)
            {
                throw new GameClientException("The client is not initialized");
            }

            var serializedGameRequest = this.serializer.Serialize(gameRequest);

            this.gameMessageWriter.WriteString(serializedGameRequest);
        }

        private void ReceiveMessage(MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                using (var reader = args.GetDataReader())
                {
                    reader.UnicodeEncoding = UnicodeEncoding.Utf8;

                    var message = reader.ReadString(reader.UnconsumedBufferLength);
                    var gameNotification = this.serializer.Deserialize<GameNotification>(message);

                    if (this.MessageReceived != null)
                    {
                        this.MessageReceived(this, new MessageReceivedEventArgs(gameNotification));
                    }
                }
            }
            catch (Exception ex)
            {
                var status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                var errorMessage = string.Format("An error occured while trying to send a message to the server. Error type: {0}", status.ToString());

                throw new GameClientException(errorMessage, ex);
            }
        }

        private void CloseConnection(WebSocketClosedEventArgs args)
        {
            var webSocketClient = Interlocked.Exchange(ref this.gameWebSocketClient, null);

            if (webSocketClient != null)
            {
                webSocketClient.Dispose();
                webSocketClient = null;
            }
        }
    }
}
