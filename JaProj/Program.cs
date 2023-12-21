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

        //[DllImport(@"C:\Users\sgork\source\repos\RepozytoriumPK4\lab1\JaProj\x64\Debug\JAAsm.dll")]
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());         
        }
    }
}
