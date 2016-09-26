using System.Collections.Generic;
using System.Linq;

namespace DataStructure.Quizs
{
    class QuizRunner
    {

        public QuizRunner(IList<int> inputs)
        {
            _input = inputs;
        }

        public void Register(AQuiz aQuiz)
        {
            _quizzes.Add(aQuiz);
        }

        public void Run()
        {
            foreach (var quiz in _quizzes)
            {
                quiz.Input = _input.ToList();
                quiz.Run();
            }
        }

        private readonly IList<AQuiz> _quizzes = new List<AQuiz>();
        private readonly IList<int> _input;
    }
}
