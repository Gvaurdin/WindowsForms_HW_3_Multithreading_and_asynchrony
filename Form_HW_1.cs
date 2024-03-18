using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_HW_3_Multithreading_and_asynchrony
{
    public partial class Form_HW_1 : Form
    {
        private List<ProgressBar> progressBars;
        private Thread thread;
        private int count;
        private ManualResetEvent resetEvent = new ManualResetEvent(true);
        public Form_HW_1()
        {
            InitializeComponent();
            progressBars = new List<ProgressBar>();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            resetEvent.Set();
            count = int.Parse(textBoxCountProgressBars.Text);
            if (count > 5)
            {
                MessageBox.Show("Enter to correct value progress bars ( MAX 5)");
                return;
            }
            else if (progressBars == null || count != progressBars.Count)
            {
                int height = 70;
                for (int i = 0; i < count; i++)
                {
                    ProgressBar progressBar = new ProgressBar();
                    progressBar.Parent = this;
                    progressBar.Visible = true;
                    progressBar.Width = 200;
                    progressBar.Height = 30;
                    height += 40;
                    progressBar.Location = new Point(200, height);
                    progressBars.Add(progressBar);
                }
            }
            thread = new Thread(() => ChangeValueProgressBars());
            thread.Start();
        }

        private void ChangeValueProgressBars()
        {
            Random random = new Random();
            while (true)
            {
                foreach (var item in progressBars)
                {
                    if (item.InvokeRequired)
                    {
                        item.BeginInvoke(new Action(delegate ()
                        {
                            int value = random.Next(item.Minimum, item.Maximum + 1);
                            Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                            item.Value = value;
                            item.Style = ProgressBarStyle.Continuous;
                            item.ForeColor = randomColor;
                        }));
                    }
                }
                Thread.Sleep(200);
                resetEvent.WaitOne();
            }



        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            resetEvent.Reset();
        }

        private void Form_HW_1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }
        }
    }
}
