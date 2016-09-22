using System;
using System.Collections.Generic;

namespace DataStructure.Quizs
{
// ReSharper disable InconsistentNaming
    sealed class _130929MaxAndMinQuiz : AQuiz
// ReSharper restore InconsistentNaming
    {
        
        public _130929MaxAndMinQuiz()
        {
            ProblemDescription = "2013/10/1 3:26\n设计一个最优算法来查找一n个元素数组中的最大值和最小值。已知一种需要比较2n次的方法，请给一个更优的算法。请特别注意优化时间复杂度的常数。";
            _calculator = new QuickMaxAndMin();
        }

        protected override void Calculate()
        {
            int min, max;

            _calculator.Caculate(Input, out max, out min);

            Output = "Min = " + min + " , Max = " + max + ", Compared " + CompareCount + " times,  Assigned " + AssignCount + " times";
            NeedToPromote = false;
        }

        private ACaculateMaxAndMin _calculator;
    }

    abstract class ACaculateMaxAndMin :AQuiz
    {

        public abstract void Caculate(IList<int> input, out int max, out int min);

    }

    class QuickMaxAndMin : ACaculateMaxAndMin
    {

        public override void Caculate(IList<int> input, out int max, out int min)
        {
            _inputs = input;
            int first = 0;
            int last = input.Count-1;
            Cal(first, last);
            min = _inputs[0];
            max = _inputs[_inputs.Count - 1];
        }

        private void Cal(int first, int last)
        {
            int middle = (first + last)/2;

            Swap(first, last);

            if((first+last)%2 == 1)
            {
                Min(first, middle);
                Max(middle+1, last);
            }
            else
            {
                Min(first, middle);
                Max(middle, last);
            }
        }

        private void Swap(int first, int last)
        {
            int i = first;
            int j = last;
            for (; i < j; i++, j--)
            {
               CompareCount++;

                if (_inputs[i] > _inputs[j])
                {
                    int temp = _inputs[i];
                    _inputs[i] = _inputs[j];
                    _inputs[j] = temp;

                    AssignCount += 3;
                }
            }
        }

        private void Min(int first, int last)
        {
            if (first == last) return;
            Swap(first, last);
            Min(first, (first+last)/2);
        }

        private void Max(int first, int last)
        {
            if(first==last) return;
            Swap(first, last);
            Max((first + last + 1) / 2, last);
        }

        private IList<int> _inputs;
        protected override void Calculate()
        {
            throw new NotImplementedException();
        }
    }

    class _2NCompareMaxAndMin : ACaculateMaxAndMin
    {
        public override void Caculate(IList<int> input, out int max, out int min)
        {
            min = input[0];
            max = min;
            for (int i = 1; i < input.Count; i++)
            {
                CompareCount += 2;
                if (min > input[i])
                {
                    min = input[i];
                    AssignCount++;
                }
                    
                if (max < input[i])
                {
                    max = input[i];
                    AssignCount++;
                }
            }
        }

        protected override void Calculate()
        {
            throw new NotImplementedException();
        }
    }
}