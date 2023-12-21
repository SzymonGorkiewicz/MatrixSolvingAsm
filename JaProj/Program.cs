using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    public static class Gauss_Solver
    {
        [DllImport(@"C:\Users\sgork\Desktop\JaProj\x64\Debug\JAAsm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void gauss_solver(double[,] matrix, int size);
    }
}
