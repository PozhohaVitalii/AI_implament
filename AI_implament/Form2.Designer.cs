namespace AI_implament
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            button1 = new Button();
            button2 = new Button();
            richTextBox1 = new RichTextBox();
            button3 = new Button();
            button4 = new Button();
            textBox1 = new TextBox();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(22, 27);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(543, 542);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            button1.Location = new Point(619, 27);
            button1.Name = "button1";
            button1.Size = new Size(279, 59);
            button1.TabIndex = 1;
            button1.Text = "ADD IMAGE";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(784, 110);
            button2.Name = "button2";
            button2.Size = new Size(114, 80);
            button2.TabIndex = 2;
            button2.Text = "+CLASS";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(619, 213);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(279, 255);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            button3.Location = new Point(619, 110);
            button3.Name = "button3";
            button3.Size = new Size(122, 80);
            button3.TabIndex = 4;
            button3.Text = "Transform";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            button4.Location = new Point(619, 487);
            button4.Name = "button4";
            button4.Size = new Size(279, 60);
            button4.TabIndex = 5;
            button4.Text = "VISUALIZATION";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            textBox1.Location = new Point(904, 27);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(115, 59);
            textBox1.TabIndex = 6;
            textBox1.Text = "Sectors Count";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.Click += textBox1_Click;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
            textBox1.MouseLeave += textBox1_MouseLeave;
            textBox1.MouseHover += textBox1_MouseHover;
            // 
            // button5
            // 
            button5.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button5.Location = new Point(904, 487);
            button5.Name = "button5";
            button5.Size = new Size(115, 60);
            button5.TabIndex = 7;
            button5.Text = "Recognise";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button6.Location = new Point(904, 431);
            button6.Name = "button6";
            button6.Size = new Size(115, 50);
            button6.TabIndex = 8;
            button6.Text = "DISttance";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Font = new Font("Stencil", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button7.Location = new Point(904, 375);
            button7.Name = "button7";
            button7.Size = new Size(115, 50);
            button7.TabIndex = 9;
            button7.Text = "Drawing";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1079, 600);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(textBox1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(richTextBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "Form2";
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private Button button1;
        private Button button2;
        private RichTextBox richTextBox1;
        private Button button3;
        private Button button4;
        public TextBox textBox1;
        private Button button5;
        private Button button6;
        private Button button7;
    }
}