namespace FingerGraph {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.FingerprintImage = new System.Windows.Forms.PictureBox();
            this.FirstNameBox = new System.Windows.Forms.TextBox();
            this.LastNameBox = new System.Windows.Forms.TextBox();
            this.EnrollModeChk = new System.Windows.Forms.CheckBox();
            this.AutoIdentifyModeChk = new System.Windows.Forms.CheckBox();
            this.IdentifyBtn = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FingerprintImage)).BeginInit();
            this.SuspendLayout();
            // 
            // FingerprintImage
            // 
            this.FingerprintImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FingerprintImage.BackColor = System.Drawing.Color.White;
            this.FingerprintImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FingerprintImage.Location = new System.Drawing.Point(12, 12);
            this.FingerprintImage.Name = "FingerprintImage";
            this.FingerprintImage.Size = new System.Drawing.Size(276, 334);
            this.FingerprintImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FingerprintImage.TabIndex = 0;
            this.FingerprintImage.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(57, 349);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(29, 13);
            label1.TabIndex = 1;
            label1.Text = "Имя";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(186, 349);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 13);
            label2.TabIndex = 2;
            label2.Text = "Фамилия";
            // 
            // FirstNameBox
            // 
            this.FirstNameBox.Enabled = false;
            this.FirstNameBox.Location = new System.Drawing.Point(12, 365);
            this.FirstNameBox.Name = "FirstNameBox";
            this.FirstNameBox.Size = new System.Drawing.Size(135, 20);
            this.FirstNameBox.TabIndex = 3;
            // 
            // LastNameBox
            // 
            this.LastNameBox.Enabled = false;
            this.LastNameBox.Location = new System.Drawing.Point(153, 365);
            this.LastNameBox.Name = "LastNameBox";
            this.LastNameBox.Size = new System.Drawing.Size(135, 20);
            this.LastNameBox.TabIndex = 4;
            // 
            // EnrollModeChk
            // 
            this.EnrollModeChk.AutoSize = true;
            this.EnrollModeChk.Location = new System.Drawing.Point(12, 391);
            this.EnrollModeChk.Name = "EnrollModeChk";
            this.EnrollModeChk.Size = new System.Drawing.Size(128, 17);
            this.EnrollModeChk.TabIndex = 5;
            this.EnrollModeChk.Text = "Режим регистрации";
            this.EnrollModeChk.UseVisualStyleBackColor = true;
            // 
            // AutoIdentifyModeChk
            // 
            this.AutoIdentifyModeChk.AutoSize = true;
            this.AutoIdentifyModeChk.Location = new System.Drawing.Point(153, 391);
            this.AutoIdentifyModeChk.Name = "AutoIdentifyModeChk";
            this.AutoIdentifyModeChk.Size = new System.Drawing.Size(147, 17);
            this.AutoIdentifyModeChk.TabIndex = 6;
            this.AutoIdentifyModeChk.Text = "Автоматический режим";
            this.AutoIdentifyModeChk.UseVisualStyleBackColor = true;
            // 
            // IdentifyBtn
            // 
            this.IdentifyBtn.Location = new System.Drawing.Point(112, 414);
            this.IdentifyBtn.Name = "IdentifyBtn";
            this.IdentifyBtn.Size = new System.Drawing.Size(75, 23);
            this.IdentifyBtn.TabIndex = 7;
            this.IdentifyBtn.Text = "Распознать";
            this.IdentifyBtn.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 443);
            this.Controls.Add(this.IdentifyBtn);
            this.Controls.Add(this.AutoIdentifyModeChk);
            this.Controls.Add(this.EnrollModeChk);
            this.Controls.Add(this.LastNameBox);
            this.Controls.Add(this.FirstNameBox);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.FingerprintImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FingerGraph";
            ((System.ComponentModel.ISupportInitialize)(this.FingerprintImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox FingerprintImage;
        private System.Windows.Forms.TextBox FirstNameBox;
        private System.Windows.Forms.TextBox LastNameBox;
        private System.Windows.Forms.CheckBox EnrollModeChk;
        private System.Windows.Forms.CheckBox AutoIdentifyModeChk;
        private System.Windows.Forms.Button IdentifyBtn;
    }
}

