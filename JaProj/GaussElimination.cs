using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JaProj
{
    internal class GaussElimination
    {
        
        public int threads = Environment.ProcessorCount;
        public List<double[,]> ReadMatricesFromFile(string filePath)
        {
            List<double[,]> matricesList = new List<double[,]>();
            List<double[]> currentMatrix = new List<double[]>();

            foreach (string line in File.ReadLines(filePath))
            {
                if (line.Contains("[matrix]"))
                {
                    // Rozpocznij nową macierz
                    currentMatrix = new List<double[]>();
                }
                else if (line.Contains("[end]"))
                {
                    // Zakończ bieżącą macierz i dodaj ją do listy
                    matricesList.Add(ConvertListToMatrix(currentMatrix));
                }
                else
                {
                    // Usuń nawiasy kwadratowe na początku i końcu linii
                    string cleanedLine = line.Trim('[', ']');

                    string[] values = cleanedLine.Split(new char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    double[] row = new double[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (!double.TryParse(values[i], out row[i]))
                        {
                            Console.WriteLine($"Błąd parsowania wartości w linii: {line}");
                            return null; // Lub inny sposób obsługi błędu
                        }
                    }

                    currentMatrix.Add(row);
                }
            }

            return matricesList;
        }

        private double[,] ConvertListToMatrix(List<double[]> matrixList)
        {
            int rows = matrixList.Count;
            int columns = matrixList.First().Length;

            double[,] matrix = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = matrixList[i][j];
                }
            }

            return matrix;
        }

        public List<double[]> SolveSystems(List<double[,]> matrices)
        {
            List<double[]> solutionsList = new List<double[]>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = threads };
            // Iteruj po macierzach równolegle
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.ForEach(matrices, options, matrix =>
            {
                int n = matrix.GetLength(0);

                // Przekształć macierz do postaci trójkątnej górnej
                for (int i = 0; i < n; i++)
                {
                    // Szukaj niezerowego elementu w kolumnie i
                    int nonZeroRow = -1;
                    for (int j = i; j < n; j++)
                    {
                        if (matrix[j, i] != 0)
                        {
                            nonZeroRow = j;
                            break;
                        }
                    }

                    // Jeśli nie znaleziono niezerowego elementu, układ równań jest sprzeczny
                    if (nonZeroRow == -1)
                    {
                        throw new InvalidOperationException("Układ równań jest sprzeczny.");
                    }

                    // Zamień wiersze
                    for (int k = 0; k < n + 1; k++)
                    {
                        double temp = matrix[i, k];
                        matrix[i, k] = matrix[nonZeroRow, k];
                        matrix[nonZeroRow, k] = temp;
                    }

                    // Normalizuj wiersz i
                    double factor = matrix[i, i];
                    if (factor == 0)
                    {
                        throw new InvalidOperationException("Dzielenie przez zero. Dodaj obsługę tej sytuacji.");
                    }

                    for (int k = i; k < n + 1; k++)
                    {
                        matrix[i, k] /= factor;
                    }

                    // Wyzeruj poniżej i-tej kolumny
                    for (int j = i + 1; j < n; j++)
                    {
                        factor = matrix[j, i] / matrix[i, i];
                        for (int k = i; k < n + 1; k++)
                        {
                            matrix[j, k] -= factor * matrix[i, k];
                        }
                    }
                }

                // Rozwiązanie układu równań z macierzą trójkątną górną
                double[] solutions = new double[n];
                for (int i = n - 1; i >= 0; i--)
                {
                    double sum = 0;
                    for (int j = i + 1; j < n; j++)
                    {
                        sum += matrix[i, j] * solutions[j];
                    }
                    solutions[i] = matrix[i, n] - sum;
                    solutions[i] = Math.Round(solutions[i], 2);
                }

                solutionsList.Add(solutions);
            });
            stopwatch.Stop();
            Console.WriteLine($"Czas wykonania dla {threads} watków : {stopwatch.ElapsedMilliseconds} ms");
            return solutionsList;
        }    
    }
}
