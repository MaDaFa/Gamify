using Gamify.Sdk.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Sdk.IntegrationTests
{
    public class GameHelper
    {
        private static readonly Lazy<GameHelper> instance;

        private IDictionary<string, string> questions;
        private IDictionary<string, string> answers;
        private IDictionary<string, KeyValuePair<string, bool>> questionsMap;

        public static GameHelper Instance 
        {
            get
            {
                return instance.Value;
            }
        }

        static GameHelper()
        {
            instance = new Lazy<GameHelper>(() =>
            {
                return new GameHelper();
            });
        }

        public GameHelper()
        {
            this.questions = new Dictionary<string, string>();
            this.answers = new Dictionary<string, string>();
            this.BuildQuestions();
            this.BuildAnswers();
            this.BuildQuestionMap();
        }

        private void BuildQuestions()
        {
            this.questions = new Dictionary<string, string>();

            this.questions.Add("1", "Question 1?");
            this.questions.Add("2", "Question 2?");
            this.questions.Add("3", "Question 3?");
        }

        private void BuildAnswers()
        {
            this.answers = new Dictionary<string, string>();

            this.answers.Add("1", "Answer 1");
            this.answers.Add("2", "Answer 2");
            this.answers.Add("3", "Answer 3");
            this.answers.Add("4", "Answer 4");
            this.answers.Add("5", "Answer 5");
            this.answers.Add("6", "Answer 6");
            this.answers.Add("7", "Answer 7");
            this.answers.Add("8", "Answer 8");
            this.answers.Add("9", "Answer 9");
        }

        private void BuildQuestionMap()
        {
            this.questionsMap = new Dictionary<string, KeyValuePair<string, bool>>();

            this.questionsMap.Add("1", new KeyValuePair<string, bool>("1", false));
            this.questionsMap.Add("1", new KeyValuePair<string, bool>("2", true));
            this.questionsMap.Add("1", new KeyValuePair<string, bool>("3", false));
            this.questionsMap.Add("2", new KeyValuePair<string, bool>("4", true));
            this.questionsMap.Add("2", new KeyValuePair<string, bool>("5", false));
            this.questionsMap.Add("2", new KeyValuePair<string, bool>("6", false));
            this.questionsMap.Add("3", new KeyValuePair<string, bool>("7", true));
            this.questionsMap.Add("3", new KeyValuePair<string, bool>("8", false));
            this.questionsMap.Add("3", new KeyValuePair<string, bool>("9", false));
        }

        public string GetQuestion(string questionId)
        {
            return this.questions.FirstOrDefault(q => q.Key == questionId).Value;
        }

        public string GetAnswer(string answerId)
        {
            return this.answers.FirstOrDefault(q => q.Key == answerId).Value;
        }

        public bool IsCorrect(TestMoveObject move)
        {
            var answer = this.questionsMap
                .Where(q => q.Key == move.QuestionId)
                .First(q => q.Value.Key == move.AnswerId)
                .Value;

            return answer.Value;
        }
    }
}
