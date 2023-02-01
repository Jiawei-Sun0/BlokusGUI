using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Media;

namespace BlokusMod
{
    public partial class GameOver : Form
    {
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        private Label[] _labellist;
        public GameOver()
        {
            InitializeComponent();

            for (var i = 0; i < _game.NumPlayers; i++)
            {
                _board.PlayerRank.Add(_board.Records.IndexOf(_board.Records.Max()));
                _board.Records[_board.Records.IndexOf(_board.Records.Max())] = -1;
            }

            //SoundPlayer se = new SoundPlayer("../../victory_se.wav");
            //se.Play();
            var bmp = new Bitmap(winnerlabel.Width, winnerlabel.Height);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(_board.PieceBrushes[_board.PlayerRank[0]], 0, 0, bmp.Width, bmp.Height);
            g.DrawString($"Winner: {_game.Players[_board.PlayerRank[0]].Name} ! Point: {_board.Scores[_board.PlayerRank[0]]}", new Font("MV Boli", 12), Brushes.White, 3, 3);
            winnerlabel.Image = bmp;
            winnerlabel.Refresh();

            var btnSize = new Size(250, 30);
            _labellist = new Label[_game.NumPlayers];
            for (var i = 0; i < _game.NumPlayers-1; i++)
            {
                _labellist[i] = new Label();
                _labellist[i].Location = new Point(12, 60 + 30 * i);
                _labellist[i].Size = btnSize;
                _labellist[i].Font = new Font("MV Boli", 12); 
                _labellist[i].Text = $"{i+2}位: {_game.Players[_board.PlayerRank[i+1]].Name}, Point: {_board.Scores[_board.PlayerRank[i+1]]}";
                //Debug.WriteLine($"{_game.PlayerRank.Count()}");
                _labellist[i].Tag = i;
            }
            this.Controls.AddRange(_labellist);
        }
        public void Return2Start(object sender, EventArgs e)
        {
            Application.Restart();
        }
        public void Exit(object sender, EventArgs e)
        {
            this.Close();
            //System.Environment.Exit(0);
        }
    }
}
