using System;

namespace MatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Which system would you like to run?");
            Console.WriteLine("0 - Basic, 1 - Divide and Conquer");
            Console.Write("Choice: ");
            var response = int.Parse(Console.ReadLine());

            for (int exp = 0; exp < 20; exp++)
            {
                var size = (int)Math.Pow(2, exp);

                double total = 0;
                var count = 0;

                for (int i = 0; i < 1000; i++)
                {
                    var ar1 = generateArrays(size);
                    var ar2 = generateArrays(size);

                    for (int j = 0; j < 20; j++)
                    {
                        double time = 0;
                        if (response == 0)
                            time = BasicMultiplicationTestRun(ar1, ar2);
                        else if (response == 1)
                            time = DivideAndConquerTestRun(ar1, ar2);
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

        static double BasicMultiplicationTestRun(int[][] ar1, int[][] ar2)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            BasicMultiplication(ar1, ar2);
            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
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

        static double DivideAndConquerTestRun(int[][] ar1, int[][] ar2)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            DivideAndConquer(ar1, ar2);
            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }

        static int[][] DivideAndConquer(int[][] ar1, int[][] ar2)
        {
            var result = new int[ar1.Length][];

            var newSize = ar1.Length / 2;

            if (newSize == 0)
                return result;

            var A11 = new int[newSize][];
            var A12 = new int[newSize][];
            var A21 = new int[newSize][];
            var A22 = new int[newSize][];

            var B11 = new int[newSize][];
            var B12 = new int[newSize][];
            var B21 = new int[newSize][];
            var B22 = new int[newSize][];

            for (int i = 0; i < newSize; i++)
            {
                A11[i] = new int[newSize];
                A12[i] = new int[newSize];
                A21[i] = new int[newSize];
                A22[i] = new int[newSize];

                B11[i] = new int[newSize];
                B12[i] = new int[newSize];
                B21[i] = new int[newSize];
                B22[i] = new int[newSize];
            }

            for (int i = 0; i < ar1.Length; i++)
            {
                for (int j = 0; j < ar1[i].Length; j++)
                {
                    if (j < newSize)
                    {
                        if (i < newSize)
                        {
                            //A11
                            A11[i % newSize][j % newSize] = ar1[i][j];
                            B11[i % newSize][j % newSize] = ar2[i][j];
                        }
                        else
                        {
                            //A21
                            A21[i % newSize][j % newSize] = ar1[i][j];
                            B21[i % newSize][j % newSize] = ar2[i][j];
                        }
                    }
                    else
                    {
                        if (i < newSize)
                        {
                            //A12
                            A12[i % newSize][j % newSize] = ar1[i][j];
                            B12[i % newSize][j % newSize] = ar2[i][j];
                        }
                        else
                        {
                            //A22
                            A22[i % newSize][j % newSize] = ar1[i][j];
                            B22[i % newSize][j % newSize] = ar2[i][j];
                        }
                    }
                }
            }

            var C11 = AddMatricies(BasicMultiplication(A11, B11), BasicMultiplication(A12, B21));
            var C12 = AddMatricies(BasicMultiplication(A11, B12), BasicMultiplication(A12, B22));
            var C21 = AddMatricies(BasicMultiplication(A21, B11), BasicMultiplication(A22, B21));
            var C22 = AddMatricies(BasicMultiplication(A21, B12), BasicMultiplication(A22, B22));

            //won't combine the matricies since that takes unncessarily computational time that takes away from the main point of the algorithm

            return result;
        }

        static int[][] AddMatricies(int[][] ar1, int[][] ar2)
        {
            var result = new int[ar1.Length][];

            for (int i = 0; i < ar1.Length; i++)
            {
                for (int j = 0; j < ar1[0].Length; j++)
                {
                    if (j == 0)
                        result[i] = new int[ar1[0].Length];
                    result[i][j] = ar1[i][j] + ar2[i][j];
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
