using System.Windows.Forms;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;

namespace VAN_Demo
{
    public partial class MyMessageBox : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// Find window by Caption only. Note you must pass IntPtr.Zero as the first parameter.
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        const UInt32 WM_CLOSE = 0x0010;

        static MyMessageBox newMessageBox;
        public MyMessageBox()
        {
            InitializeComponent();
        }

        public static void ShowBox(string txtMessage)
        {
            string[] msg = txtMessage.Split(';');
            newMessageBox = new MyMessageBox();
            newMessageBox.TopMost = true;
            try
            {
                newMessageBox.label1.Text = msg[0];
                newMessageBox.label2.Text = msg[1];
                newMessageBox.label3.Text = msg[2];
            }
            catch
            { 
            }
            newMessageBox.ShowDialog();    
        }

        public static void close()
        {
            IntPtr windowPtr = FindWindowByCaption(IntPtr.Zero, "Warning");
            SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
