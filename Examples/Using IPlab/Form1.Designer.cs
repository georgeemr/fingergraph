namespace ImageProcessing
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.InputImagePb = new System.Windows.Forms.PictureBox();
            this.OutputImagePb = new System.Windows.Forms.PictureBox();
            this.ProcessBtn = new System.Windows.Forms.Button();
            this.ConvertBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.InputImagePb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputImagePb)).BeginInit();
            this.SuspendLayout();
            // 
            // InputImagePb
            // 
            this.InputImagePb.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.InputImagePb.Image = ((System.Drawing.Image)(resources.GetObject("InputImagePb.Image")));
            this.InputImagePb.Location = new System.Drawing.Point(12, 22);
            this.InputImagePb.Name = "InputImagePb";
            this.InputImagePb.Size = new System.Drawing.Size(320, 480);
            this.InputImagePb.TabIndex = 0;
            this.InputImagePb.TabStop = false;
            // 
            // OutputImagePb
            // 
            this.OutputImagePb.BackColor = System.Drawing.SystemColors.ControlLight;
            this.OutputImagePb.Location = new System.Drawing.Point(350, 22);
            this.OutputImagePb.Name = "OutputImagePb";
            this.OutputImagePb.Size = new System.Drawing.Size(318, 480);
            this.OutputImagePb.TabIndex = 1;
            this.OutputImagePb.TabStop = false;
            // 
            // ProcessBtn
            // 
            this.ProcessBtn.Location = new System.Drawing.Point(139, 519);
            this.ProcessBtn.Name = "ProcessBtn";
            this.ProcessBtn.Size = new System.Drawing.Size(143, 34);
            this.ProcessBtn.TabIndex = 2;
            this.ProcessBtn.Text = "Process";
            this.ProcessBtn.UseVisualStyleBackColor = true;
            this.ProcessBtn.Click += new System.EventHandler(this.ProcessBtn_Click);
            // 
            // ConvertBtn
            // 
            this.ConvertBtn.Location = new System.Drawing.Point(439, 519);
            this.ConvertBtn.Name = "ConvertBtn";
            this.ConvertBtn.Size = new System.Drawing.Size(137, 30);
            this.ConvertBtn.TabIndex = 3;
            this.ConvertBtn.Text = "Convert";
            this.ConvertBtn.UseVisualStyleBackColor = true;
            this.ConvertBtn.Click += new System.EventHandler(this.ConvertBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 565);
            this.Controls.Add(this.ConvertBtn);
            this.Controls.Add(this.ProcessBtn);
            this.Controls.Add(this.OutputImagePb);
            this.Controls.Add(this.InputImagePb);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.InputImagePb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputImagePb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox InputImagePb;
        private System.Windows.Forms.PictureBox OutputImagePb;
        private System.Windows.Forms.Button ProcessBtn;
        private System.Windows.Forms.Button ConvertBtn;
    }
}

