
namespace BlokusGUI {
    partial class FormGameSetup {
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
            this.UpdownPlayers = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.UpdownBoardSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.UpdownPlayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdownBoardSize)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdownPlayers
            // 
            this.UpdownPlayers.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.UpdownPlayers.Location = new System.Drawing.Point(43, 47);
            this.UpdownPlayers.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.UpdownPlayers.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.UpdownPlayers.Name = "UpdownPlayers";
            this.UpdownPlayers.ReadOnly = true;
            this.UpdownPlayers.Size = new System.Drawing.Size(81, 55);
            this.UpdownPlayers.TabIndex = 0;
            this.UpdownPlayers.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UpdownPlayers.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "参加人数を選択";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(104, 133);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 45);
            this.button1.TabIndex = 2;
            this.button1.Text = "開始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(211, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "盤サイズを選択";
            // 
            // UpdownBoardSize
            // 
            this.UpdownBoardSize.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.UpdownBoardSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.UpdownBoardSize.Location = new System.Drawing.Point(224, 47);
            this.UpdownBoardSize.Maximum = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.UpdownBoardSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.UpdownBoardSize.Name = "UpdownBoardSize";
            this.UpdownBoardSize.ReadOnly = true;
            this.UpdownBoardSize.Size = new System.Drawing.Size(107, 55);
            this.UpdownBoardSize.TabIndex = 4;
            this.UpdownBoardSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UpdownBoardSize.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // FormGameSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 190);
            this.ControlBox = false;
            this.Controls.Add(this.UpdownBoardSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdownPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(1500, 1000);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGameSetup";
            this.ShowInTaskbar = false;
            this.Text = "BlokusGUI ゲーム条件の設定";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.UpdownPlayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdownBoardSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown UpdownPlayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown UpdownBoardSize;
    }
}