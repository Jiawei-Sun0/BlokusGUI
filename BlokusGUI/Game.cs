﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BlokusMod
{

    /// <summary>
    /// プレイヤークラス
    /// </summary>
    public class Player {
        public bool Alive { get; set; } = true;     // true: プレイ中  false: 降参
        public List<bool> PiecesUsed { get; set; }  // true: 使用済みピース  false: 未使用
        public int ID { get; private set; }
        public string Name { get; private set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Player(int id, string name) {
            ID = id;
            Name = name;
            PiecesUsed = new List<bool>(new bool[Board.GetInstance().Pieces.Count()]);
        }
    }

    /// <summary>
    /// ゲームクラス
    /// シングルトンパターンを適用
    /// </summary>
    public class Game {
        public WMPLib.WindowsMediaPlayer bgm = new WMPLib.WindowsMediaPlayer();
        //private Board _board = Board.GetInstance();     // ボードのインスタンス

        private static Game _instace = new Game();      // 唯一のインスタンス

        public readonly int MAX_PLAYERS = 8;               // 最大プレイヤー数
        public int NumPlayers { get; private set; }     // プレイヤー数
        public List<Player> Players { get; private set; }       // プレイヤー情報
        public int Turn { get; private set; } = 0;      // ターン
        public int TurnPlayer { get { return PlayOrder == null ? Turn : PlayOrder[Turn]; } }     // 現在のプレイヤー
        public int[] PlayOrder { get; private set; }    // 順番

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Game()
        {
        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Game GetInstance()
        {
            return _instace;
        }

        /// <summary>
        /// ゲーム初期化
        /// </summary>
        /// <param name="numPlayers">プレイヤー数</param>
        public void Initialize(int numPlayers, int[] ids = null, string[] names = null, bool shuffle = false)
        {
            NumPlayers = numPlayers;
            Players = new List<Player>();
            for (var i = 0; i < NumPlayers; i++)
            {
                Players.Add(new Player(ids?[i] ?? 0, names?[i] ?? ""));
            }
            if (numPlayers > 0)
            {
                PlayOrder = Enumerable.Range(0, NumPlayers).ToArray();
                if (shuffle)
                {
                    PlayOrder = this.Shuffle(PlayOrder);
                    // デバッグ用に逆順設定
                    // for(int i = 0; i < numPlayers; i++) { PlayOrder[i] = numPlayers - i - 1; }
                }
                Turn = 0;
            }
            else
            {
                Turn = 0;
            }
            bgm.settings.volume = 10;
            bgm.URL = System.Configuration.ConfigurationManager.AppSettings["BGM_PATH"];
        }

        /// <summary>
        /// プレイ中の人数
        /// </summary>
        public int AlivePlayers
        {
            get
            {
                return Players.Count(c => c.Alive == true);
            }
        }

        /// <summary>
        /// ピースを置く
        /// </summary>
        /// <param name="piece">ピース番号</param>
        public void SetPiece(SetInfo si)
        {
            Players[TurnPlayer].PiecesUsed[si.Piece] = true;
            this.SwitchPlayer();
        }

        /// <summary>
        /// 降参
        /// </summary>
        public void GiveUp()
        {
            Players[TurnPlayer].Alive = false;
            this.SwitchPlayer();
        }

        /// <summary>
        /// プレイヤー交代
        /// </summary>
        public void SwitchPlayer()
        {
            if (AlivePlayers > 0)
            {
                do
                {
                    Turn = (Turn + 1) % NumPlayers;
                } while (Players[TurnPlayer].Alive == false);
            }
            else
            {
                this.GameOver();
            }
        }

        /// <summary>
        /// ID指定プレイヤー交代
        /// </summary>
        /// <param name="id"></param>
        public void SwitchPlayer(int id)
        {
            var player = Players.FindIndex(c => c.ID == id);
            Turn = PlayOrder.ToList().FindIndex(c => c == player);
        }

        /// <summary>
        /// 配列をシャッフル
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private int[] Shuffle(int[] list)
        {
            var rnd = new Random();
            for (var i = 0; i < list.Length; i++)
            {
                var r = rnd.Next(list.Length);
                (list[i], list[r]) = (list[r], list[i]);
            }
            return list;
        }
        private void GameOver()
        {
            bgm.URL = "";
        }
    }
}
