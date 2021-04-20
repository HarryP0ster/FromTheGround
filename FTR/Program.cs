using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace FTR
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Добро пожаловать в ад
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            global.form_menu = new Form1();
            Application.Run(global.form_menu);
        }
    }
}
