using System.Linq;
using DataStructure.Quizs;

namespace DataStructure
{
    class Program
    {
        static void Main()
        {
            var ints = DataGenerator.GenerateRandomArray(30, 100);

            var quizRunner = new QuizRunner(ints.ToList());

            ////////////////////////////////////////////////////////////////////////////////
            //Quiz Registeration,  later on bottom
            ////////////////////////////////////////////////////////////////////////////////
            
            quizRunner.Register(new _130929MaxAndMinQuiz());

            quizRunner.Register(new _131002_2To10());

            quizRunner.Register(new _131003Sort());

            ////////////////////////////////////////////////////////////////////////////////

            quizRunner.Run();
        }
    }
}
