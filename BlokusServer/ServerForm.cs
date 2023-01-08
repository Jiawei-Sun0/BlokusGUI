using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BlokusMod {

    /// <summary>
    /// ブロックスサーバーメインフォーム
    /// </summary>
    public partial class ServerForm : Form {

        private Server _server = Server.GetInstance();  // サーバーのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        private Game _game = Game.GetInstance();     // ゲームのインスタンス
#if DEBUG
        const int MIN_PLAYERS_TO_PLAY = 1;  // デバッグ時は一人プレイ可能にする
#else
        const int MIN_PLAYERS_TO_PLAY = 2;
#endif

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ServerForm() {
            InitializeComponent();
            _server.Init(this);
        }

        /// <summary>
        /// フォームを開いたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e) {
            _server.StartServer();
            TxtServerIP.Text = _server.ServerIP;
            this.UpdateForm();
        }

        /// <summary>
        /// フォームを閉じるとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            _server.StopServer();
        }

        /// <summary>
        /// メッセージ表示（スレッドセーフ）
        /// </summary>
        /// <param name="msg"></param>
        public void Message(string msg) {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { Message(msg); });
                return;
            }
            TxtMessage.Text += msg + "\r\n";
            TxtMessage.SelectionStart = TxtMessage.TextLength;
            TxtMessage.ScrollToCaret();
        }

        /// <summary>
        /// フォーム更新（スレッドセーフ）
        /// </summary>
        public void UpdateForm(bool updateList = false) {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { UpdateForm(updateList); });
                return;
            }
            switch (_server.State) {
            case States.Lobby:
                TxtHeader.Text = $"参加者受付中";
                BtnGameStart.Text = "ゲーム開始";
                BtnGameStart.Visible = _server.Clients.Count >= MIN_PLAYERS_TO_PLAY;
                break;
            case States.Playing:
                TxtHeader.Text = $"ゲーム進行中：{_server.TurnPlayer.Name}の番です";
                BtnGameStart.Visible = false;
                break;
            case States.Gameover:
                TxtHeader.Text = $"ゲーム終了：{_server.WinnersName()}の勝利";
                if (_server.AutoPlayLast > 0) {
                    Thread.Sleep(_server.MessageDulation);
                    //MessageBox.Show("Pause"); // ゲーム終了時に一時停止
                    _server.StartGame();
                } else {
                    BtnGameStart.Text = "最初に戻る";
                    BtnGameStart.Visible = true;
                }
                break;
            }
            PicBoard.Image = _server.GetBoardImage();
            if (updateList) {
                ListPlayers.Items.Clear();
                var clients = _game.PlayOrder == null ? _server.Clients : _game.PlayOrder.Select(p => _server.Clients[p]);
                var players = clients.Select(c => new ListViewItem(new string[] { c.ClientID.ToString(), c.Name }));
                ListPlayers.Items.AddRange(players.ToArray());
                for (int i = 0; i < ListPlayers.Items.Count; i++) {
                    ListPlayers.Items[i].BackColor = _board.PieceColors[i];
                }
            }
        }

        /// <summary>
        /// ゲーム開始ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGameStart_Click(object sender, EventArgs e) {
            if (_server.State == States.Lobby) {
                _server.StartGame();
            }
            if (_server.State == States.Gameover) {
                _server.MoveToLobbby();
            }
        }
    }
}
