using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestMoveResultNotificationFactory : IMoveResultNotificationFactory
    {
        public IMoveResultReceivedServerMessage Create(SendMoveClientMessage moveRequest, IGameMoveResponse moveResponse)
        {
            var responseObject = moveResponse.MoveResponseObject as TestResponseObject;
            var moveResultNotificationObject = new TestMoveResultNotificationObject
            {
                SessionName = moveRequest.SessionName,
                PlayerName = moveRequest.UserName,
                AnsweredCorrect = responseObject.AnsweredCorrect
            };

            return moveResultNotificationObject;
        }
    }
}
