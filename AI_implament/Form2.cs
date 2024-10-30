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
        Color[,] color;
        Color[,] colorBW;
        public static int SectorsCount;
        public static List<Class2> ClassesBlackBox = new List<Class2>();
        Class2 BlackBox;
        List<Form> classForm = new List<Form>();
        Form drawingForm = new Form4();
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
    }
}
