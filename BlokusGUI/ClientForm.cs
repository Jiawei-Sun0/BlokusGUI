using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace BlokusMod {

    /// <summary>
    /// ブロックスクライアントメインフォーム
    /// </summary>
    public partial class ClientForm : Form {
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        //private Pieces _pieces = Pieces.GetInstance();  // ピースのインスタンス
        private Client _client = Client.GetInstance();  // クライアントのインスタンス
        private Button[] _pieceButtons;                 // ボタン配列
        private int _userMode = 0;                      // ユーザーモード 0:人間 1-:cpu
        private int _hold = -1;                         // ピースの選択状態
        private int _rotate = 0;                        // ピースの回転状態
        private PointF _mousePos = new PointF(-1, -1);  // マウス位置（pictureBox内相対位置）
        const int NUM_PIECES_ROW = 3;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClientForm() {
            InitializeComponent();
            _client.Init(this);

            // ピースボタン生成
            var btnSize = new Size(77, 53);
            _pieceButtons = new Button[_board.Pieces.Count()];
            for (var i = 0; i < _board.Pieces.Count(); i++) {
                _pieceButtons[i] = new Button();
                var col = i % NUM_PIECES_ROW;
                var row = i / NUM_PIECES_ROW;
                _pieceButtons[i].Location = new Point(150 + (btnSize.Width + 7) * col, 10 + (btnSize.Height + 7) * row);
                _pieceButtons[i].Size = btnSize;
                _pieceButtons[i].Text = _board.Pieces[i].GetShapeString();
                _pieceButtons[i].Tag = i;
                _pieceButtons[i].Visible = false;
                _pieceButtons[i].Click += new EventHandler(this.PieceButton_Click);
            }
            this.Controls.AddRange(_pieceButtons);
            this.pictureBox1.MouseWheel += new MouseEventHandler(this.pictureBox1_MouseWheel);

#if DEBUG
            // 名前をランダム生成
            var rnd = new Random();
            var name = "";
            for (int i = 0; i < 4; i++) name += Convert.ToChar(0x3041+(rnd.Next()%80)).ToString();
            TxtName.Text = name;    // 人が操作
            //TxtName.Text = "CPU" + name;    // CPU操作
#endif
        }

        /// <summary>
        /// グラフィクス描画
        /// </summary>
        private void Draw() {
            pictureBox1.Image = _board.DrawBoard(_hold, _mousePos, _rotate);
            pictureBox1.Refresh();
        }

        /// <summary>
        /// ピースボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PieceButton_Click(object sender, EventArgs e) {
            _hold = (int)((Button)sender).Tag;
        }

        /// <summary>
        /// 盤上でマウス移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            _mousePos.X = (float)e.X / pictureBox1.Width;
            _mousePos.Y = (float)e.Y / pictureBox1.Height;
            if (_hold >= 0) Draw();
        }

        /// <summary>
        /// 盤上でマウスクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e) {
            if (_hold < 0 || _hold >= _board.Pieces.Count()) return;

            if (((MouseEventArgs)e).Button == MouseButtons.Left) {
                var si = new SetInfo(_hold, _rotate, _board.CursorCell(_mousePos));
                if (_board.CheckPlace(_game.Turn, si)) {
                    _client.SetPiece(si);
                    _hold = -1;
                    Console.Beep(1600, 200);
                } else {
                    Console.Beep(800, 200);
                }
            }
            if (((MouseEventArgs)e).Button == MouseButtons.Right) {
                _hold = -1;
                this.Draw();
            }
        }

        /// <summary>
        /// マウスホイール
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e) {
            _rotate = (_rotate + (e.Delta > 0 ? 1 : -1) + 8) % 8;
            //Debug.WriteLine(_rotate);
            this.Draw();
        }

        /// <summary>
        /// フォーム更新（スレッドセーフ）
        /// </summary>
        public void UpdateForm(bool updateList = false) {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { UpdateForm(updateList); });
                return;
            }

            // コントロール制御
            BtnConnect.Visible = (_client.State == States.Unconnect);
            TxtServer.ReadOnly = (_client.State != States.Unconnect);
            TxtName.ReadOnly = (_client.State != States.Unconnect);

            if (_game.Players != null) {
                for (var i = 0; i < _board.Pieces.Count(); i++) {
                    _pieceButtons[i].Visible = (_client.State == States.Playing && !_game.Players[_game.TurnPlayer].PiecesUsed[i]);
                    _pieceButtons[i].Enabled = _client.IsMyTurn && (_userMode == 0);
                }
                BtnGiveUp.Visible = (_userMode == 0) && _client.IsMyTurn && _game.Players[_game.TurnPlayer].Alive;
                if (updateList) {
                    ListPlayers.Items.Clear();
                    var players = _game.PlayOrder.Select(c => _game.Players[c]).Select(c => new ListViewItem(new string[] { c.ID.ToString(), c.Name }));
                    //var players = _game.Players.Select(c => new ListViewItem(new string[] { c.ID.ToString(), c.Name }));
                    ListPlayers.Items.AddRange(players.ToArray());
                    for (int i = 0; i < ListPlayers.Items.Count; i++) {
                        ListPlayers.Items[i].BackColor = _board.PieceColors[i];
                    }
                }
                if (_client.IsMyTurn && _userMode > 0) {
                    Cpu.GetInstance().Turn();
                }
            }
            this.Draw();

            // ヘッダー表示
            var name = _game.Players == null ? "" : (_client.IsMyTurn ? "あなた" : _game.Players[_game.TurnPlayer].Name);
            switch (_client.State) {
            case States.Unconnect:
                TxtHeader.Text = "サーバーに接続してください．";
                break;
            case States.Prestart:
                TxtHeader.Text = "ゲーム開始待機中";
                //ListPlayers.Items.Clear();
                break;
            case States.Playing:
                TxtHeader.Text = $"ゲーム進行中：{name}の番です．";
                break;
            case States.Miss:
                TxtHeader.Text = $"{name}の不正操作（パス）．";
                break;
            case States.Giveup:
                TxtHeader.Text = $"{name}がギブアップ";
                break;
            case States.Gameover:
                TxtHeader.Text = $"ゲーム終了：{_client.WinnersName()}の勝利";
                break;
            }
        }

        /// <summary>
        /// 接続ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnect_Click(object sender, EventArgs e) {
            if (TxtName.Text.Count() < 1) {
                MessageBox.Show("名前を記入してください．");
                return;
            }
            _userMode = TxtName.Text.StartsWith("cpu",StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            _client.StartClient(TxtServer.Text, TxtName.Text);
            this.UpdateForm();
        }


        /// <summary>
        /// フォームを閉じるとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            _client.StopClient();
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
        /// フォームを開いたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e) {
            this.UpdateForm();
        }

        /// <summary>
        /// ギブアップボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGiveUp_Click(object sender, EventArgs e) {
            _client.GiveUp();
            this.UpdateForm();
            Console.Beep(1600, 200);
        }
    }
}
