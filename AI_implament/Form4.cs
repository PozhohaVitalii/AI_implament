using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FuzzySharp;

namespace AI_implament
{
    public partial class Form4 : Form
    {
        int gridSize;
        int gridDeviation;
        Point ObjMin;
        Point ObjMax;
        List<int> objectStats;
        private bool isDrawing = false;
        private static Point Start;
        private List<Point> FirstPairPoint = new List<Point>();
        private List<Point> SecondPairPoint = new List<Point>();
        private List<Point> ObjPoints = new List<Point>();
        private Point lastPoint;
        private Bitmap drawingBitmap;
        private Graphics drawingGraphics;

        public Form4()
        {
            InitializeComponent();
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.Paint += Form1_Paint;

            // Initialize Bitmap and Graphics for drawing
            drawingBitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            drawingGraphics = Graphics.FromImage(drawingBitmap);
            drawingGraphics.Clear(Color.White); // Set background color if needed
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                Start = e.Location;
                lastPoint = e.Location;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                // Draw on the Bitmap

                drawingGraphics.DrawLine(Pens.Black, lastPoint, e.Location);
                lastPoint = e.Location;
                Invalidate(); // Trigger Paint event to refresh the form
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = false;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Display the Bitmap on the form
            e.Graphics.DrawImage(drawingBitmap, Point.Empty);
        }

        // Optional: Save the bitmap to a file
        private void SaveDrawing(string filePath)
        {
            drawingBitmap.Save(filePath);
        }
        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int N = 0;
            N = int.Parse(textBox3.Text);
            if (N >= 0)
            {

                string a = "C:\\Users\\verto\\source\\repos\\AI_implament\\AI_implament\\" + "file" + N + ".txt";

                try
                {
                    WriteVectorsToFile(objectStats, a);
                    richTextBox1.Text += "\n" + a;
                }
                catch (Exception ex)
                {
                    richTextBox1.Text += "\n" + "Помилка при записі файлу: {ex.Message}";
                }

            }
        }

