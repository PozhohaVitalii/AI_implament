using OpenTK.Input;
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
    public partial class Form6 : Form
    {
        public int NumbOFclasses = 0;
        public static List<Class2> ClassesBlackBox = new List<Class2>();
        List<Form> classForm = new List<Form>();
        List<Button> buttons = new List<Button>();


        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] layerSizes = { 2, 3, 1 };
           // var mlp = new MLP(layerSizes, ActivationFunction.Sigmoid);

            // Вхідні дані (наприклад, логічна функція AND)
            double[][] inputs =
            {
            new double[] { 0, 0 },
            new double[] { 0, 1 },
            new double[] { 1, 0 },
            new double[] { 1, 1 }
        };

            // Очікувані результати для функції AND
            double[][] targets =
            {
            new double[] { 0 },
            new double[] { 0 },
            new double[] { 0 },
            new double[] { 1 }
        };

            // Навчання моделі
            int epochs = 10000;
            double learningRate = 0.1;
           // mlp.Train(inputs, targets, epochs, learningRate);

            // Тестування на нових даних
            Console.WriteLine("Testing MLP on AND function:");
            foreach (var input in inputs)
            {
              //  double[] output = mlp.CalculateOutput(input);
               // Console.WriteLine($"Input: [{string.Join(", ", input)}] -> Output: {output[0]:F4}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NumbOFclasses++;
            ClassesBlackBox.Add(new Class2());
            classForm.Add(new Form3(NumbOFclasses));
            classForm[classForm.Count - 1].ShowDialog();

            for (int i = 0; i < ClassesBlackBox.Count; i++)
            {
                buttons.Add(new Button());
                buttons[i].Text = "CLASS " + (i).ToString();
                buttons[i].Size = new System.Drawing.Size(115, 30);
                buttons[i].Location = new System.Drawing.Point(904, 213 + i * 40);

                if (i != ClassesBlackBox.Count - 1)
                {
                    this.Controls.Add(buttons[i]);
                    continue;
                }

                buttons[i].Click += (sender, e) => Button_Click(sender, e, i);

                // Add the button to the form's controls
                this.Controls.Add(buttons[i]);
            }


        }
        private void Button_Click(object sender, EventArgs e, int index)
        {
            classForm[index - 1].ShowDialog();
        }
    }
}
