namespace MillLineScan
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
            PlayButton = new Button();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            label3 = new Label();
            FrameCountTextBox = new TextBox();
            label2 = new Label();
            HeightTextbox = new TextBox();
            PageLabel = new Label();
            PrevButton = new Button();
            NextButton = new Button();
            SaveButton = new Button();
            label1 = new Label();
            WdthTextbox = new TextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // PlayButton
            // 
            PlayButton.Location = new Point(264, 5);
            PlayButton.Name = "PlayButton";
            PlayButton.Size = new Size(84, 30);
            PlayButton.TabIndex = 0;
            PlayButton.Text = "Play";
            PlayButton.UseVisualStyleBackColor = true;
            PlayButton.Click += PlayButton_Click;
            // 
            // panel1
            // 
            panel1.Location = new Point(12, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(448, 396);
            panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(WdthTextbox);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(FrameCountTextBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(HeightTextbox);
            groupBox1.Location = new Point(618, 41);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(241, 396);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Parameter";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 97);
            label3.Name = "label3";
            label3.Size = new Size(84, 15);
            label3.TabIndex = 3;
            label3.Text = "Frame Count :";
            // 
            // FrameCountTextBox
            // 
            FrameCountTextBox.Location = new Point(112, 97);
            FrameCountTextBox.Name = "FrameCountTextBox";
            FrameCountTextBox.Size = new Size(89, 23);
            FrameCountTextBox.TabIndex = 2;
            FrameCountTextBox.KeyPress += FrameCountTextBox_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(40, 39);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 1;
            label2.Text = "Height :";
            // 
            // HeightTextbox
            // 
            HeightTextbox.Location = new Point(112, 36);
            HeightTextbox.Name = "HeightTextbox";
            HeightTextbox.Size = new Size(89, 23);
            HeightTextbox.TabIndex = 0;
            HeightTextbox.KeyPress += HeightTextbox_KeyPress;
            // 
            // PageLabel
            // 
            PageLabel.AutoSize = true;
            PageLabel.Location = new Point(187, 453);
            PageLabel.Name = "PageLabel";
            PageLabel.Size = new Size(39, 15);
            PageLabel.TabIndex = 0;
            PageLabel.Text = "label1";
            // 
            // PrevButton
            // 
            PrevButton.Location = new Point(153, 448);
            PrevButton.Name = "PrevButton";
            PrevButton.Size = new Size(28, 24);
            PrevButton.TabIndex = 2;
            PrevButton.Text = "<";
            PrevButton.UseVisualStyleBackColor = true;
            PrevButton.Click += PrevButton_Click;
            // 
            // NextButton
            // 
            NextButton.Location = new Point(232, 448);
            NextButton.Name = "NextButton";
            NextButton.Size = new Size(28, 24);
            NextButton.TabIndex = 3;
            NextButton.Text = ">";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Click += NextButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(354, 5);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(106, 30);
            SaveButton.TabIndex = 4;
            SaveButton.Text = "Image Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 70);
            label1.Name = "label1";
            label1.Size = new Size(46, 15);
            label1.TabIndex = 5;
            label1.Text = "Width :";
            // 
            // WdthTextbox
            // 
            WdthTextbox.Location = new Point(112, 67);
            WdthTextbox.Name = "WdthTextbox";
            WdthTextbox.Size = new Size(89, 23);
            WdthTextbox.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(945, 528);
            Controls.Add(SaveButton);
            Controls.Add(NextButton);
            Controls.Add(PrevButton);
            Controls.Add(PageLabel);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Controls.Add(PlayButton);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button PlayButton;
        private Panel panel1;
        private GroupBox groupBox1;
        private Label PageLabel;
        private Button PrevButton;
        private Button NextButton;
        private Label label2;
        private TextBox HeightTextbox;
        private Label label3;
        private TextBox FrameCountTextBox;
        private Button SaveButton;
        private Label label1;
        private TextBox WdthTextbox;
    }
}
