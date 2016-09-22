using System;
using System.Collections.Generic;

namespace DataStructure.Quizs
{
    abstract class AQuiz
    {
        public void Run()
        {
            DisplayProblemDescription();

            Calculate();

            DisplayResults();
        }

        #region Interface

        protected abstract void Calculate();

        #endregion


        #region Properties

        public IList<int> Input { protected get; set; }

        protected string ProblemDescription { get; set; }

        protected object Output { get; set; }

        protected bool NeedToPromote { get; set; }

        protected string WhyNeedToPromote { get; set; }

        public int Base { get; set; }
        public int CompareCount { get; set; }
        public int AssignCount { get; set; }


        #endregion


        #region Private Methods

        private void DisplayResults()
        {
            // BMK ABC
            Console.WriteLine("Input: " );
            Utility.DisplayCollection(Input);
            Console.WriteLine();
            Console.WriteLine("Result: " + Output);

            if(NeedToPromote)
            {
                Console.WriteLine("----Not Completed---- : " + WhyNeedToPromote);
            }

            Utility.DisplayHorizontalSplitter();
            Console.WriteLine();
        }

        private void DisplayProblemDescription()
        {
            Utility.DisplayInfoWithHorizontalSplitter(ProblemDescription);
        }

        #endregion

    }
}