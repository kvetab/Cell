using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheCell
{
    public partial class Form1 : Form
    {
        //Cell cell;
        Model model;

        public Form1()
        {
            InitializeComponent();
            model = new Model(pictureBox1.CreateGraphics(), pictureBox1);
            //cell = new Cell();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            model.SetSun(trackBar1.Value);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            model.SetUV(trackBar2.Value);
        }

        private void button2_Click(object sender, EventArgs e)
            //pause
        {
            model.Pause();
            button3.Enabled = true;
            button4.Enabled = true;
            trackBar5.Enabled = true;
            //model.Test(textBox1);            
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            model.SetN(trackBar3.Value);
        }

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            model.SetP(trackBar4.Value);
        }

        private void button3_Click(object sender, EventArgs e)
            //reset
        {
            model = new Model(pictureBox1.CreateGraphics(), pictureBox1);
            richTextBox1.Enabled = false;
            trackBar1.Value = 50;
            trackBar2.Value = 50;
            trackBar3.Value = 50;
            trackBar4.Value = 50;
            model.Redraw();
        }

        private void button1_Click(object sender, EventArgs e)
            //start
        {
            button3.Enabled = false;
            button4.Enabled = false;
            richTextBox1.Enabled = false;
            richTextBox1.Visible = false;
            trackBar5.Enabled = false;
            model.Start();
            //model.OneTick();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Enabled = true;
            richTextBox1.Visible = true;
        }

        private void trackBar5_ValueChanged(object sender, EventArgs e)
        {
            model.SetInterval(trackBar5.Value);
        }
    }
}
