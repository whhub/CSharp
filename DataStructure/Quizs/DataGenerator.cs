using System;
using System.Collections.Generic;

namespace DataStructure.Quizs
{
    class DataGenerator
    {
        static public IEnumerable<int> GenerateRandomArray(int count, int max, int min=0)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                yield return random.Next(min, max);
            }
        }
    }
}
