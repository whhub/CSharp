using System;
using System.Collections.Generic;

namespace DataStructure.Quizs
{
// ReSharper disable InconsistentNaming
    class _131002_2To10 : AQuiz
// ReSharper restore InconsistentNaming
    {
        public _131002_2To10()
        {
            ProblemDescription = "2013/10/2 1:47\n 利用栈的数据结构特点，将二进制转换为十进制数";
        }

        protected override void Calculate()
        {
            var statck = new Stack<int>();

            Console.WriteLine("输入一个二进制数:");

            int inputKey = 0;

            while (inputKey == 0 || inputKey==1)
            {
                statck.Push(inputKey);
                inputKey = Console.Read();
                inputKey -= '0';
            }

            Console.WriteLine("对应的十进制数为: ");

            int weight = 1;
            int result = 0;
            while (statck.Count !=0 )
            {
                result += weight*statck.Pop();
                weight *= 2;
            }

            Console.WriteLine(result);

            Output = result;
        }
    }
}