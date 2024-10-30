using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AI_implament
{

    public partial class Form3 : Form
    {
        public Bitmap ImageExempl;
        public Bitmap BlackWhite;
        Color[,] color;
        Color[,] colorBW;
        List<PictureBox> pictureBoxes = new List<PictureBox>();
        int i;
        public int NumbOfClass { get; set; }
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(int message)
        {
            InitializeComponent();
            NumbOfClass = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {



                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                    openFileDialog.Multiselect = true; // Дозволяє вибирати кілька файлів одночасно

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string fileName in openFileDialog.FileNames)
                        {
                            i++;
                            pictureBoxes.Add(new PictureBox());
                            pictureBoxes[i - 1].Size = new Size(110, 110);
                            pictureBoxes[i - 1].Image = Image.FromFile(fileName);
                            pictureBoxes[i - 1].Location = new Point((i - 1) * 110 + 10, 70);
                            pictureBoxes[i - 1].SizeMode = PictureBoxSizeMode.Zoom;

                            if ((i - 1) * 110 + 110 > this.Width) this.Width = (i - 1) * 110 + 130;
                            button1.Location = new Point(Width / 2 - 60, Height - 110);
                            ImageExempl = new Bitmap(pictureBoxes[i - 1].Image);
                            BlackWhite = new Bitmap(pictureBoxes[i - 1].Image);



                            int minj = 0;
                            int mini = 0;
                            int maxj = 0;
                            int maxi = 0;

                            if (Form2.SectorsCount != 0)
                            {
                                double sectorAngle = 90.0 / Form2.SectorsCount;
                                int CountOfBlackPoints = 0;
                                color = Form2.GetPixelArray(ImageExempl);
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
                                        else if (gray <= 128)
                                        {
                                            gray = 1;
                                            mini = mini < i ? mini : i;
                                            minj = minj < j ? minj : j;
                                            maxi = maxi > i ? maxi : i;
                                            maxj = maxj > j ? maxj : j;

                                        }
                                    }
                                }

                                for (int i = mini; i < maxi; i++)
                                {
                                    for (int j = minj; j < maxj; j++)
                                    {
                                        int gray = (int)(0.299 * color[i, j].R + 0.587 * color[i, j].G + 0.114 * color[i, j].B);
                                        if (gray > 128)
                                        {
                                            gray = 255;
                                        }
                                        else if (gray <= 128)
                                        {
                                            gray = 1;
                                            CountOfBlackPoints++;


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
                                        if (Form2.SectorsCount < 8) m = 17;
                                        gray = gray - n * m;
                                    }

                                    BlackWhite.SetPixel(i, j, Color.FromArgb(255, gray, gray, gray));
                                }
                            }

                            using (Graphics g = Graphics.FromImage(BlackWhite))
                                {
                                    Pen pen = new Pen(Color.LightGreen, 1);
                                    g.DrawRectangle(pen, mini, minj, (maxi - mini), (maxj - minj));
                                    pen.Dispose();
                                }

                                pictureBoxes[i - 1].Image = BlackWhite;
                                pictureBoxes[i - 1].SizeMode = PictureBoxSizeMode.Zoom;

                                Rectangle part = new Rectangle(mini, minj, (maxi - mini), (maxj - minj));
                                Bitmap clonedBitmap = Form2.ClonePartOfBitmap(ImageExempl, part);
                                Form2.ClassesBlackBox[NumbOfClass - 1].Add(clonedBitmap);


                            }
                            this.Controls.Add(pictureBoxes[i - 1]);

                        }
                    }
                }
            }

            private void Form3_Load(object sender, EventArgs e)
        {
            if (Form2.SectorsCount == 0) { throw new Exception("Its no number for sectors"); }
        }
    }
}
