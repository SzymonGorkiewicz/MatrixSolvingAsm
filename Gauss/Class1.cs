using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaussAlgorithm
{
    public class Class1
    {
        public int threads = Environment.ProcessorCount;
        public List<double[]> SolveSystems(List<double[,]> matrices)
        {
            List<double[]> solutionsList = new List<double[]>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = threads };
            // Iteruj po macierzach równolegle
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

            return solutionsList;
        }
    }
}
