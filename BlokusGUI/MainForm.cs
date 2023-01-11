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
using System.Media;

namespace BlokusMod
{
    public partial class MainForm : Form {
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        private Client _client = Client.GetInstance();  // クライアントのインスタンス
        private Button[] _pieceButtons;                 // ボタン配列
        private int _hold = -1;                         // ピースの選択状態
        private int _userMode = 0;                      // ユーザーモード 0:人間 1-:cpu
        private int _rotate = 0;
        private PointF _mousePos = new PointF(-1, -1);  // マウス位置（pictureBox内相対位置）
        private Label[] _scorelist;
        const int NUM_PIECES_ROW = 2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm() {
            InitializeComponent();
            _client.Init(this);
            // ゲーム条件設定
            //var frmSelectPlayers = new FormGameSetup();
            //frmSelectPlayers.ShowDialog();

            //SoundPlayer se = new SoundPlayer("../../menu_open_se_1.wav");
            //se.Play();
            //// 初期化
            //_game.Initialize(frmSelectPlayers.NumPlayers);
            //_board.Initialize(frmSelectPlayers.BoardSize);
            //this.Draw();

            // ピースボタン生成
            var btnSize = new Size(77, 53);
            _pieceButtons = new Button[_board.Pieces.Count()];
            for (var i=0; i< _board.Pieces.Count(); i++) {
                _pieceButtons[i] = new Button();
                var col = i % NUM_PIECES_ROW;
                var row = i / NUM_PIECES_ROW;
                _pieceButtons[i].Location = new Point(12 + 84 * col, 45 + 60 * row);
                _pieceButtons[i].Size = btnSize;
                _pieceButtons[i].Text = _board.Pieces[i].GetShapeString();
                _pieceButtons[i].Tag = i;
                _pieceButtons[i].Visible = false;
                _pieceButtons[i].Click += new EventHandler(this.PieceButton_Click);
            }

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.Key_Down);
            this.Controls.AddRange(_pieceButtons);
            //this.UpdateController();
#if DEBUG   
            // 名前をランダム生成
            var rnd = new Random();
            var name = "";
            for (int i = 0; i < 4; i++) name += Convert.ToChar(0x3041 + (rnd.Next() % 80)).ToString();
            TxtName.Text = name;    // 人が操作
            //TxtName.Text = "CPU" + name;    // CPU操作
#endif
        }  
        private void Key_Down(object sender, KeyEventArgs e) {
            if (_hold < 0 || _hold >= _board.Pieces.Count())
            {
                return;
            }
            else
            {
                int adj = _rotate > 3 ? 4 : 0;

                if (e.KeyCode.ToString() == "A")
                {
                    _rotate -= 1;
                    if (_rotate < 0 + adj)
                    {
                        _rotate += 4;
                    }
                }
                if (e.KeyCode.ToString() == "D")
                {
                    _rotate += 1;
                    if (_rotate > 3 + adj)
                    {
                        _rotate -= 4;
                    }
                }
                if (e.KeyCode.ToString() == "W") // REVERSE PIECE
                {
                    if (_rotate < 4)
                    {
                        _rotate += 4;
                    }
                    else if (_rotate >= 4)
                    {
                        _rotate -= 4;
                    }

                }
                Debug.WriteLine($"{_hold}::{_rotate}");
                this.Draw();
                return;
            }
        }
        /// <summary>
        /// グラフィクス描画
        /// </summary>
        private void Draw()
        {
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
                if (_board.CheckPlace(_game.Turn, si))
                {
                    _client.SetPiece(si);
                    _hold = -1;
                    SoundPlayer se = new SoundPlayer("../../setpiece_se_1.wav");
                    se.Play();
                    //this.UpdateController();
                } else {
                    SoundPlayer se = new SoundPlayer("../../wrongplace_se.wav");
                    se.Play();
                }
            }
            if (((MouseEventArgs)e).Button == MouseButtons.Right) {
                _hold = -1;
                this.Draw();
            }
        }

        /// <summary>
        /// 操作部の状態更新
        /// </summary>
        private void UpdateController() {
            var bmp = new Bitmap(PicPlayerLabel.Width, PicPlayerLabel.Height);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(_board.PieceBrushes[_game.TurnPlayer], 0, 0, bmp.Width, bmp.Height);
            g.DrawString($"プレイヤー {_game.TurnPlayer + 1} の番", new Font("MS UI Gothic", 14), Brushes.White, 3, 3);
            PicPlayerLabel.Image = bmp;
            PicPlayerLabel.Refresh();
            
            for (var i = 0; i < _board.Pieces.Count(); i++) {
                _pieceButtons[i].Enabled = !_game.Players[_game.TurnPlayer].PiecesUsed[i];
            }

            for (var i = 0; i < _game.NumPlayers; i++)
            {
                if (_game.Players[i].Alive == false)
                {
                    _scorelist[i].Text = $"Player {i + 1}: {_board.Scores[i]} ×";
                    continue;
                }
                _scorelist[i].Text = $"Player {i + 1}: {_board.Scores[i]}";
            }

        }
        public void UpdateForm(bool updateList = false)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { UpdateForm(updateList); });
                return;
            }

            // コントロール制御
            BtnConnect.Visible = (_client.State == States.Unconnect);
            TxtServer.ReadOnly = (_client.State != States.Unconnect);
            TxtName.ReadOnly = (_client.State != States.Unconnect);

            if (_game.Players != null)
            {
                for (var i = 0; i < _board.Pieces.Count(); i++)
                {
                    _pieceButtons[i].Visible = (_client.State == States.Playing && !_game.Players[_game.TurnPlayer].PiecesUsed[i]);
                    _pieceButtons[i].Enabled = _client.IsMyTurn && (_userMode == 0);
                }
                button1.Visible = (_userMode == 0) && _client.IsMyTurn && _game.Players[_game.TurnPlayer].Alive;
                if (updateList)
                {
                    ListPlayers.Items.Clear();
                    var players = _game.PlayOrder.Select(c => _game.Players[c]).Select(c => new ListViewItem(new string[] { c.ID.ToString(), c.Name }));
                    //var players = _game.Players.Select(c => new ListViewItem(new string[] { c.ID.ToString(), c.Name }));
                    ListPlayers.Items.AddRange(players.ToArray());
                    for (int i = 0; i < ListPlayers.Items.Count; i++)
                    {
                        ListPlayers.Items[i].BackColor = _board.PieceColors[i];
                    }
                }
                if (_client.IsMyTurn && _userMode > 0)
                {
                    Cpu.GetInstance().Turn();
                }
            }
            this.Draw();
            if (_client.State == States.Preplaying)
            {
                CreateScoreList();
                _client.State = States.Playing;
            }
            if (_client.State == States.Playing)
                ScoreUpdate();
            if (_client.State == States.Gameover)
                foreach (Label l in _scorelist)
                    l.Dispose();

            // ヘッダー表示
            var name = _game.Players == null ? "" : (_client.IsMyTurn ? "あなた" : _game.Players[_game.TurnPlayer].Name);
            switch (_client.State)
            {
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
        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (TxtName.Text.Count() < 1)
            {
                MessageBox.Show("名前を記入してください．");
                return;
            }
            _userMode = TxtName.Text.StartsWith("cpu", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            _client.StartClient(TxtServer.Text, TxtName.Text);
            this.UpdateForm();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _client.StopClient();
        }
        public void Message(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { Message(msg); });
                return;
            }
            TxtMessage.Text += msg + "\r\n";
            TxtMessage.SelectionStart = TxtMessage.TextLength;
            TxtMessage.ScrollToCaret();
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.UpdateForm();
        }

        /// <summary>
        /// ギブアップボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGiveUp_Click(object sender, EventArgs e)
        {
            _client.GiveUp();
            this.UpdateForm();
            SoundPlayer se = new SoundPlayer("../../giveup_se_1.wav");
            se.Play();
        }
        public void ScoreUpdate()
        {
            _board.CalculateScore();
            for (var i = 0; i < _game.NumPlayers; i++)
            {
                if (_game.Players[i].Alive == false)
                {
                    _scorelist[i].Text = $"{_game.Players[i].Name}: {_board.Scores[i]} ×";
                    continue;
                }
                _scorelist[i].Text = $"{_game.Players[i].Name}: {_board.Scores[i]}";
                Debug.WriteLine($"ss{i}:{_board.Scores[i]}");
            }
        }
        public void CreateScoreList()
        {
            _scorelist = new Label[_game.NumPlayers];
            for (var i = 0; i < _game.NumPlayers; i++)
            {
                _scorelist[i] = new Label
                {
                    Location = new Point(866, 60 + 50 * i),
                    Font = new Font("MV Boli", 15),
                    Size = new Size(200, 30),
                    BackColor = _board.PieceColors[i],
                    ForeColor = Color.Black,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Anchor = AnchorStyles.Right | AnchorStyles.Top
                };
            }
            this.Controls.AddRange(_scorelist);
        }
    }
}
