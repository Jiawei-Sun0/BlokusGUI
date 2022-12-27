using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BlokusGUI {

    /// <summary>
    /// プレイヤークラス
    /// </summary>
    class Player {
        public bool Alive { get; set; } = true;     // true: プレイ中  false: 降参
        public List<bool> PiecesUsed { get; set; }  // true: 使用済みピース  false: 未使用
        public bool first { get; set; } = true;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Player() {
            PiecesUsed = new List<bool>(new bool[Pieces.GetInstance().NumPieces()]);
        }
    }

    /// <summary>
    /// ゲームクラス
    /// シングルトンパターンを適用
    /// </summary>
    class Game {
        private static Game _instace = new Game();      // 唯一のインスタンス

        public int NumPlayers { get; private set; }     // プレイヤー数
        public List<Player> Players { get; private set; }       // プレイヤー情報
        public int TurnPlayer { get; private set; }     // 現在のプレイヤー
        public int AlivePlayers { get; private set; }   // プレイ中の人数

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Game() {
        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Game GetInstance() {
            return _instace;
        }

        /// <summary>
        /// ゲーム初期化
        /// </summary>
        /// <param name="numPlayers">プレイヤー数</param>
        public void Initialize(int numPlayers) {
            NumPlayers = numPlayers;
            Players = new List<Player>();
            for (var i = 0; i < NumPlayers; i++) {
                Players.Add(new Player());
            }
            TurnPlayer = 0;
            AlivePlayers = NumPlayers;
        }

        /// <summary>
        /// ピースを置く
        /// </summary>
        /// <param name="piece">ピース番号</param>
        public void SetPiece(int piece) {
            Players[TurnPlayer].PiecesUsed[piece] = true;
            this.SwitchPlayer();
        }

        /// <summary>
        /// 降参
        /// </summary>
        public void GiveUp() {
            Players[TurnPlayer].Alive = false;
            if (--AlivePlayers > 0) {
                this.SwitchPlayer();
            } else {
                this.GameOver();
            }
        }

        /// <summary>
        /// ゲーム終了
        /// </summary>
        private void GameOver() {

        }

        /// <summary>
        /// 次のプレイヤーへ
        /// </summary>
        private void SwitchPlayer() {
            do {
                TurnPlayer = (TurnPlayer + 1) % NumPlayers;
            } while (Players[TurnPlayer].Alive == false);
        }
    }
}
