using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsRain
{
    public partial class Form1 : Form
    {
        private Animator a;
        public Form1()
        {
            InitializeComponent();
            a = new Animator(panel1.CreateGraphics(), panel1.ClientRectangle);
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            a.Update(panel1.CreateGraphics(), panel1.ClientRectangle);
        }

        private bool start = false;
        private void button1_Click(object sender, EventArgs e)
        {
            start = true;
        }

        private bool rightWind = true;
        private bool leftWind = false;

        private void button2_Click(object sender, EventArgs e)
        {
            button4.BackColor = button6.BackColor; 
            button2.BackColor = Color.White;
            rightWind = true;
            leftWind = false;
            UpdateDx();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            button2.BackColor = button6.BackColor;
            rightWind = false;
            leftWind = true;
            UpdateDx();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(start) a.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            a.Stop();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            a.Stop();
            start = false;
        }
        private void UpdateDx()
        {
            if (rightWind)
            {
                button4.BackColor = button6.BackColor;
                button2.BackColor = Color.White;
                Drop.DX = trackBar1.Value;
            }
            if (leftWind)
            {
                button4.BackColor = Color.White;
                button2.BackColor = button6.BackColor;
                Drop.DX = -trackBar1.Value;
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateDx();
        }
       
        private void button5_Click(object sender, EventArgs e)
        {
            Drop.DX = 0;
            button4.BackColor = button6.BackColor;
            button2.BackColor = button6.BackColor;
            trackBar1.Value = 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Refresh();
            a.Stop();
            start = false;
        }
    }
}