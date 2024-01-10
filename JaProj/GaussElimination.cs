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

    }
}
