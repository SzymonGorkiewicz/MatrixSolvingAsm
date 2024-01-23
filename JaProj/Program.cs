using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JaProj
{
    internal class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());         
        }
    }

    public class Assembler
    {
        
        [DllImport(@"C:\Users\sgork\Desktop\JaProj\x64\Debug\JAAsm.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern void MyProc1(double[] matrices, int rows, int cols, int length, double[] solutions);
        public  int threads = Environment.ProcessorCount;
        private readonly Action<string> _updateLabelCallback;

        public Assembler(Action<string> updateLabelCallback)
        {
            _updateLabelCallback = updateLabelCallback;
        }
        public void SolveSystems(List<double[,]> matrices)
        {
            
            List<double[]> solutionsListASM = new List<double[]>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var options = new ParallelOptions { MaxDegreeOfParallelism = threads };
            for (int d = 0; d < 5; d++)
            {
                solutionsListASM.Clear();
                Parallel.ForEach(matrices, options, matrix =>
                {

                    double[] flattenedMatrix = FlattenMatrix(matrix);

                    int rows = matrix.GetLength(0);
                    int cols = matrix.GetLength(1);
                    int length = flattenedMatrix.GetLength(0);
                    double[] solutions = new double[rows];
                  
                    MyProc1(flattenedMatrix, rows, cols, length, solutions);

                    solutionsListASM.Add(solutions);

                });
            }
            stopwatch.Stop();
            TimeSpan timeElapsed = stopwatch.Elapsed;
            //Console.WriteLine($"Czas wykonania dla assemblera: {(timeElapsed.TotalMilliseconds) / 5} ms");
            _updateLabelCallback?.Invoke($"Czas wykonania dla ASM: {(timeElapsed.TotalMilliseconds) / 5} ms");
            string outputPath = "C://Users//sgork//Desktop//JaProj//JaProj//output_data//wynikiASM.txt";
            using (StreamWriter writer = new StreamWriter(outputPath))
            {

                for (int i = 0; i < solutionsListASM.Count; i++)
                {
                    writer.WriteLine($"Rozwiązania dla macierzy {i + 1}:");
                    if (solutionsListASM[i] != null)
                    {
                        for (int j = 0; j < solutionsListASM[i].Length; j++)
                        {
                            writer.WriteLine($"x{j + 1} = {solutionsListASM[i][j]}");
                        }
                    }
                  
                    writer.WriteLine(); 
                }

                MessageBox.Show("Rozwiązania zostały zapisane do pliku wynikowego.");
            }

        }

        private static double[] FlattenMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[] flattenedMatrix = new double[rows * cols];

            int currentIndex = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flattenedMatrix[currentIndex++] = matrix[i, j];
                }
            }

            return flattenedMatrix;
        }
    }
}
