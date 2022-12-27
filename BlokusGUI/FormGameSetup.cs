using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlokusGUI {
    public partial class FormGameSetup : Form {

        public int NumPlayers { get; private set; }
        public int BoardSize { get; private set; }

        private Button surrenderButton;


        public FormGameSetup() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            NumPlayers = decimal.ToInt32(UpdownPlayers.Value);
            BoardSize = decimal.ToInt32(UpdownBoardSize.Value);
            this.Close();
        }
    }
}
