using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestMoveResultNotificationFactory : IMoveResultNotificationFactory
    {
        public IMoveResultNotificationObject Create(MoveRequestObject moveRequest, IGameMoveResponse moveResponse)
        {
            var responseObject = moveResponse.MoveResponseObject as TestResponseObject;
            var moveResultNotificationObject = new TestMoveResultNotificationObject
            {
                SessionName = moveRequest.SessionName,
                PlayerName = moveRequest.PlayerName,
                AnsweredCorrect = responseObject.AnsweredCorrect
            };

            return moveResultNotificationObject;
        }
    }
}
