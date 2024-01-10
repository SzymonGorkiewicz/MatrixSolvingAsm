﻿using System;
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

        [DllImport(@"C:\Users\sgork\Desktop\JaProj\x64\Debug\JAAsm.dll", CallingConvention = CallingConvention.Cdecl)]
       
        public static extern int MyProc1(int a, int b, int c);

        private GaussElimination gauss= new GaussElimination();
        GaussAlgorithm.Class1 GaussAlg = new GaussAlgorithm.Class1();
        public List<double[,]> matrices;
        private List<double[]> solutionsList;
        public Form1()
        {
            InitializeComponent();
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
                matrices = gauss.ReadMatricesFromFile(filePath);
                if (matrices != null)
                {
                    // Wyświetl wczytane macierze w konsoli
                    for (int i = 0; i < matrices.Count; i++)
                    {
                        Console.WriteLine($"Matrix {i + 1}:");

                        for (int row = 0; row < matrices[i].GetLength(0); row++)
                        {
                            for (int col = 0; col < matrices[i].GetLength(1); col++)
                            {
                                Console.Write($"{matrices[i][row, col]} ");
                            }
                            Console.WriteLine();
                        }

                        Console.WriteLine();
                    }

                    label1.Text = $"{filePath} readed";
                }
                else
                {
                    label1.Text = "Error reading matrices from file";
                }
            }
            
            
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
                long elapsedTicks = stopwatch.ElapsedTicks;
                double nanoseconds = (double)elapsedTicks / Stopwatch.Frequency * 1e6;
                Console.WriteLine($"Czas wykonania: {(nanoseconds)/5} mikroseconds");

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
                        writer.WriteLine(); // Dodaj pustą linię między rozwiązaniami macierzy
                    }

                    MessageBox.Show("Rozwiązania zostały zapisane do pliku wynikowego.");
                }
            }
            else
            {
                MessageBox.Show("Wczytaj najpierw plik z układem równań.");
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = 1;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = Environment.ProcessorCount;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = 2;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = 4;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = 8;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = 16;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = 32;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            GaussAlg.threads = 64;
            label3.Text = "Wybrana ilosc watkow: " + GaussAlg.threads;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int a = MyProc1(1, 2, 3);
            Console.WriteLine(a);
        }
    }
}
