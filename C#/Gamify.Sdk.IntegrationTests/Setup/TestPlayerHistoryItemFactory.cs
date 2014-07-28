using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestPlayerHistoryItemFactory : IPlayerHistoryItemFactory<TestMoveObject, TestResponseObject>
    {
        public IPlayerHistoryItem Create(TestResponseObject gameResponseObject)
        {
            var question = GameHelper.Instance.GetQuestion(gameResponseObject.QuestionId);
            var answer = GameHelper.Instance.GetAnswer(gameResponseObject.AnswerId);

            return new TestPlayerHistoryItem
            {
                Question = question,
                Answer = answer,
                Correct = gameResponseObject.AnsweredCorrect
            };
        }
    }
}
