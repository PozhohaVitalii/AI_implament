using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_implament
{
    public partial class Form2 : Form
    {
        public int NumbOFclasses = 0;
        public Bitmap ImageExempl;
        public Bitmap BlackWhite;
        public MLP mlp;
        Color[,] color;
        Color[,] colorBW;
        public static int SectorsCount;
        public static List<Class2> ClassesBlackBox = new List<Class2>();
        Class2 BlackBox;
        List<Form> classForm = new List<Form>();
        Form drawingForm = new Form4();
        Form NeuroForm0 = new Form5();
        Form NeuroForm1 = new Form6();
        List<Button> buttons = new List<Button>();

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    ImageExempl = new Bitmap(pictureBox1.Image);
                    BlackWhite = new Bitmap(pictureBox1.Image);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int minj = 0;
            int mini = 0;
            int maxj = 0;
            int maxi = 0;

            try
            {
                SectorsCount = int.Parse(textBox1.Text);
            }
            catch { }
            if (SectorsCount != 0)
            {
                double sectorAngle = 90.0 / SectorsCount;
                int CountOfBlackPoints = 0;
                color = GetPixelArray(ImageExempl);
                mini = color.GetLength(0);
                minj = color.GetLength(1);
                for (int i = 0; i < color.GetLength(0); i++)
                {
                    for (int j = 0; j < color.GetLength(1); j++)
                    {
                        int gray = (int)(0.299 * color[i, j].R + 0.587 * color[i, j].G + 0.114 * color[i, j].B);
                        if (gray > 128)
                        {
                            gray = 255;
                        }
                        else
                        {
                            CountOfBlackPoints++;
                            gray = 1;
                            mini = mini < i ? mini : i;
                            minj = minj < j ? minj : j;
                            maxi = maxi > i ? maxi : i;
                            maxj = maxj > j ? maxj : j;
                        }
                        BlackWhite.SetPixel(i, j, Color.FromArgb(255, gray, gray, gray));
                    }
                }


                for (int i = mini; i < maxi + 1; i++)
                {
                    for (int j = minj; j < maxj + 1; j++)
                    {
                        int gray = (int)(0.299 * color[i, j].R + 0.587 * color[i, j].G + 0.114 * color[i, j].B);
                        if (gray > 128)
                        {
                            gray = 255;
                        }
                        else
                        {
                            CountOfBlackPoints++;
                            gray = 1;

                        }
                        double wCord, hCord;
                        wCord = i - mini;
                        hCord = j - minj;
                        double hipotenusa = Math.Sqrt(Math.Pow(maxi - mini - wCord, 2) + Math.Pow(hCord, 2));
                        double angle = Math.Asin((double)hCord / hipotenusa) * (180.0 / Math.PI);
                        int n = 0;
                        do
                        {
                            Console.WriteLine(sectorAngle + " " + n);
                            n++;
                        }
                        while (angle > n * sectorAngle);
                        if (gray == 255)
                        {

                            int m = 7;
                            if (SectorsCount < 8) m = 17;
                            gray = gray - n * m;
                        }

                        BlackWhite.SetPixel(i, j, Color.FromArgb(255, gray, gray, gray));
                    }
                }

                richTextBox1.Text += "\n Count of black points on image: " + CountOfBlackPoints.ToString() + "\n";
                using (Graphics g = Graphics.FromImage(BlackWhite))
                {
                    Pen pen = new Pen(Color.Lime, 1);
                    g.DrawRectangle(pen, mini, minj, maxi - mini, maxj - minj);
                    pen.Dispose();
                }

                BlackBox = new Class2();
                Rectangle part = new Rectangle(mini, minj, maxi - mini, maxj - minj);
                Bitmap clonedBitmap = ClonePartOfBitmap(ImageExempl, part);
                Bitmap clonedBitmap1 = ClonePartOfBitmap(BlackWhite, part);
                pictureBox1.Image = BlackWhite;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                BlackBox.Add(clonedBitmap);
            }
        }
        public static Bitmap ClonePartOfBitmap(Bitmap sourceBitmap, Rectangle section)
        {
            // Clone the specified section of the bitmap.
            return sourceBitmap.Clone(section, sourceBitmap.PixelFormat);
        }


        public string getTextBox1()
        {
            return textBox1.Text;
        }
        public static Color[,] GetPixelArray(Bitmap imageExempl)
        {
            // Створення двовимірного масиву для пікселів
            int width = imageExempl.Width;
            int height = imageExempl.Height;
            Color[,] pixelArray = new Color[width, height];

            // Блокування бітів зображення
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = imageExempl.LockBits(rect, ImageLockMode.ReadOnly, imageExempl.PixelFormat);

            try
            {
                // Визначення кількості байтів на один рядок
                int bytesPerPixel = Image.GetPixelFormatSize(imageExempl.PixelFormat) / 8;
                int byteCount = bitmapData.Stride * height;
                byte[] pixels = new byte[byteCount];

                // Копіювання піксельних даних у масив
                Marshal.Copy(bitmapData.Scan0, pixels, 0, byteCount);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Обчислення індексу в масиві для конкретного пікселя
                        int index = (y * bitmapData.Stride) + (x * bytesPerPixel);

                        // Зчитування компонентів кольору
                        byte blue = pixels[index];
                        byte green = pixels[index + 1];
                        byte red = pixels[index + 2];

                        // Якщо формат зображення підтримує альфа-канал
                        byte alpha = bytesPerPixel == 4 ? pixels[index + 3] : (byte)255;

                        // Додавання кольору до двовимірного масиву
                        pixelArray[x, y] = Color.FromArgb(alpha, red, green, blue);
                    }
                }
            }
            finally
            {
                // Розблокування бітів зображення
                imageExempl.UnlockBits(bitmapData);
            }

            return pixelArray;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Form form = new Form1();
            form.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold | FontStyle.Italic);
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
                buttons[i].Location = new System.Drawing.Point(904, 83 + i * 40);

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
        private void button5_Click(object sender, EventArgs e)
        {
            BlackBox.calcFirst();
            int[,] SignsVector = BlackBox.getSector();
            double[] PozhohaVitaliiM1 = new double[SignsVector.GetLength(1)];
            double[] PozhohaVitaliiS1 = new double[SignsVector.GetLength(1)];
            double[] AbsoluteVector = new double[SignsVector.GetLength(1)];

            for (int i = 0; i < SignsVector.GetLength(0); i++)
            {
                richTextBox1.Text = richTextBox1.Text + "\n";
                for (int j = 0; j < SignsVector.GetLength(1); j++)
                {
                    PozhohaVitaliiM1[j] = SignsVector[i, j];
                    PozhohaVitaliiS1[j] = SignsVector[i, j];
                    AbsoluteVector[j] = SignsVector[i, j];
                    richTextBox1.Text = richTextBox1.Text + " " + SignsVector[i, j];
                }
            }
            double mx = PozhohaVitaliiM1.Max();
            for (int i = 0; i < PozhohaVitaliiM1.Length; i++)
            {

                PozhohaVitaliiM1[i] = PozhohaVitaliiM1[i] / mx;
                PozhohaVitaliiS1[i] = PozhohaVitaliiS1[i] / (double)BlackBox.CountOfBlackPoints[0];
            }
            richTextBox1.Text = richTextBox1.Text + "\n" + "PozhohaVitaliiS1\n";
            for (int i = 0; i < PozhohaVitaliiS1.Length; i++)
            {
                richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiS1[i].ToString("F2") + "  ";
            }
            richTextBox1.Text = richTextBox1.Text + "\n" + "PozhohaVitaliiM1\n";
            for (int i = 0; i < PozhohaVitaliiM1.Length; i++)
            {
                richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiM1[i].ToString("F2") + "  ";
            }

            double[] LowLimS;
            double[] HighLimS;


            bool[] classEntity = new bool[ClassesBlackBox.Count];
            for (int t = 0; t < ClassesBlackBox.Count; t++)
            {


                ClassesBlackBox[t].calcFirst();

                classEntity[t] = true;
                LowLimS = ClassesBlackBox[t].getLowLimit();
                HighLimS = ClassesBlackBox[t].getHighLimit();

                richTextBox1.Text = richTextBox1.Text + "\n";




                for (int i = 0; i < LowLimS.Length; i++)
                {
                    if (LowLimS[i] > PozhohaVitaliiS1[i]) classEntity[t] = false;
                    if (HighLimS[i] < PozhohaVitaliiS1[i]) classEntity[t] = false;
                }

                if (classEntity[t])
                {
                    richTextBox1.Text = richTextBox1.Text + "\n" + " OBJECT RECOGNIZED like:  class" + t.ToString();
                    richTextBox1.Text = richTextBox1.Text + "\n" + "High lim" + "\n"; ;
                    for (int j = 0; j < HighLimS.Length; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + HighLimS[j].ToString("F3") + "  ";
                    }
                    richTextBox1.Text = richTextBox1.Text + "\n" + " Object " + "\n";
                    for (int j = 0; j < PozhohaVitaliiS1.Length; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiS1[j].ToString("F3") + "  ";
                    }
                    richTextBox1.Text = richTextBox1.Text + "\n" + " Low lim " + "\n";
                    for (int j = 0; j < LowLimS.Length; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + LowLimS[j].ToString("F3") + "  ";
                    }
                    richTextBox1.Text = richTextBox1.Text + "\n";

                }
                else
                {
                    richTextBox1.Text = richTextBox1.Text + "Class  " + t + " are not recognized " + "\n" + "High lim" + "\n";
                    for (int j = 0; j < HighLimS.Length; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + HighLimS[j].ToString("F3") + "  ";
                    }

                    richTextBox1.Text = richTextBox1.Text + "\n" + " Object " + "\n";
                    for (int j = 0; j < PozhohaVitaliiS1.Length; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiS1[j].ToString("F3") + "  ";
                    }

                    richTextBox1.Text = richTextBox1.Text + "\n" + " Low lim " + "\n";
                    for (int j = 0; j < LowLimS.Length; j++)
                    {
                        richTextBox1.Text = richTextBox1.Text + LowLimS[j].ToString("F3") + "  ";
                    }
                    richTextBox1.Text = richTextBox1.Text + "\n";

                }

            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            SectorsCount = int.Parse(textBox1.Text);
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                SectorsCount = int.Parse(textBox1.Text);
            }
            catch (Exception) { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (BlackBox.CountOfBlackPoints[0] == null)
            {
                BlackBox.calcFirst();
            }
            int[,] SignsVector = BlackBox.getSector();
            double[] PozhohaVitaliiM1 = new double[SignsVector.GetLength(1)];
            double[] PozhohaVitaliiS1 = new double[SignsVector.GetLength(1)];
            double[] AbsoluteVector = new double[SignsVector.GetLength(1)];

            for (int i = 0; i < SignsVector.GetLength(0); i++)
            {
                richTextBox1.Text = richTextBox1.Text + "\n";
                for (int j = 0; j < SignsVector.GetLength(1); j++)
                {
                    PozhohaVitaliiM1[j] = SignsVector[i, j];
                    PozhohaVitaliiS1[j] = SignsVector[i, j];
                    AbsoluteVector[j] = SignsVector[i, j];
                    richTextBox1.Text = richTextBox1.Text + " " + SignsVector[i, j];
                }
            }
            double mx = PozhohaVitaliiM1.Max();
            for (int i = 0; i < PozhohaVitaliiM1.Length; i++)
            {

                PozhohaVitaliiM1[i] = PozhohaVitaliiM1[i] / mx;
                PozhohaVitaliiS1[i] = PozhohaVitaliiS1[i] / (double)BlackBox.CountOfBlackPoints[0];
            }
            richTextBox1.Text = richTextBox1.Text + "\n" + "PozhohaVitaliiS1\n";
            for (int i = 0; i < PozhohaVitaliiS1.Length; i++)
            {
                richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiS1[i].ToString("F2") + "  ";
            }
            richTextBox1.Text = richTextBox1.Text + "\n" + "PozhohaVitaliiM1\n";
            for (int i = 0; i < PozhohaVitaliiM1.Length; i++)
            {
                richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiM1[i].ToString("F2") + "  ";
            }

            richTextBox1.Text = richTextBox1.Text + "\n\n\n";

            double[] LowLimS;
            double[] HighLimS;
            double[] CentreS;

            double[] Distance = new double[ClassesBlackBox.Count];

            bool[] classEntity = new bool[ClassesBlackBox.Count];
            for (int t = 0; t < ClassesBlackBox.Count; t++)
            {
                if (ClassesBlackBox[t].CountOfBlackPoints[0] == null)
                {
                    ClassesBlackBox[t].calcFirst();
                }
                classEntity[t] = true;
                LowLimS = ClassesBlackBox[t].getLowLimit();
                HighLimS = ClassesBlackBox[t].getHighLimit();
                CentreS = new double[LowLimS.Length];


                richTextBox1.Text = richTextBox1.Text + "\n Class " + t + "  PozhohaVitaliiS1Centre\n";
                double distance = 0;
                for (int i = 0; i < LowLimS.Length; i++)
                {
                    CentreS[i] = (HighLimS[i] + LowLimS[i]) / 2;
                    distance += Math.Abs(PozhohaVitaliiS1[i] - CentreS[i]);
                    richTextBox1.Text = richTextBox1.Text + CentreS[i].ToString("F2") + " ";
                }
                richTextBox1.Text = richTextBox1.Text + "\n";
                Distance[t] = distance;
            }
            int minInd = 0;
            for (int i = 0; i < Distance.Length; i++)
            {
                if (Distance[i] < Distance[minInd]) minInd = i;
                richTextBox1.Text = richTextBox1.Text + "\n D" + i + " =" + Distance[i];
            }
            richTextBox1.Text = richTextBox1.Text + "\n min D  is " + Distance[minInd] + "\n Object append to class - " + minInd;






        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            drawingForm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            NeuroForm0.ShowDialog();
        }
        public static double[][] ConvertToJaggedArray(double[,] array2D)
        {
            int rows = array2D.GetLength(0);
            int cols = array2D.GetLength(1);
            double[][] jaggedArray = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = array2D[i, j];
                }
            }

            return jaggedArray;
        }
        public static double[][] CombineByRows(double[][] array1, double[][] array2)
        {
            int rows1 = array1.Length;
            int rows2 = array2.Length;
            int totalRows = rows1 + rows2;
            int cols = array1[0].Length;

            double[][] result = new double[totalRows][];

            // Copy rows from the first array
            for (int i = 0; i < rows1; i++)
            {
                result[i] = new double[cols];
                Array.Copy(array1[i], result[i], cols);
            }

            // Copy rows from the second array
            for (int i = 0; i < rows2; i++)
            {
                result[rows1 + i] = new double[cols];
                Array.Copy(array2[i], result[rows1 + i], cols);
            }

            return result;
        }

        public void DisplayArrayInRichTextBox(double[][] array, RichTextBox richTextBox)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var row in array)
            {
                sb.AppendLine(string.Join("\t", row)); // Join each element in the row with tabs for formatting
            }

            richTextBox.Text += sb.ToString();
        }
        public static double[][] ConvertListToJaggedArray(List<int> list)
        {
            // Find the maximum value in the list to set the size of each inner array
            int maxValue = list.Max();
            double[][] result = new double[list.Count][];

            // Create each row with '1' at the specified index, all other elements set to '0'
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = new double[maxValue + 1]; // Inner array size is equal to maxValue
                int index = list[i];          // Place '1' at the index corresponding to the list value
                result[i][index] = 1;
            }

            return result;
        }


        private void button8_Click(object sender, EventArgs e)
        {
            if (BlackBox.CountOfBlackPoints[0] == null)
            {
                BlackBox.calcFirst();
            }
            for (int t = 0; t < ClassesBlackBox.Count; t++)
            {
                if (ClassesBlackBox[t].CountOfBlackPoints[0] == null)
                {
                    ClassesBlackBox[t].calcFirst();
                }
            }



            double[,] SignsVector = BlackBox.getSectorS();
            double[][] oblSectors = ConvertToJaggedArray(SignsVector);
            //DisplayArrayInRichTextBox(oblSectors, richTextBox1);

            List<int> list = new List<int>();
            double[][] classes = ConvertToJaggedArray(ClassesBlackBox[0].getSectorS());
            for (int j = list.Count; j < classes.Length; j++)
            {
                list.Add(0);
            }
            for (int i = 1; i < ClassesBlackBox.Count; i++)
            {
                classes = CombineByRows(classes, ConvertToJaggedArray(ClassesBlackBox[i].getSectorS()));
                for (int j = list.Count; j < classes.Length; j++)
                {
                    list.Add(i);
                }
            }
            // DisplayArrayInRichTextBox(classes, richTextBox1);




            // NeuroForm1.ShowDialog();
            int inputSize = SectorsCount;
            int hiddenSize = SectorsCount;
            int outputSize = ClassesBlackBox.Count;
            double learningRate = 0.1;

            mlp = new MLP(inputSize, hiddenSize, outputSize, learningRate);


            // Вхідні дані (наприклад, логічна функція AND)
            double[][] inputs = classes;

            // Очікувані результати для функції AND
            double[][] targets = ConvertListToJaggedArray(list);

            //DisplayArrayInRichTextBox(targets, richTextBox1);
            // Навчання моделі
            int epochs = 5000;
            mlp.Train(inputs, targets, epochs);

            // Тестування на нових даних
            richTextBox1.Text += "\n\n\nTesting MLP on classification :\n";
        
            richTextBox1.Text += "Neurons output\n";
            for (int i = 0; i < oblSectors.Length; i++)
            {
                double[] output = mlp.Predict(oblSectors[i]);
                foreach (double a in output)
                {
                    richTextBox1.Text += a.ToString("F1") + " ";
                }
                richTextBox1.Text += "\n";
            }


        }

        private void button10_Click(object sender, EventArgs e)
        {
            BlackBox.calcFirst();
            double[,] SignsVector = BlackBox.getSectorS();
            double[][] oblSectors = ConvertToJaggedArray(SignsVector);
            DisplayArrayInRichTextBox(oblSectors, richTextBox1);

            richTextBox1.Text += "\n\n";
            richTextBox1.Text += "Neurons output \n";
            for (int i = 0; i < oblSectors.Length; i++)
            {
                double[] output = mlp.Predict(oblSectors[i]);
                foreach (double a in output)
                {
                    richTextBox1.Text += a.ToString("F1") + " ";
                }
                richTextBox1.Text += "\n";
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Define a network with 2 inputs, one hidden layer of 3 neurons, and 1 output neuron
            // Define the XOR dataset
            double[][] inputs = new double[][]
            {
            new double[] { 0, 0 },
            new double[] { 0, 1 },
            new double[] { 1, 0 },
            new double[] { 1, 1 }
            };

            double[][] expectedOutputs = new double[][]
            {
            new double[] { 0 },
            new double[] { 0 },
            new double[] { 0 },
            new double[] { 1 }
            };

            // Initialize the MLP with 2 input neurons, 2 hidden neurons, and 1 output neuron
            int inputSize = 2;
            int hiddenSize = 2;
            int outputSize = 1;
            double learningRate = 0.1;

            MLP mlp = new MLP(inputSize, hiddenSize, outputSize, learningRate);

            // Train the MLP
            int epochs = 100000;
            mlp.Train(inputs, expectedOutputs, epochs);

            // Test the trained MLP on the XOR dataset
            Console.WriteLine("Testing on XOR dataset:");
            foreach (var input in inputs)
            {
                double[] output = mlp.Predict(input);
                richTextBox1.Text += $"Input: {input[0]}, {input[1]} => Predicted Output: {output[0]:F4}";
            }
        }
    }
}
