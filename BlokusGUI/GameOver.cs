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
            Debug.WriteLine(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            SoundPlayer se = new SoundPlayer("../../victory_se.wav");
            se.Play();
            var bmp = new Bitmap(winnerlabel.Width, winnerlabel.Height);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(_board.PieceBrushes[_game.PlayerRank[0]], 0, 0, bmp.Width, bmp.Height);
            g.DrawString($"Winner: Player {_game.PlayerRank[0] + 1} ! Point: {_game.Scores[_game.PlayerRank[0]]}", new Font("MV Boli", 13), Brushes.White, 3, 3);
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
                _labellist[i].Text = $"{i+2}位: Player {_game.PlayerRank[i+1] + 1}, Point: {_game.Scores[_game.PlayerRank[i+1]]}";
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
            System.Environment.Exit(0);
        }

        private void WindowClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
