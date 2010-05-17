namespace AlgTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.pictureBoxIn = new System.Windows.Forms.PictureBox();
            this.pictureBoxFiltred = new System.Windows.Forms.PictureBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.openFileDialogImg = new System.Windows.Forms.OpenFileDialog();
            this.testBtn2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFiltred)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxIn
            // 
            this.pictureBoxIn.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxIn.Name = "pictureBoxIn";
            this.pictureBoxIn.Size = new System.Drawing.Size(221, 206);
            this.pictureBoxIn.TabIndex = 0;
            this.pictureBoxIn.TabStop = false;
            this.pictureBoxIn.Click += new System.EventHandler(this.pictureBoxIn_Click);
            // 
            // pictureBoxFiltred
            // 
            this.pictureBoxFiltred.Location = new System.Drawing.Point(250, 12);
            this.pictureBoxFiltred.Name = "pictureBoxFiltred";
            this.pictureBoxFiltred.Size = new System.Drawing.Size(207, 206);
            this.pictureBoxFiltred.TabIndex = 1;
            this.pictureBoxFiltred.TabStop = false;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(12, 286);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(207, 46);
            this.buttonTest.TabIndex = 2;
            this.buttonTest.Text = "1";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // testBtn2
            // 
            this.testBtn2.Location = new System.Drawing.Point(250, 286);
            this.testBtn2.Name = "testBtn2";
            this.testBtn2.Size = new System.Drawing.Size(207, 46);
            this.testBtn2.TabIndex = 3;
            this.testBtn2.Text = "2";
            this.testBtn2.UseVisualStyleBackColor = true;
            this.testBtn2.Click += new System.EventHandler(this.testBtn2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.testBtn2);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.pictureBoxFiltred);
            this.Controls.Add(this.pictureBoxIn);
            this.Name = "Form1";
            this.Text = "Test";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFiltred)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxIn;
        private System.Windows.Forms.PictureBox pictureBoxFiltred;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.OpenFileDialog openFileDialogImg;
        private System.Windows.Forms.Button testBtn2;
    }
}

