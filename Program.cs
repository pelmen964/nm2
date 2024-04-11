using System;
using System.IO;
using System.Text;

namespace nm2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            uint methodNumber,matrixRank;
            double[,] taskMatrix;
            double elem;
            string[] border;

            using (var sr = new StreamReader("Task.txt"))
            {
                methodNumber = Convert.ToUInt32(sr.ReadLine());
                matrixRank = Convert.ToUInt32(sr.ReadLine());
                taskMatrix = new double[matrixRank, matrixRank + 1];
                for (int i = 0; i < matrixRank; i++)
                {
                    border = sr.ReadLine().Split();
                    for (int j = 0; j < matrixRank+1; j++)
                    {
                        taskMatrix[i,j] = Convert.ToDouble(border[j]);
                    }
                }
            }

            Task t = new Task(matrixRank, taskMatrix);
            t.Solve(methodNumber, "outTask.txt");
        }
    }
}