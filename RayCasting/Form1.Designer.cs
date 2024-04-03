namespace TestApp
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
            KeyPreview = true;

            button1 = new Button();
            panel1 = new Panel();
            KeyDown += Form1_KeyDown;
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 637);
            button1.Name = "button1";
            button1.Size = new Size(135, 32);
            button1.TabIndex = 1;
            button1.Text = "DrawLine";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.BackgroundImageLayout = ImageLayout.None;
            panel1.Location = new Point(803, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 263);
            panel1.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1064, 681);
            Controls.Add(panel1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Panel panel1;
    }
}