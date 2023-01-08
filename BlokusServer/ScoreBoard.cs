using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlokusMod {
    public partial class ScoreBoard : Form {

        private TextBox[] _txtNames;
        private TextBox[] _txtScores;
        private int _numPlayers;
        private int _numPlay;

        public ScoreBoard(List<string> names, int numPlay) {
            InitializeComponent();

            _numPlayers = names.Count();
            _numPlay = numPlay;
            var nameSize = new Size(150, 35);
            var scoreSize = new Size(150, 100);
            var margin = 12;
            var header = 50;
            var rowHeight = margin * 5 + nameSize.Height + scoreSize.Height;

            txtGameInfo.Text = $"1/{_numPlay} 試合目対戦中";

            _txtNames = new TextBox[_numPlayers];
            for (var i = 0; i < _numPlayers; i++) {
                var c = i % 4;
                var r = i / 4;
                _txtNames[i] = new TextBox();
                _txtNames[i].Location = new Point(margin + c * (nameSize.Width + margin), header + margin + rowHeight * r);
                _txtNames[i].Size = nameSize;
                _txtNames[i].Text = names[i];
                _txtNames[i].Font = new Font("MS UI Gothic", 20);
                _txtNames[i].TextAlign = HorizontalAlignment.Center;
                _txtNames[i].BorderStyle = BorderStyle.None;
                //_txtNames[i].Enabled = false;
            }
            this.Controls.AddRange(_txtNames);

            _txtScores = new TextBox[_numPlayers];
            for (var i = 0; i < _numPlayers; i++) {
                var c = i % 4;
                var r = i / 4;
                _txtScores[i] = new TextBox();
                _txtScores[i].Location = new Point(margin + c * (scoreSize.Width + margin), header + margin * 2 + nameSize.Height + rowHeight * r);
                _txtScores[i].Size = scoreSize;
                _txtScores[i].Text = $"0";
                _txtScores[i].Font = new Font("MS UI Gothic", 72);
                _txtScores[i].TextAlign = HorizontalAlignment.Center;
                _txtScores[i].BorderStyle = BorderStyle.None;
                //_txtScores[i].Enabled = false;
            }
            this.Controls.AddRange(_txtScores);

            var clientWidth = (nameSize.Width + margin) * (_numPlayers > 3 ? 4 : _numPlayers) + margin;
            var clientHeight = header + ((_numPlayers > 4) ? rowHeight * 2 : rowHeight);
            this.ClientSize = new Size(clientWidth, clientHeight);
        }

        public void ShowScore(List<int> scores, int currentPlay) {
            if (this.InvokeRequired) {
                this.Invoke((MethodInvoker)delegate { ShowScore(scores, currentPlay); });
                return;
            }

            if (currentPlay < 1) txtGameInfo.Text = $"{_numPlay} 試合終了";
            else txtGameInfo.Text = $"{1 + _numPlay - currentPlay}/{_numPlay} 試合目対戦中";
            for (var i = 0; i < _numPlayers; i++) {
                _txtScores[i].Text = $"{scores[i]}";
            }
        }
    }
}
