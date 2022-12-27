namespace BlokusGUI
{
    partial class GameOver
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
            this.winnerlabel = new System.Windows.Forms.PictureBox();
            this.Restart = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.winnerlabel)).BeginInit();
            this.SuspendLayout();
            // 
            // winnerlabel
            // 
            this.winnerlabel.Location = new System.Drawing.Point(12, 19);
            this.winnerlabel.Name = "winnerlabel";
            this.winnerlabel.Size = new System.Drawing.Size(257, 30);
            this.winnerlabel.TabIndex = 0;
            this.winnerlabel.TabStop = false;
            // 
            // Restart
            // 
            this.Restart.Font = new System.Drawing.Font("MV Boli", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Restart.Location = new System.Drawing.Point(12, 281);
            this.Restart.Name = "Restart";
            this.Restart.Size = new System.Drawing.Size(116, 47);
            this.Restart.TabIndex = 1;
            this.Restart.Text = "Restart";
            this.Restart.UseVisualStyleBackColor = true;
            this.Restart.Click += new System.EventHandler(this.Return2Start);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("MV Boli", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(153, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Exit);
            // 
            // GameOver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 350);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Restart);
            this.Controls.Add(this.winnerlabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(1000, 1000);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameOver";
            this.Text = "Game Set";
            ((System.ComponentModel.ISupportInitialize)(this.winnerlabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox winnerlabel;
        private System.Windows.Forms.Button Restart;
        private System.Windows.Forms.Button button1;
    }
}