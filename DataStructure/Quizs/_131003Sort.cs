using System.Linq;

namespace DataStructure.Quizs
{
// ReSharper disable InconsistentNaming
    class _131003Sort : AQuiz
// ReSharper restore InconsistentNaming
    {
        public _131003Sort()
        {
            ProblemDescription = "2013/10/3 21:33\t排序算法";
        }

        protected override void Calculate()
        {
            //var sorter = new bubbleSort();
            //var input = Input.ToList();
            //sorter.Sort(input);

            //Console.WriteLine("1.冒泡排序:");
            //Utility.DisplayCollection(input);

            var quizRunner = new QuizRunner(DataGenerator.GenerateRandomArray(20, 100).ToList());
            quizRunner.Register(new BubbleSort());
            quizRunner.Register(new SelectionSort());
            quizRunner.Register(new InsertSort());

            quizRunner.Run();
        }
    }

    abstract class ASort: AQuiz
    {

        protected abstract void Sort();

        protected override void Calculate()
        {

            Base = Input.Count;

            Sort();

            Output = "Base = " + Base + " , Compared " + CompareCount + " times, Assigned " +
                     AssignCount + " times";
        }
    }

    class InsertSort : ASort
    {
        public InsertSort()
        {
            ProblemDescription =
                "2013/10/4 21:01\t3.插入排序:将下一个插入已排好的序列中\n最佳效率O（n）；最糟效率O（n²）与冒泡、选择相同，适用于排序小列表\n若列表基本有序，则插入排序比冒泡、选择更有效率。";

        }

        protected override void Sort()
        {
            for (int i = 1; i < Base-1; i++)
            {
                int temp = Input[i];
                for (int j = i-1; j >= 0; j--)
                {
                    CompareCount++;
                    if (Input[j] <= temp)
                    {
                        Input[j + 1] = temp;
                        AssignCount++;
                        break;
                    }
                    Input[j + 1] = Input[j];
                    AssignCount++;
                }
            }
        }
    }

    class SelectionSort : ASort
    {

        public SelectionSort()
        {
            ProblemDescription = "2013/10/4 20:20\t2.选择排序:——每次最小/大排在相应的位置, 效率 O（n²）,适用于排序小列表。\n它与冒泡排序的差别:\n 冒泡排序只要发现比较的两个数字顺序与排序顺序相反就会进行交换操作，所以每一轮的比较可能需要进行多次交换操作；选择排序比较时每次只会记录下最小的（或者最大的）的位置，一轮比较完成之后才会进行对应位置和最小位置（或者最大位置）的交换操作，所以每一轮的比较只做一次交换操作。\n 所以选择排序似乎好点";
        }

        protected override void Sort()
        {
            for (int i = 0; i < Base-1; i++)
            {
                int minLocation = i;
                int min = Input[i];

                for (int j = i+1; j < Base; j++)
                {
                    CompareCount++;

                    if (Input[j] < min)
                    {
                        minLocation = j;
                        min = Input[j];

                        AssignCount++;
                    }
                }

                if (minLocation != i)
                {
                    int temp = Input[i];
                    Input[i] = Input[minLocation];
                    Input[minLocation] = temp;

                    AssignCount += 3;
                }
            }
        }
    }

    class BubbleSort : ASort
    {

        public BubbleSort()
        {
            ProblemDescription = "2013/10/3 22:33\t1.冒泡排序: 效率 O（n²）,适用于排序小列表。 ";
        }

        protected override void Sort()
        {

            for (int i = 0; i < Base - 1; i++)
            {
                for (int j = Base - 1; j > i; j--)
                {
                    CompareCount++;
                    if (Input[j] < Input[j - 1])
                    {
                        int temp = Input[j];
                        Input[j] = Input[j - 1];
                        Input[j - 1] = temp;

                        AssignCount += 3;
                    }
                }
            }
        }
    }
}

