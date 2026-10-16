using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChatBot
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            pictureBox1 = new PictureBox();
            ChatInput = new RichTextBox();
            ChatInput2 = new TextBox();
            Send = new Button();
            textBox1 = new TextBox();
            radioButton1 = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(2, 1);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(78, 20);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.HighlightText;
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(297, 280);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(78, 20);
            button2.TabIndex = 1;
            button2.Text = "Start!";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Location = new Point(2, 1);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(124, 82);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // ChatInput
            // 
            ChatInput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ChatInput.BackColor = Color.WhiteSmoke;
            ChatInput.Location = new Point(130, 60);
            ChatInput.Margin = new Padding(2);
            ChatInput.Name = "ChatInput";
            ChatInput.ReadOnly = true;
            ChatInput.ScrollBars = RichTextBoxScrollBars.Vertical;
            ChatInput.Size = new Size(486, 189);
            ChatInput.TabIndex = 3;
            ChatInput.Text = "";
            ChatInput.KeyDown += ChatInput_KeyDown;
            // 
            // ChatInput2
            // 
            ChatInput2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ChatInput2.BackColor = SystemColors.ControlLight;
            ChatInput2.Location = new Point(150, 253);
            ChatInput2.Margin = new Padding(2);
            ChatInput2.Name = "ChatInput2";
            ChatInput2.Size = new Size(384, 23);
            ChatInput2.TabIndex = 4;
            ChatInput2.TextChanged += ChatInput2_TextChanged;
            // 
            // Send
            // 
            Send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Send.BackColor = SystemColors.HotTrack;
            Send.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            Send.ForeColor = Color.Fuchsia;
            Send.Location = new Point(552, 253);
            Send.Margin = new Padding(2);
            Send.Name = "Send";
            Send.Size = new Size(51, 29);
            Send.TabIndex = 5;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = false;
            Send.Click += Send_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Navy;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Cursor = Cursors.Cross;
            textBox1.Font = new Font("Segoe UI Black", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.Fuchsia;
            textBox1.Location = new Point(224, 1);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(232, 54);
            textBox1.TabIndex = 6;
            textBox1.Text = "CHATBOT";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            radioButton1.ForeColor = Color.Lime;
            radioButton1.Location = new Point(561, 12);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(65, 19);
            radioButton1.TabIndex = 7;
            radioButton1.TabStop = true;
            radioButton1.Text = "Online";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Navy;
            ClientSize = new Size(656, 311);
            Controls.Add(radioButton1);
            Controls.Add(textBox1);
            Controls.Add(Send);
            Controls.Add(ChatInput2);
            Controls.Add(ChatInput);
            Controls.Add(pictureBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Chatbot";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private PictureBox pictureBox1;
        private RichTextBox ChatInput;
        private TextBox ChatInput2;
        private Button Send;
        private TextBox textBox1;
        private RadioButton radioButton1;
    }
}
