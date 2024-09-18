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
        public Bitmap ImageExempl;
        public Bitmap BlackWhite;
        Color[,] color;
        Color[,] colorBW;
        public static int SectorsCount;
        Class2 BlackBox;
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
            try
            {
                SectorsCount = int.Parse(textBox1.Text);
            }
            catch { }
            if (SectorsCount != 0)
            {   double sectorAngle  = 90 / SectorsCount;
                BlackBox = new Class2();
                BlackBox.Add(ImageExempl);
                int CountOfBlackPoints = 0;
                color = GetPixelArray(ImageExempl);
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
                            gray = 1;
                            CountOfBlackPoints++;
                        }
                        double hipotenusa = Math.Sqrt(Math.Pow(color.GetLength(0)-i, 2) + Math.Pow(j, 2));
                        double angle = Math.Asin((double)j / hipotenusa) * (180.0 / Math.PI);
                        int n = 0;
                        do
                        {
                            Console.WriteLine(sectorAngle+" "+n);
                            n++;
                        }
                        while (angle > n * sectorAngle);
                        if (gray == 255)
                        {
                            gray = gray - n * 7;
                        }
                        else
                        {
                            gray = gray + n * 7;
                        }
                        BlackWhite.SetPixel(i, j, Color.FromArgb(255, gray, gray, gray));
                    }
                }
                richTextBox1.Text += "Count of black points on image: " + CountOfBlackPoints.ToString() + "\n";
                pictureBox1.Image = BlackWhite;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
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
            textBox1.Text = string.Empty;

            textBox1.Font = new Font(textBox1.Font.FontFamily, 17);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BlackBox.calcFirst();
            int[,] SignsVector =  BlackBox.getSector();
            double[] PozhohaVitaliiM1 = new double[SignsVector.GetLength(1)];
            double[] PozhohaVitaliiS1 = new double[SignsVector.GetLength(1)];

            for (int i = 0; i< SignsVector.GetLength(0);i++) {
                richTextBox1.Text = richTextBox1.Text + "\n";
                for (int j = 0; j< SignsVector.GetLength(1); j++) {
                    PozhohaVitaliiM1[j] = SignsVector[i, j];
                    PozhohaVitaliiS1[j]= SignsVector[i, j];
                    richTextBox1.Text = richTextBox1.Text  +" "+ SignsVector[i,j];
                }
            }
            double mx = PozhohaVitaliiM1.Max();
            for (int i = 0; i < PozhohaVitaliiM1.Length; i++)
            {
               
                PozhohaVitaliiM1[i] = PozhohaVitaliiM1[i] / mx;
                PozhohaVitaliiS1[i] = PozhohaVitaliiS1[i] / BlackBox.CountOfBlackPoints;
            }
            richTextBox1.Text = richTextBox1.Text + "\n";
            for (int i = 0; i < PozhohaVitaliiS1.Length; i++)
            {
                richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiS1[i].ToString("F2") +"  ";
            }
            richTextBox1.Text = richTextBox1.Text + "\n";
            for (int i = 0; i < PozhohaVitaliiM1.Length; i++)
            {
                richTextBox1.Text = richTextBox1.Text + PozhohaVitaliiM1[i].ToString("F2") + "  ";
            }



        }
    }
}
