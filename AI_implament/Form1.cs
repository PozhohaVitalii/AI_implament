using System;
using System.Drawing;
using System.Windows.Forms;
namespace AI_implament
{
    public partial class Form1 : Form
    {
        private OpenGLControl openGLControl;


        public int buttonWidth = 100;
        public int buttonHeight = 30;
        public int spacing = 10;
        public int startX = 10;
        public int startY = 10;

        // Оголошення всіх кнопок
        private Button btnRotateXPos;
        private Button btnRotateXNeg;
        private Button btnRotateYPos;
        private Button btnRotateYNeg;
        private Button btnRotateZPos;
        private Button btnRotateZNeg;

        // Таймер для оновлення панелей
        private System.Windows.Forms.Timer updateTimer;

        public Form1()
        {
            InitializeComponent();

            // Ініціалізуємо OpenGLControl
            openGLControl = new OpenGLControl
            {
                Dock = DockStyle.Fill
            };

            // Ініціалізуємо панель для кнопок


            // Додаємо панель та OpenGLControl на форму
            this.Controls.Add(openGLControl);


            // Ініціалізуємо та додаємо кнопки
            InitializeControls();

            // Ініціалізуємо таймер
            InitializeTimer();
            updatePanels(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Додаткові налаштування при завантаженні форми
        }

        private void InitializeTimer()
        {
            // Ініціалізація таймера
            updateTimer = new System.Windows.Forms.Timer
            {
                Interval = 500 // Інтервал у мілісекундах (1 секунда)
            };
            updateTimer.Tick += (s, e) => updatePanels(); // Прив'язуємо подію Tick до функції updatePanels
            updateTimer.Start(); // Запускаємо таймер
        }

        private void updatePanels()
        {
            // Оновлення розмірів та розташування панелі
            panel1.Width = buttonWidth + 2 * spacing;
            panel1.Height = 2 * buttonHeight + 3 * spacing;
            panel1.Location = new Point(10, this.ClientSize.Height / 2 - (2 * buttonHeight + 3 * spacing) / 2);

            panel2.Width = 2 * buttonWidth + 3 * spacing;
            panel2.Height = buttonHeight + 2 * spacing;
            panel2.Location = new Point(this.ClientSize.Width / 2 - (2 * buttonWidth + 3 * spacing) / 2, 10);

            panel3.Width = 2* buttonWidth + 2 * spacing;
            panel3.Height = 2 * buttonHeight + 3 * spacing;
            panel3.Location = new Point(this.ClientSize.Width - panel3.Width - 10, this.ClientSize.Height - panel3.Height - 10);




        }

        private void InitializeControls()
        {
            // Кнопки для обертання навколо осі X
            btnRotateXPos = new Button
            {
                Text = "Rotate X+",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX, startY)
            };
            btnRotateXPos.Click += (s, e) => openGLControl.RotateX(5.0f);

            btnRotateXNeg = new Button
            {
                Text = "Rotate X-",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX, startY + buttonHeight + spacing)
            };
            btnRotateXNeg.Click += (s, e) => openGLControl.RotateX(-5.0f);

            // Кнопки для обертання навколо осі Y
            btnRotateYPos = new Button
            {
                Text = "Rotate Y+",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX, startY)
            };
            btnRotateYPos.Click += (s, e) => openGLControl.RotateY(5.0f);

            btnRotateYNeg = new Button
            {
                Text = "Rotate Y-",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + spacing + buttonWidth, startY)
            };
            btnRotateYNeg.Click += (s, e) => openGLControl.RotateY(-5.0f);

            // Кнопки для обертання навколо осі Z
            btnRotateZPos = new Button
            {
                Text = "Rotate Z+",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX + buttonWidth , startY)
            };
            btnRotateZPos.Click += (s, e) => openGLControl.RotateZ(5.0f);

            btnRotateZNeg = new Button
            {
                Text = "Rotate Z-",
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(startX , startY + spacing + buttonHeight)
            };
            btnRotateZNeg.Click += (s, e) => openGLControl.RotateZ(-5.0f);

            // Додаємо кнопки на панель
            panel1.Controls.Add(btnRotateXPos);
            panel1.Controls.Add(btnRotateXNeg);
            panel2.Controls.Add(btnRotateYPos);
            panel2.Controls.Add(btnRotateYNeg);
            panel3.Controls.Add(btnRotateZPos);
            panel3.Controls.Add(btnRotateZNeg);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Обробка події малювання панелі
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
