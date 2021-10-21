using System;

namespace MatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int exp = 0; exp < 20; exp++)
            {
                var size = (int)Math.Pow(2, exp);

                long total = 0;
                var count = 0;

                for (int i = 0; i < 1000; i++)
                {
                    var ar1 = generateArrays(size);
                    var ar2 = generateArrays(size);

                    for (int j = 0; j < 20; j++)
                    {
                        var time = Driver(ar1, ar2);
                        total += time;
                        count++;
                    }
                }
                var avg = total / count;
                if (avg == 0)
                    Console.WriteLine($"Execution Time For Size {size}: neglagable");
                else
                    Console.WriteLine($"Execution Time For Size {size}: {total / count} ms");
            }
        }

        static int[][] generateArrays(int SIZE)
        {
            Random rnd = new Random();

            var array = new int[SIZE][];

            for (int i = 0; i < array.Length; i++)
            {
                var row1 = new int[SIZE];
                for (int j = 0; j < row1.Length; j++)
                {
                    row1[j] = rnd.Next(1, 16);
                }

                array[i] = row1;
            }

            return array;
        }

        static long Driver(int[][] ar1, int[][] ar2)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            BasicMultiplication(ar1, ar2);
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }

        static int[][] BasicMultiplication(int[][] ar1, int[][] ar2)
        {
            var result = new int[ar1.Length][];

            for (int i = 0; i < ar1.Length; i++)
            {
                result[i] = new int[ar1.Length];
                for (int j = 0; j < ar1[0].Length; j++)
                {
                    result[i][j] = 0;
                    for (int k = 0; k < ar1.Length; k++)
                    {
                        result[i][j] = result[i][j] + (ar1[i][k] * ar2[k][j]);
                    }
                }
            }

            return result;
        }

        static void PrintArray(int[][] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                PrintArray(array[i]);
            }
        }
        static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
                if (i != array.Length - 1)
                    Console.Write(", ");
                else
                    Console.Write("\n");
            }
        }
    }
}
