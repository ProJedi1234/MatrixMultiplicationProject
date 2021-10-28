using System;

namespace MatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get Run Choice
            Console.WriteLine("Which system would you like to run?");
            Console.WriteLine("0 - Basic, 1 - Divide and Conquer, 2 - Strassen");
            Console.Write("Choice: ");
            var response = int.Parse(Console.ReadLine());
            Console.WriteLine("\n\nRun Style");
            Console.WriteLine("0 - Quick, 1 - Full Test");
            Console.Write("Choice: ");
            var runStyle = int.Parse(Console.ReadLine());

            // Generate 20 Sets
            for (int exp = 0; exp < 20; exp++)
            {
                var size = (int)Math.Pow(2, exp);

                double total = 0;
                var count = 0;

                // Run each set 1000 times (or once if the run time is simple)
                for (int i = 0; i < (runStyle == 1 ? 1000 : 1); i++)
                {
                    var ar1 = generateArrays(size);
                    var ar2 = generateArrays(size);

                    for (int j = 0; j < (runStyle == 1 ? 20 : 1); j++)
                    {
                        double time = 0;
                        if (response == 0)
                            time = BasicMultiplicationTestRun(ar1, ar2);
                        else if (response == 1)
                            time = DivideAndConquerTestRun(ar1, ar2);
                        else if (response == 2)
                            time = StrassenTestRun(ar1, ar2);
                        total += time; //add time
                        count++; // add to count
                    }
                }
                var avg = total / count; // calculate average
                if (avg == 0)
                    Console.WriteLine($"Execution Time For Size {size}: neglagable");
                else
                    Console.WriteLine($"Execution Time For Size {size}: {total / count} ms");
            }
        }
        #region BasicMultiplication
        static double BasicMultiplicationTestRun(int[][] ar1, int[][] ar2)
        {
            var watch = new System.Diagnostics.Stopwatch(); //create a stop watch to time the algorithm
            watch.Start();
            BasicMultiplication(ar1, ar2);
            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }

        static int[][] BasicMultiplication(int[][] ar1, int[][] ar2)
        {
            var result = new int[ar1.Length][];

            //triple for loop to follow the formula
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
        #endregion
        #region Divide and Conquer
        static double DivideAndConquerTestRun(int[][] ar1, int[][] ar2)
        {
            var watch = new System.Diagnostics.Stopwatch(); //create a stop watch to time the algorithm
            watch.Start();
            DivideAndConquer(ar1, ar2);
            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }

        static int[][] DivideAndConquer(int[][] ar1, int[][] ar2)
        {
            var result = InitializeMatrix(ar1.Length);

            var newSize = ar1.Length / 2; //find the halfway point

            if (newSize <= 1)
                return BasicMultiplication(ar1, ar2); //Base Case

            //Initialize all the new matricies
            int[][] A11 = InitializeMatrix(newSize);
            int[][] A12 = InitializeMatrix(newSize);
            int[][] A21 = InitializeMatrix(newSize);
            int[][] A22 = InitializeMatrix(newSize);
            int[][] B11 = InitializeMatrix(newSize);
            int[][] B12 = InitializeMatrix(newSize);
            int[][] B21 = InitializeMatrix(newSize);
            int[][] B22 = InitializeMatrix(newSize);

            // Splitting algorithm
            for (int i = 0; i < newSize; i++)
                for (int j = 0; j < newSize; j++)
                {
                    A11[i][j] = ar1[i][j];
                    A12[i][j] = ar1[i][newSize + j];
                    A21[i][j] = ar1[newSize + i][j];
                    A22[i][j] = ar1[newSize + i][newSize + j];
                    B11[i][j] = ar2[i][j];
                    B12[i][j] = ar2[i][newSize + j];
                    B21[i][j] = ar2[newSize + i][j];
                    B22[i][j] = ar2[newSize + i][newSize + j];
                }

            //Recursively continue the formula
            var C11 = AddMatricies(DivideAndConquer(A11, B11), DivideAndConquer(A12, B21));
            var C12 = AddMatricies(DivideAndConquer(A11, B12), DivideAndConquer(A12, B22));
            var C21 = AddMatricies(DivideAndConquer(A21, B11), DivideAndConquer(A22, B21));
            var C22 = AddMatricies(DivideAndConquer(A21, B12), DivideAndConquer(A22, B22));

            //Put it back together
            for (int i = 0; i < newSize; i++)
                for (int j = 0; j < newSize; j++)
                {
                    result[i][j] = C11[i][j];
                    result[i][j + newSize] = C12[i][j];
                    result[newSize + i][j] = C21[i][j];
                    result[newSize + i][newSize + j] = C22[i][j];
                }

            return result;
        }
        #endregion
        #region Strassen
        static double StrassenTestRun(int[][] ar1, int[][] ar2)
        {
            var watch = new System.Diagnostics.Stopwatch(); //create a stop watch to time the algorithm
            watch.Start();
            Strassen(ar1, ar2);
            watch.Stop();

            return watch.Elapsed.TotalMilliseconds;
        }

        static int[][] Strassen(int[][] A, int[][] B)
        {
            if (A.Length <= 2)
                return BasicMultiplication(A, B); //Base Case


            int[][] C = InitializeMatrix(A.Length);
            int k = A.Length / 2; //Find halfway point

            //Initialize all the new matricies
            int[][] A11 = InitializeMatrix(k);
            int[][] A12 = InitializeMatrix(k);
            int[][] A21 = InitializeMatrix(k);
            int[][] A22 = InitializeMatrix(k);
            int[][] B11 = InitializeMatrix(k);
            int[][] B12 = InitializeMatrix(k);
            int[][] B21 = InitializeMatrix(k);
            int[][] B22 = InitializeMatrix(k);

            // Splitting algorithm
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {
                    A11[i][j] = A[i][j];
                    A12[i][j] = A[i][k + j];
                    A21[i][j] = A[k + i][j];
                    A22[i][j] = A[k + i][k + j];
                    B11[i][j] = B[i][j];
                    B12[i][j] = B[i][k + j];
                    B21[i][j] = B[k + i][j];
                    B22[i][j] = B[k + i][k + j];
                }

            //Recursive algorithm
            int[][] P1 = Strassen(A11, SubtractMatricies(B12, B22));
            int[][] P2 = Strassen(AddMatricies(A11, A12), B22);
            int[][] P3 = Strassen(AddMatricies(A21, A22), B11);
            int[][] P4 = Strassen(A22, SubtractMatricies(B21, B11));
            int[][] P5 = Strassen(AddMatricies(A11, A22), AddMatricies(B11, B22));
            int[][] P6 = Strassen(SubtractMatricies(A12, A22), AddMatricies(B21, B22));
            int[][] P7 = Strassen(SubtractMatricies(A11, A21), AddMatricies(B11, B12));

            int[][] C11 = SubtractMatricies(AddMatricies(AddMatricies(P5, P4), P6), P2);
            int[][] C12 = AddMatricies(P1, P2);
            int[][] C21 = AddMatricies(P3, P4);
            int[][] C22 = SubtractMatricies(SubtractMatricies(AddMatricies(P5, P1), P3), P7);

            //Put it back together
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                {
                    C[i][j] = C11[i][j];
                    C[i][j + k] = C12[i][j];
                    C[k + i][j] = C21[i][j];
                    C[k + i][k + j] = C22[i][j];
                }

            return C;
        }
        #endregion
        #region Helpers
        static int[][] AddMatricies(int[][] ar1, int[][] ar2)
        {
            var result = new int[ar1.Length][];

            for (int i = 0; i < ar1.Length; i++)
            {
                for (int j = 0; j < ar1[0].Length; j++)
                {
                    if (j == 0)
                        result[i] = new int[ar1[0].Length]; //New Row
                    result[i][j] = ar1[i][j] + ar2[i][j]; //Add Values
                }
            }

            return result;
        }
        static int[][] SubtractMatricies(int[][] ar1, int[][] ar2)
        {
            var result = new int[ar1.Length][];

            for (int i = 0; i < ar1.Length; i++)
            {
                for (int j = 0; j < ar1[0].Length; j++)
                {
                    if (j == 0)
                        result[i] = new int[ar1[0].Length]; //New Row
                    result[i][j] = ar1[i][j] - ar2[i][j]; //Subtract Values
                }
            }

            return result;
        }
        static int[][] InitializeMatrix(int k)
        {
            var result = new int[k][];

            for (int i = 0; i < result.Length; i++)
                result[i] = new int[k]; //generate rows

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
        static int[][] generateArrays(int SIZE)
        {
            Random rnd = new Random();

            var array = new int[SIZE][];

            for (int i = 0; i < array.Length; i++)
            {
                var row1 = new int[SIZE];
                for (int j = 0; j < row1.Length; j++)
                {
                    row1[j] = rnd.Next(1, 16); //create random number
                }

                array[i] = row1;
            }

            return array;
        }
        #endregion
    }
}