        public static void DrawGrid(Bitmap bitmap, Point areaPoint1, Point areaPoint2, Point centerPoint, int cellSize)
        {
            // Визначаємо межі області
            int xMin = Math.Min(areaPoint1.X, areaPoint2.X);
            int xMax = Math.Max(areaPoint1.X, areaPoint2.X);
            int yMin = Math.Min(areaPoint1.Y, areaPoint2.Y);
            int yMax = Math.Max(areaPoint1.Y, areaPoint2.Y);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Встановлюємо колір і товщину ліній сітки
                Pen gridPen = new Pen(Color.Red, 1);

                // Визначаємо зсув для центрованої сітки
                int xOffset = centerPoint.X % cellSize;
                int yOffset = centerPoint.Y % cellSize;

                // Малюємо вертикальні лінії
                for (int x = centerPoint.X; x <= xMax; x += cellSize)
                {
                    if (x >= xMin) g.DrawLine(gridPen, x, yMin, x, yMax);
                }
                for (int x = centerPoint.X; x >= xMin; x -= cellSize)
                {
                    if (x <= xMax) g.DrawLine(gridPen, x, yMin, x, yMax);
                }

                // Малюємо горизонтальні лінії
                for (int y = centerPoint.Y; y <= yMax; y += cellSize)
                {
                    if (y >= yMin) g.DrawLine(gridPen, xMin, y, xMax, y);
                }
                for (int y = centerPoint.Y; y >= yMin; y -= cellSize)
                {
                    if (y <= yMax) g.DrawLine(gridPen, xMin, y, xMax, y);
                }
            }
        }

        public static List<Point> SortPoints(List<Point> points)
        {
            List<Point> sortedPoints = new List<Point>();
            HashSet<Point> remainingPoints = new HashSet<Point>(points);

            // Початкова точка
            Point currentPoint = Start;
            sortedPoints.Add(currentPoint);
            remainingPoints.Remove(currentPoint);

            // Шукаємо наступні точки на відстані 1
            while (remainingPoints.Count > 0)
            {
                Point nextPoint = remainingPoints.FirstOrDefault(p => IsDistanceOne(currentPoint, p));
                if (nextPoint != Point.Empty)
                {
                    sortedPoints.Add(nextPoint);
                    remainingPoints.Remove(nextPoint);
                    currentPoint = nextPoint;
                }
                else
                {
                    break; // Якщо не знайдено точки на відстані 1
                }
            }

            return sortedPoints;
        }

        private static bool IsDistanceOne(Point p1, Point p2)
        {
            bool a = true;
            int b = Math.Abs(p1.X - p2.X);
            int c = Math.Abs(p1.Y - p2.Y);
            if (b > 1 || c > 1) a = false;
            if (b == 0 && c == 0) a = false;
            return a;
        }
        private static bool IsDistanceEnough(Point p1, Point p2)
        {
            bool a = false;
            int b = Math.Abs(p1.X - p2.X);
            int c = Math.Abs(p1.Y - p2.Y);
            if (b > 3 || c > 3) a = true;
            return a;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            gridSize = int.Parse(textBox1.Text);
            gridDeviation = int.Parse(textBox2.Text);
            if (gridSize > 0)
            {
                ObjMin = Start;
                ObjMax = lastPoint;
                for (int x = 0; x < drawingBitmap.Width; x++)
                {
                    for (int y = 0; y < drawingBitmap.Height; y++)
                    {
                        Color color = drawingBitmap.GetPixel(x, y);
                        if (color.R < 10 && color.B < 10 && color.G < 10)
                        {
                            ObjMax.X = x > ObjMax.X ? x : ObjMax.X;
                            ObjMax.Y = y > ObjMax.Y ? y : ObjMax.Y;
                            ObjMin.X = x < ObjMin.X ? x : ObjMin.X;
                            ObjMin.Y = y < ObjMin.Y ? y : ObjMin.Y;
                            ObjPoints.Add(new Point(x, y));

                        }
                    }
                }




                drawingBitmap.SetPixel(ObjMin.X, ObjMin.Y, Color.Green);
                drawingBitmap.SetPixel(ObjMax.X, ObjMax.Y, Color.Green);
                // richTextBox1.Text = richTextBox1.Text + "\n" + ObjMin.ToString() + "\n" + ObjMax.ToString();
                DrawGrid(drawingBitmap, ObjMin, ObjMax, Start, gridSize);
                // Create a Graphics object for the form
                using (Graphics g = this.CreateGraphics())
                {
                    // Create a new PaintEventArgs
                    PaintEventArgs pe = new PaintEventArgs(g, this.ClientRectangle);
                    // Call the Form1_Paint method directly
                    Form1_Paint(this, pe);
                }
                richTextBox1.Text += "\n";
                richTextBox1.Text += "\n points raw Count " + ObjPoints.Count;
                List<Point> points = SortPoints(ObjPoints);
                richTextBox1.Text += "\n points Count " + points.Count;

                List<Point> Redpoints = new List<Point>();
                Redpoints.Add(points[0]);
                for (int i = 1; i < points.Count; i++)
                {
                    Color color = drawingBitmap.GetPixel(points[i].X, points[i].Y);
                    if ((color.R > 10 && color.B < 10 && color.G < 10) && IsDistanceEnough(Redpoints[Redpoints.Count - 1], points[i]))
                    {
                        richTextBox1.Text += points[i].ToString() + "\n";
                        Redpoints.Add(points[i]);
                    }
                }

                for (int i = 0; i < Redpoints.Count; i++)
                {
                    if (i == 0)
                    {
                        FirstPairPoint.Add(Redpoints[i]);
                        continue;
                    }
                    else if (i == Redpoints.Count - 1)
                    {
                        SecondPairPoint.Add(Redpoints[i]);
                        continue;
                    }
                    else
                    {
                        FirstPairPoint.Add(Redpoints[i]);
                        SecondPairPoint.Add(Redpoints[i]);
                    }
                }

                foreach (Point point in FirstPairPoint)
                {
                    richTextBox1.Text += "\nF " + point.ToString();
                }
                foreach (Point point in SecondPairPoint)
                {
                    richTextBox1.Text += "\nS " + point.ToString();
                }

                int size = FirstPairPoint.Count >= SecondPairPoint.Count ? SecondPairPoint.Count : FirstPairPoint.Count;
                using (Graphics g = this.CreateGraphics())
                {
                    Pen Pen1 = new Pen(Color.Green, 1);

                    for (int i = 0; i < size; i++)
                    {
                        g.DrawLine(Pen1, FirstPairPoint[i].X, FirstPairPoint[i].Y, SecondPairPoint[i].X, SecondPairPoint[i].Y);
                    }
                }
                objectStats = new List<int>();
                for (int i = 0; i < size; i++)
                {
                    Point vector = new Point(SecondPairPoint[i].X - FirstPairPoint[i].X, SecondPairPoint[i].Y - FirstPairPoint[i].Y);
                    double angle = CalculateAngle(vector.X, vector.Y);
                    if (angle < 0)
                    {
                        angle += 360;
                    }

                    switch (angle)
                    {
                        case >= 337.5 or < 22.5:
                            objectStats.Add(1);
                            break;
                        case >= 22.5 and < 67.5:
                            objectStats.Add(2);
                            break;
                        case >= 67.5 and < 112.5:
                            objectStats.Add(3);
                            break;
                        case >= 112.5 and < 157.5:
                            objectStats.Add(4);
                            break;
                        case >= 157.5 and < 202.5:
                            objectStats.Add(5);
                            break;
                        case >= 202.5 and < 247.5:
                            objectStats.Add(6);
                            break;
                        case >= 247.5 and < 292.5:
                            objectStats.Add(7);
                            break;
                        case >= 292.5 and < 337.5:
                            objectStats.Add(8);
                            break;
                        default:
                            richTextBox1.Text += "\nНевідомий сектор";
                            break;
                    }
                }
                richTextBox1.Text += " \n";





            }
            else
            {
                richTextBox1.Text = richTextBox1.Text + "\nadd grid size\n";
            }
        }
        public static double CalculateAngle(double x, double y)
        {
            // Обчислюємо кут в радіанах
            double angleInRadians = Math.Atan2(y, x);

            // Перетворюємо в градуси
            double angleInDegrees = angleInRadians * (180.0 / Math.PI);

            return angleInDegrees;
        }
        public static void WriteVectorsToFile(List<int> vectors, string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            string line = string.Join(",", vectors);

            // Записуємо цей рядок у файл
            File.AppendAllText(filePath, line + Environment.NewLine);
        }
        public static List<List<int>> ReadVectorsFromFile(string filePath)
        {
            try
            {
                filePath = Path.GetFullPath(filePath);


                // Зчитуємо всі рядки з файлу
                var lines = File.ReadAllLines(filePath);

                // Створюємо список списків для збереження чисел з кожного рядка
                List<List<int>> vectors = new List<List<int>>();

                foreach (var line in lines)
                {
                    // Розбиваємо рядок за комами і конвертуємо кожен елемент в int
                    List<int> row = line.Split(',')
                                        .Select(int.Parse)
                                        .ToList();

                    // Додаємо зчитаний рядок як список чисел до загального списку
                    vectors.Add(row);
                }

                Console.WriteLine("Дані успішно зчитано.");
                return vectors;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при читанні з файлу: {ex.Message}");
                return new List<List<int>>(); // Повертаємо порожній список у разі помилки
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (int pair in objectStats)
            {
                richTextBox1.Text += " " + pair.ToString();
            }
            List<List<List<int>>> Data = new List<List<List<int>>>();

            for (int i = 0; i < 10; i++)
            {
                string a = "C:\\Users\\verto\\source\\repos\\AI_implament\\AI_implament\\" + "file" + i + ".txt";
                var lines = File.ReadAllLines(a);
                List<List<int>> vector = new List<List<int>>();
                foreach (var line in lines)
                {
                    // Розбиваємо рядок за комами і конвертуємо кожен елемент в int
                    List<int> row = line.Split(',')
                                        .Select(int.Parse)
                                        .ToList();

                    // Додаємо зчитаний рядок як список чисел до загального списку
                    vector.Add(row);
                }
                Data.Add(vector);
            }

/*
            for (int i = 0; i < Data.Count; i++)
            {

                for (int j = 0; j < Data[i].Count; j++)
                {
                    for (int r = 0; r < Data[i][j].Count; r++)
                    {
                        richTextBox1.Text += " " + Data[i][j][r].ToString();
                    }
                    richTextBox1.Text += "\n";
                }
                richTextBox1.Text += "\n\n";
            }
*/
            string Objectstr = "";
            for (int i = 0; i < objectStats.Count; i++)
            {
                Objectstr += objectStats[i].ToString() + " ";
            }

            double[] Class = new double[Data.Count];

            for (int i = 0; i < Data.Count; i++)
            {
                double ClV = 0;

                for (int j = 0; j < Data[i].Count; j++)
                {
                    string row = "";
                    for (int r = 0; r < Data[i][j].Count; r++)
                    {
                        row += Data[i][j][r].ToString() + " ";
                    }

                    ClV += Fuzz.Ratio(Objectstr, row);

                    if (j == Data[i].Count - 1)
                    {
                      
                        ClV = ClV / (j+1);
                    }
                }
                Class[i] = ClV;
            }
            richTextBox1.Text += "\n";
            int p = 0;
            foreach (double a in Class)
            {
                richTextBox1.Text += "Append to class" + p + "  " + a.ToString("F2") + "\n";
                p++;
            }





        }
        public static void RemoveDuplicates(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] == list[i + 1])
                {
                    list.RemoveAt(i + 1);
                    i--; // Залишаємо індекс на місці, щоб перевірити новий сусід
                }
            }
        }
    }
}
