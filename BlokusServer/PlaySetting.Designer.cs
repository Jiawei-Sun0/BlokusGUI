
namespace BlokusMod {
    partial class PlaySetting {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaySetting));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TxtNumGames = new System.Windows.Forms.TextBox();
            this.RadMultiGames = new System.Windows.Forms.RadioButton();
            this.RadOneGame = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TxtBoardSizeList = new System.Windows.Forms.TextBox();
            this.TxtBoardSize = new System.Windows.Forms.TextBox();
            this.RadRandomSize = new System.Windows.Forms.RadioButton();
            this.RadFixedSize = new System.Windows.Forms.RadioButton();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.ChkShuffleOrder = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TxtNumGames);
            this.groupBox1.Controls.Add(this.RadMultiGames);
            this.groupBox1.Controls.Add(this.RadOneGame);
            this.groupBox1.Location = new System.Drawing.Point(26, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 98);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "試合数";
            // 
            // TxtNumGames
            // 
            this.TxtNumGames.Enabled = false;
            this.TxtNumGames.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TxtNumGames.Location = new System.Drawing.Point(119, 58);
            this.TxtNumGames.Name = "TxtNumGames";
            this.TxtNumGames.Size = new System.Drawing.Size(45, 26);
            this.TxtNumGames.TabIndex = 4;
            this.TxtNumGames.Text = "5";
            this.TxtNumGames.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RadMultiGames
            // 
            this.RadMultiGames.AutoSize = true;
            this.RadMultiGames.Location = new System.Drawing.Point(24, 59);
            this.RadMultiGames.Name = "RadMultiGames";
            this.RadMultiGames.Size = new System.Drawing.Size(59, 16);
            this.RadMultiGames.TabIndex = 3;
            this.RadMultiGames.Text = "多試合";
            this.RadMultiGames.UseVisualStyleBackColor = true;
            this.RadMultiGames.CheckedChanged += new System.EventHandler(this.RadMultiGames_CheckedChanged);
            // 
            // RadOneGame
            // 
            this.RadOneGame.AutoSize = true;
            this.RadOneGame.Checked = true;
            this.RadOneGame.Location = new System.Drawing.Point(24, 27);
            this.RadOneGame.Name = "RadOneGame";
            this.RadOneGame.Size = new System.Drawing.Size(55, 16);
            this.RadOneGame.TabIndex = 1;
            this.RadOneGame.TabStop = true;
            this.RadOneGame.Text = "１試合";
            this.RadOneGame.UseVisualStyleBackColor = true;
            this.RadOneGame.CheckedChanged += new System.EventHandler(this.RadOneGame_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TxtBoardSizeList);
            this.groupBox2.Controls.Add(this.TxtBoardSize);
            this.groupBox2.Controls.Add(this.RadRandomSize);
            this.groupBox2.Controls.Add(this.RadFixedSize);
            this.groupBox2.Location = new System.Drawing.Point(26, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 95);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ボードサイズ";
            // 
            // TxtBoardSizeList
            // 
            this.TxtBoardSizeList.Enabled = false;
            this.TxtBoardSizeList.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TxtBoardSizeList.Location = new System.Drawing.Point(119, 54);
            this.TxtBoardSizeList.Name = "TxtBoardSizeList";
            this.TxtBoardSizeList.Size = new System.Drawing.Size(126, 26);
            this.TxtBoardSizeList.TabIndex = 3;
            this.TxtBoardSizeList.Text = "11,15,21";
            // 
            // TxtBoardSize
            // 
            this.TxtBoardSize.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TxtBoardSize.Location = new System.Drawing.Point(119, 19);
            this.TxtBoardSize.Name = "TxtBoardSize";
            this.TxtBoardSize.Size = new System.Drawing.Size(45, 26);
            this.TxtBoardSize.TabIndex = 2;
            this.TxtBoardSize.Text = "11";
            this.TxtBoardSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RadRandomSize
            // 
            this.RadRandomSize.AutoSize = true;
            this.RadRandomSize.Location = new System.Drawing.Point(24, 55);
            this.RadRandomSize.Name = "RadRandomSize";
            this.RadRandomSize.Size = new System.Drawing.Size(59, 16);
            this.RadRandomSize.TabIndex = 1;
            this.RadRandomSize.Text = "ランダム";
            this.RadRandomSize.UseVisualStyleBackColor = true;
            this.RadRandomSize.CheckedChanged += new System.EventHandler(this.RadRandomSize_CheckedChanged);
            // 
            // RadFixedSize
            // 
            this.RadFixedSize.AutoSize = true;
            this.RadFixedSize.Checked = true;
            this.RadFixedSize.Location = new System.Drawing.Point(24, 22);
            this.RadFixedSize.Name = "RadFixedSize";
            this.RadFixedSize.Size = new System.Drawing.Size(47, 16);
            this.RadFixedSize.TabIndex = 0;
            this.RadFixedSize.TabStop = true;
            this.RadFixedSize.Text = "固定";
            this.RadFixedSize.UseVisualStyleBackColor = true;
            this.RadFixedSize.CheckedChanged += new System.EventHandler(this.RadFixedSize_CheckedChanged);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(26, 328);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(121, 38);
            this.BtnStart.TabIndex = 5;
            this.BtnStart.Text = "開始";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(169, 328);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(121, 38);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "キャンセル";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ChkShuffleOrder
            // 
            this.ChkShuffleOrder.AutoSize = true;
            this.ChkShuffleOrder.Location = new System.Drawing.Point(50, 278);
            this.ChkShuffleOrder.Name = "ChkShuffleOrder";
            this.ChkShuffleOrder.Size = new System.Drawing.Size(129, 16);
            this.ChkShuffleOrder.TabIndex = 7;
            this.ChkShuffleOrder.Text = "プレー順番のシャッフル";
            this.ChkShuffleOrder.UseVisualStyleBackColor = true;
            // 
            // PlaySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 378);
            this.Controls.Add(this.ChkShuffleOrder);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlaySetting";
            this.Text = "ゲーム条件の設定";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TxtNumGames;
        private System.Windows.Forms.RadioButton RadMultiGames;
        private System.Windows.Forms.RadioButton RadOneGame;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TxtBoardSizeList;
        private System.Windows.Forms.TextBox TxtBoardSize;
        private System.Windows.Forms.RadioButton RadRandomSize;
        private System.Windows.Forms.RadioButton RadFixedSize;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.CheckBox ChkShuffleOrder;
    }
}