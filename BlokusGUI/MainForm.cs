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

namespace BlokusGUI {
    public partial class MainForm : Form {
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        private Pieces _pieces = Pieces.GetInstance();  // ピースのインスタンス
        private Button[] _pieceButtons;                 // ボタン配列
        private int _hold = -1;                         // ピースの選択状態
        private int _rotate = 0;
        private PointF _mousePos = new PointF(-1, -1);  // マウス位置（pictureBox内相対位置）
        private Label[] _scorelist;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm() {
            InitializeComponent();

            // ゲーム条件設定
            var frmSelectPlayers = new FormGameSetup();
            frmSelectPlayers.ShowDialog();

            // 初期化
            _game.Initialize(frmSelectPlayers.NumPlayers);
            _board.Initialize(frmSelectPlayers.BoardSize);
            this.Draw();

            // ピースボタン生成
            var btnSize = new Size(77, 53);
            _pieceButtons = new Button[_pieces.NumPieces()];
            for (var i=0; i< _pieces.NumPieces(); i++) {
                _pieceButtons[i] = new Button();
                var col = i % 2;
                var row = i / 2;
                _pieceButtons[i].Location = new Point(12 + 84 * col, 45 + 60 * row);
                _pieceButtons[i].Size = btnSize;
                _pieceButtons[i].Text = _pieces.GetShapeString(i);
                _pieceButtons[i].Tag = i;
                _pieceButtons[i].Click += new EventHandler(this.PieceButton_Click);
            }

            _scorelist = new Label[_game.NumPlayers];
            for (var i = 0; i < _game.NumPlayers; i++)
            {
                _scorelist[i] = new Label
                {
                    Location = new Point(866, 60 + 30 * i),
                    Font = new Font("MS UI Gothic", 15),
                    Size = new Size(200, 30),
                    ForeColor = _board.ScoreColors[i],
                    Text = $"Player {i + 1}: {_game.Scores[i]}"
                };
                Debug.WriteLine($"{_game.PlayerRank.Count()}");
                _scorelist[i].Tag = i;
            }
            this.Controls.AddRange(_scorelist);

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.Key_Down);
            this.Controls.AddRange(_pieceButtons);
            this.UpdateController();
        }

        /// <summary>
        /// グラフィクス描画
        /// </summary>
        private void Draw() {
            pictureBox1.Image = _board.DrawBoard(_hold, _mousePos, _game.TurnPlayer, _game.Players[_game.TurnPlayer].first, _rotate);
            pictureBox1.Refresh();
        }

        private void Giveup(object sender, EventArgs e) {
            _game.GiveUp();
            this.UpdateController();
        }
        private void Key_Down(object sender, KeyEventArgs e) {
            if (_hold < 0 || _hold >= _pieces.NumPieces())
            {
                return;
            }
            else
            {
                if (e.KeyCode.ToString() == "A")
                {
                    _pieces.RotatePiece(_hold, 0);
                }
                if (e.KeyCode.ToString() == "D")
                {
                    _pieces.RotatePiece(_hold, 1);
                }
                return;
            }
            
        }
        /// <summary>
        /// ピースボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PieceButton_Click(object sender, EventArgs e) {
            _hold = (int)((Button)sender).Tag;
            _pieces.ResetRotation(_hold);
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
            if (_hold < 0 || _hold >= _pieces.NumPieces()) return;

            if (((MouseEventArgs)e).Button == MouseButtons.Left) {
                if (_board.SetPiece(_game.TurnPlayer, _hold, _mousePos, _game.Players[_game.TurnPlayer].first, _rotate)) {
                    _game.Players[_game.TurnPlayer].first = false;
                    _game.SetPiece(_hold);
                    _hold = -1;
                    this.Draw();
                    Console.Beep(1600, 200);
                    this.UpdateController();
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
        /// 操作部の状態更新
        /// </summary>
        private void UpdateController() {
            var bmp = new Bitmap(PicPlayerLabel.Width, PicPlayerLabel.Height);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(_board.PieceColors[_game.TurnPlayer], 0, 0, bmp.Width, bmp.Height);
            g.DrawString($"プレイヤー {_game.TurnPlayer + 1} の番", new Font("MS UI Gothic", 14), Brushes.White, 3, 3);
            PicPlayerLabel.Image = bmp;
            PicPlayerLabel.Refresh();
            
            for (var i = 0; i < _pieces.NumPieces(); i++) {
                _pieceButtons[i].Enabled = !_game.Players[_game.TurnPlayer].PiecesUsed[i];
            }

            for (var i = 0; i < _game.NumPlayers; i++)
            {
                if (_game.Players[i].Alive == false)
                {
                    _scorelist[i].Text = $"Player {i + 1}: {_game.Scores[i]} ×";
                    continue;
                }
                _scorelist[i].Text = $"Player {i + 1}: {_game.Scores[i]}";
            }

        }

        private void PicPlayerLabel_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //_pieces.GetShape(_hold).ForEach(p => Debug.WriteLine($"key:{p.X},{p.Y}"));
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
