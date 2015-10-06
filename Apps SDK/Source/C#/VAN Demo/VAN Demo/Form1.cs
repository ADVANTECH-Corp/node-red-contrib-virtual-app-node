using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using VANLibClass;

namespace VAN_Demo
{
    public partial class Form1 : Form
    {
        ThreadStart myRun = null;
        Thread myThread = null;
        VANLib VANExport = new VANLib();
        VANLib VANDelegate = new VANLib();

        public void VANExportCallback(string envelope, string message)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (textBox_Export_Token.Text == "")
                    textBox1.AppendText("Receive: " + message + Environment.NewLine);
                else
                    textBox1.AppendText("Receive: [ " + envelope + " ] " + message + Environment.NewLine);

                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }));
        }

        public string VANDelegateCallback(string envelope, string message)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (textBox_Delegate_Token.Text == "")
                    textBox2.AppendText("Receive: " + message + Environment.NewLine);
                else
                    textBox2.AppendText("Receive: [ " + envelope + " ] " + message + Environment.NewLine);

                textBox2.SelectionStart = textBox2.Text.Length;
                textBox2.ScrollToCaret();
            }));
            double i;
            if (Double.TryParse(message, System.Globalization.NumberStyles.Float, null, out i))
            {
                if (Double.Parse(message) > 3000)
                {
                    textBox2.ForeColor = Color.Red;
                    return "Temperature Warning";
                }
                else if (Double.Parse(message) < 2000)
                {
                    textBox2.ForeColor = Color.OrangeRed;
                    return "Fan speed abnmormal";
                }
                else
                {
                    textBox2.ForeColor = Color.Black;
                    return "Normal";
                }
            }
            return "Undefine";
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
             if (myThread != null && myThread.IsAlive)
                 myThread.Abort();
        }

        private void button_Import_Click(object sender, EventArgs e)
        {
            VANLib VANImport = new VANLib();
            String message = VANImport.Import(textBox_Import_Token.Text.ToString(), textBox_Import_Message.Text.ToString());
            textBox3.AppendText("Receive: " + message + Environment.NewLine);
            textBox3.SelectionStart = textBox2.Text.Length;
            textBox3.ScrollToCaret();
        }

        private void button_Export_Click(object sender, EventArgs e)
        {
            if (button_Export.Text == "Start")
            {
                myRun = new ThreadStart(() => VANExport.Export(textBox_Export_Token.Text.ToString(), VANExportCallback));
                myThread = new Thread(myRun);
                myThread.IsBackground = true;
                myThread.Start();
                button_Export.Text = "Stop";
            }
            else if (button_Export.Text == "Stop")
            {
                if (myThread != null && myThread.IsAlive)
                    myThread.Abort();
                button_Export.Text = "Start";
            }
        }

        private void button_Delegate_Click(object sender, EventArgs e)
        {
            if (button_Delegate.Text == "Start")
            {
                myRun = new ThreadStart(() => VANDelegate.Delegate(textBox_Delegate_Token.Text.ToString(), VANDelegateCallback));
                myThread = new Thread(myRun);
                myThread.IsBackground = true;
                myThread.Start();
                button_Delegate.Text = "Stop";
            }
            else if (button_Delegate.Text == "Stop")
            {
                if (myThread != null && myThread.IsAlive)
                    myThread.Abort();
                button_Delegate.Text = "Start";
            }
        }
    }
}
