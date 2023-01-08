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
    public partial class PlaySetting : Form {

        public int NumGames { get; private set; }
        public List<int> BoardSizeList { get; private set; }
        public bool ShuffleOrder { get; private set; } = false;

        public PlaySetting() {
            InitializeComponent();
        }

        private void RadOneGame_CheckedChanged(object sender, EventArgs e) {
            TxtNumGames.Enabled = false;
        }

        private void RadMultiGames_CheckedChanged(object sender, EventArgs e) {
            TxtNumGames.Enabled = true;
        }

        private void RadFixedSize_CheckedChanged(object sender, EventArgs e) {
            TxtBoardSize.Enabled = true;
            TxtBoardSizeList.Enabled = false;
        }

        private void RadRandomSize_CheckedChanged(object sender, EventArgs e) {
            TxtBoardSize.Enabled = false;
            TxtBoardSizeList.Enabled = true;
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            int val;
            if (RadMultiGames.Checked && (!int.TryParse(TxtNumGames.Text, out val) || val < 1)) {
                MessageBox.Show("試合数に正しい数値を記入してください．");
                return;
            }
            if (RadFixedSize.Checked && (!int.TryParse(TxtBoardSize.Text, out val) || val < 5)) {
                MessageBox.Show("ボードサイズに5以上の数値を記入してください．");
                return;
            }
            if (RadRandomSize.Checked) {
                var bslist = TxtBoardSizeList.Text.Split(',');
                if (bslist.Any(c => !int.TryParse(c, out val) || val < 5)) {
                    MessageBox.Show("ランダムボードサイズは5以上の数字をカンマ区切りで記入してください．");
                    return;
                }
            }

            NumGames = RadOneGame.Checked ? 1 : int.Parse(TxtNumGames.Text);
            if (RadFixedSize.Checked) BoardSizeList =  new List<int>() { int.Parse(TxtBoardSize.Text) };
            else BoardSizeList = TxtBoardSizeList.Text.Split(',').Select(c => int.Parse(c)).ToList();
            ShuffleOrder = ChkShuffleOrder.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
