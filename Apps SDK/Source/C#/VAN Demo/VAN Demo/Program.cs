using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZeroMQ;
using System.Diagnostics;
using System.Text;

namespace VAN_Demo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ///* Unmanaged dll */
            EmbeddedDll.LoadDll("libzmq");

            ///* Managed dll */
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                return EmbeddedDll.LoadDll(new System.Reflection.AssemblyName(args.Name).Name);
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());  
        }
    }
}
