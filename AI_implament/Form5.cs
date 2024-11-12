using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_implament
{
    public partial class Form5 : Form
    {

        int a, b;
        Perceptron P = new Perceptron(2, 0.01);
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                a = int.Parse(this.textBox1.Text);
                b = int.Parse(this.textBox2.Text);
                if ((a != 1 && a != 0) || (b != 1 && b != 0))
                {
                    throw new Exception("Fall input");
                }

            }
            catch (Exception ex)
            {

                richTextBox1.Text += ex.Message;
            }
            double[] input = { a, b };
            double result = P.Predict(input);
            richTextBox1.Text = result.ToString("F1");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[][] trainData = {
            new double[] { 0, 0 }, // 0 AND 0 = 0
            new double[] { 0, 1}, // 0 AND 1 = 0
            new double[] { 1, 0 }, // 1 AND 0 = 0
            new double[] { 1, 1 }  // 1 AND 1 = 1
                };
            int[] resultData = { 0, 0, 0, 1 };
            for (int i = 0; i < 40; i++)
            {
                P.Train(trainData, resultData);
            }
        }
    }
}
