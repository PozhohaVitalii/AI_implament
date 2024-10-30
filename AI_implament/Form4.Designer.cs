namespace AI_implament
{
    partial class Form4
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
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            button3 = new Button();
            richTextBox1 = new RichTextBox();
            button4 = new Button();
            textBox3 = new TextBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(43, 28);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(87, 27);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(672, 28);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(76, 27);
            textBox2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(43, 70);
            button1.Name = "button1";
            button1.Size = new Size(87, 47);
            button1.TabIndex = 2;
            button1.Text = "Add to C";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(660, 70);
            button2.Name = "button2";
            button2.Size = new Size(88, 47);
            button2.TabIndex = 3;
            button2.Text = "Calculate";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(136, 31);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 4;
            label1.Text = "- grid size";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(527, 31);
            label2.Name = "label2";
            label2.Size = new Size(139, 20);
            label2.TabIndex = 5;
            label2.Text = "standart deviation -";
            // 
            // button3
            // 
            button3.Font = new Font("Showcard Gothic", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button3.Location = new Point(294, 409);
            button3.Name = "button3";
            button3.Size = new Size(202, 29);
            button3.TabIndex = 6;
            button3.Text = "Clear";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(527, 255);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(221, 183);
            richTextBox1.TabIndex = 7;
            richTextBox1.Text = "";
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI Variable Display", 10.8F, FontStyle.Regular, GraphicsUnit.Point);
            button4.Location = new Point(345, 22);
            button4.Name = "button4";
            button4.Size = new Size(102, 33);
            button4.TabIndex = 8;
            button4.Text = "Recognise";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(144, 80);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(67, 27);
            textBox3.TabIndex = 9;
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox3);
            Controls.Add(button4);
            Controls.Add(richTextBox1);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Form4";
            Text = "Form4";
            Load += Form4_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private Button button3;
        private RichTextBox richTextBox1;
        private Button button4;
        private TextBox textBox3;
    }
}