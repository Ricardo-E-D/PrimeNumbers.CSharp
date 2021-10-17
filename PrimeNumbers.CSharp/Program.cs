using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PrimeNumbers.CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] mainArr = PopulateArray();
            int[] mainArrForParralel = PopulateArray();

            MeasureTime(() =>
            {
                GetPrimesSequential(mainArr);
            });

            MeasureTime(() =>
            {
                GetPrimesParallel(mainArrForParralel);
            });

            /// Observations:
            /// array length             sequential         parallel
            ///     1 000 000            0,036 sec.         0,076 sec.
            ///    10 000 000            0,388 sec.         0,225 sec.
            ///   100 000 000            3,616 sec.         1,208 sec.
            /// 1 000 000 000           42,050 sec.         12,115 sec.
        }
        public static int[] PopulateArray()
        {
            int min = 1;
            int max = 100;
            int arrLength = 1000000000;
            var rnd = new Random();

            int[] arr = new int[arrLength];
            //populate array of random values
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rnd.Next(min, max);
            }

            return arr;
        }

        public static void MeasureTime(Action ac)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ac.Invoke();
            sw.Stop();
            Console.WriteLine("         Time = {0:F5} sec.", sw.ElapsedMilliseconds / 1000f);
        }

        public static int[] GetPrimesSequential(int[] numbers)
        {
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] <= 1) numbers[i] = 0;
                else if (numbers[i] == 2) continue;
                else if (numbers[i] % 2 == 0) numbers[i] = 0;
                else
                {
                    for (int j = 3; j < i / 2; j += 2)
                        if (numbers[i] % j == 0)
                        {
                            numbers[i] = 0;
                            break;
                        }
                }
            }
            return numbers;
        }

        public static int[] GetPrimesParallel(int[] numbers)
        {
            Parallel.For(1, numbers.Length, i => {
                {
                    if (numbers[i] <= 1) numbers[i] = 0;
                    else if (numbers[i] == 2) return;
                    else if (numbers[i] % 2 == 0) numbers[i] = 0;
                    else
                    {
                        for (int j = 3; j < i / 2; j += 2)
                            if (numbers[i] % j == 0)
                            {
                                numbers[i] = 0;
                                break;
                            }
                    }
                }
            });
            return numbers;
        }
    }
}