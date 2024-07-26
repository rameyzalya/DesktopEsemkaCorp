﻿using DESKTOP_I_21.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_I_21
{

    public class AppContext : ApplicationContext { 
        
        public AppContext(Form form) 
        {
            form.FormClosed += Form_FormClosed;
            form.Show();
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            var count = Application.OpenForms.Cast<Form>().Where(f => f.TopLevel).Count();
            if (count == 0)
            {
                ExitThread();
                Application.Exit();
            }
        }
    }
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppContext(new LoginForm()));
        }
    }
}
