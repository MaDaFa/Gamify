using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestMoveProcessor : IMoveProcessor<TestMoveObject, TestResponseObject>
    {
        public IGameMoveResponse<TestResponseObject> Process(SessionGamePlayer sessionGamePlayer, IGameMove<TestMoveObject> move)
        {
            var isCorrect = GameHelper.Instance.IsCorrect(move.MoveObject);

            return new TestResponse
            {
                MoveResponseObject = new TestResponseObject
                {
                    QuestionId = move.MoveObject.QuestionId,
                    AnswerId = move.MoveObject.AnswerId,
                    AnsweredCorrect = isCorrect
                }
            };
        }
    }
}
