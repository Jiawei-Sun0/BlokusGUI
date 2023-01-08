
namespace BlokusMod {
    partial class ServerForm {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.ListPlayers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BtnGameStart = new System.Windows.Forms.Button();
            this.TxtMessage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PicBoard = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtHeader = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtServerIP = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // ListPlayers
            // 
            this.ListPlayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.ListPlayers.GridLines = true;
            this.ListPlayers.HideSelection = false;
            this.ListPlayers.Location = new System.Drawing.Point(12, 81);
            this.ListPlayers.Name = "ListPlayers";
            this.ListPlayers.Size = new System.Drawing.Size(117, 176);
            this.ListPlayers.TabIndex = 0;
            this.ListPlayers.UseCompatibleStateImageBehavior = false;
            this.ListPlayers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "名前";
            this.columnHeader2.Width = 70;
            // 
            // BtnGameStart
            // 
            this.BtnGameStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BtnGameStart.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BtnGameStart.Location = new System.Drawing.Point(12, 274);
            this.BtnGameStart.Name = "BtnGameStart";
            this.BtnGameStart.Size = new System.Drawing.Size(117, 36);
            this.BtnGameStart.TabIndex = 1;
            this.BtnGameStart.Text = "ゲーム開始";
            this.BtnGameStart.UseVisualStyleBackColor = false;
            this.BtnGameStart.Click += new System.EventHandler(this.BtnGameStart_Click);
            // 
            // TxtMessage
            // 
            this.TxtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMessage.BackColor = System.Drawing.Color.White;
            this.TxtMessage.Location = new System.Drawing.Point(12, 351);
            this.TxtMessage.Multiline = true;
            this.TxtMessage.Name = "TxtMessage";
            this.TxtMessage.ReadOnly = true;
            this.TxtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtMessage.Size = new System.Drawing.Size(434, 87);
            this.TxtMessage.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(9, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "参加プレイヤー";
            // 
            // PicBoard
            // 
            this.PicBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicBoard.BackColor = System.Drawing.SystemColors.Control;
            this.PicBoard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PicBoard.Location = new System.Drawing.Point(146, 41);
            this.PicBoard.Name = "PicBoard";
            this.PicBoard.Size = new System.Drawing.Size(300, 300);
            this.PicBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBoard.TabIndex = 8;
            this.PicBoard.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(12, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "通信ログ";
            // 
            // TxtHeader
            // 
            this.TxtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtHeader.BackColor = System.Drawing.Color.LemonChiffon;
            this.TxtHeader.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TxtHeader.Location = new System.Drawing.Point(146, 9);
            this.TxtHeader.Name = "TxtHeader";
            this.TxtHeader.ReadOnly = true;
            this.TxtHeader.Size = new System.Drawing.Size(300, 26);
            this.TxtHeader.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "サーバーIPアドレス";
            // 
            // TxtServerIP
            // 
            this.TxtServerIP.Location = new System.Drawing.Point(12, 27);
            this.TxtServerIP.Name = "TxtServerIP";
            this.TxtServerIP.ReadOnly = true;
            this.TxtServerIP.Size = new System.Drawing.Size(117, 19);
            this.TxtServerIP.TabIndex = 12;
            this.TxtServerIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 450);
            this.Controls.Add(this.TxtServerIP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtHeader);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PicBoard);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtMessage);
            this.Controls.Add(this.BtnGameStart);
            this.Controls.Add(this.ListPlayers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(474, 489);
            this.Name = "ServerForm";
            this.Text = "Blokus Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.PicBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ListPlayers;
        private System.Windows.Forms.Button BtnGameStart;
        private System.Windows.Forms.TextBox TxtMessage;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox PicBoard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtHeader;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtServerIP;
    }
}

