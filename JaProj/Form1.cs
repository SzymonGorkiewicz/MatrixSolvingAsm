using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JaProj
{
    public partial class Form1 : Form
    {

        

        private GaussElimination gauss= new GaussElimination();
        GaussAlgorithm.Class1 GaussAlg = new GaussAlgorithm.Class1();
        private Assembler asm;
        public List<double[,]> matrices;
        private List<double[]> solutionsList;
        public Form1()
        {
            asm = new Assembler(UpdateLabel);
            InitializeComponent();
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 64;
            numericUpDown1.Value = GaussAlg.threads;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Pobierz ścieżkę do wybranego pliku
                string filePath = openFileDialog.FileName;

                // Wywołaj funkcję wczytującą układy równań z pliku
                matrices = await Task.Run(() => gauss.ReadMatricesFromFile(filePath));
                if (matrices != null)
                {

                    label1.Text = $"{filePath} read";

                }
                else
                {
                    label1.Text = "Error reading matrices from file";
                }
            }
            
            
        }

        public void UpdateLabel(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateLabel), text);
                return;
            }

            label4.Text = text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
            if (matrices != null && matrices.Count > 0)
            {
                // Wywołaj funkcję rozwiązującą układy równań
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < 5; i++)
                {
                    solutionsList = GaussAlg.SolveSystems(matrices);
                }
                stopwatch.Stop();
                TimeSpan timeElapsed = stopwatch.Elapsed;
                
                label4.Text = $"Czas wykonania dla C#: {(timeElapsed.TotalMilliseconds) / 5} ms";
                // Ścieżka do pliku wynikowego
                string outputPath = "C://Users//sgork//Desktop//JaProj//JaProj//output_data//wyniki.txt";

                // Utwórz lub nadpisz plik wynikowy
                using (StreamWriter writer = new StreamWriter(outputPath))
                {
                    
                    for (int i = 0; i < solutionsList.Count; i++)
                    {
                        writer.WriteLine($"Rozwiązania dla macierzy {i + 1}:");
                        if (solutionsList[i] != null)
                        {
                            for (int j = 0; j < solutionsList[i].Length; j++)
                            {
                                writer.WriteLine($"x{j + 1} = {solutionsList[i][j]}");
                            }
                        }
                        else
                        {
                            //Console.WriteLine($"Brak rozwiązań dla macierzy {i + 1}.");
                        }
                        writer.WriteLine(); 
                    }

                    MessageBox.Show("Rozwiązania zostały zapisane do pliku wynikowego. ");
                }
            }
            else
            {
                MessageBox.Show("Wczytaj najpierw plik z układem równań.");
            }

        }

    

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        
        private void button3_Click_1(object sender, EventArgs e)
        {
            asm.SolveSystems(matrices);
            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;

            if (numericUpDown != null)
            {
                
                decimal value = numericUpDown.Value;
                int intValue = Convert.ToInt32(value);
                GaussAlg.threads = intValue;
                asm.threads = intValue;
                label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
