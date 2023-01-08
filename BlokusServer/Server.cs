using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace BlokusMod {

    public enum States { Lobby, Playing, Gameover }

    /// <summary>
    /// サーバークラス
    /// シングルトンパターン
    /// </summary>
    public class Server {

        const int MAX_CONNECTION = 8;           // 最大クライアント（プレイヤー）数
        const int TCP_PORT = 1001;              // ポート番号
        public const int SEND_INTERVAL = 100;   // 連続送信の間隔(msec)
        private static Server _instace = new Server();      // 唯一のインスタンス
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        private ServerForm _serverForm;                   // フォームのインスタンス
        private Thread _listenerThread; // 接続待ち受けスレッド
        private TcpListener _listener;  // 接続待ち受けクラス
        public List<ClientHandler> Clients { get; private set; } // 接続クライアントリスト
        public States State { get; private set; } = States.Lobby;
        public ClientHandler TurnPlayer { get { return Clients[_game.TurnPlayer]; } }
        public int MessageDulation { get { return AutoPlayLast > 0 ? 0 : 1000; } }     // 不正操作やギブアップのメッセージ待機時間(msec)
        private List<IntPtr> _winIDs;               // 勝者のIDリスト
        public string ServerIP { get; private set; }    // サーバーのIPアドレス
        public int AutoPlayLast { get; private set; } = 0;
        private PlaySetting playSetting = new PlaySetting();
        private ScoreBoard _scoreBoard = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Server() {
            Clients = new List<ClientHandler>();
            // サーバーIP取得
            var host = Dns.GetHostEntry(Dns.GetHostName());
            ServerIP = host.AddressList.Where(p => p.AddressFamily == AddressFamily.InterNetwork).Select(p => p.ToString()).First();
        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Server GetInstance() {
            return _instace;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="serverForm"></param>
        public void Init(ServerForm serverForm) {
            _serverForm = serverForm;
        }

        /// <summary>
        /// ゲーム開始
        /// </summary>
        /// <param name="boardSize"></param>
        public void StartGame() {

            if (AutoPlayLast < 1) {
                // 連続試合の初期化
                if (playSetting.ShowDialog() == DialogResult.Cancel) return;
                AutoPlayLast = playSetting.NumGames;
                Clients.ForEach(c => c.WinCount = 0);
                if (AutoPlayLast > 1) {
                    var names = Clients.Select(c => c.Name).ToList();
                    if (_scoreBoard != null) {
                        _scoreBoard.Close();
                        _scoreBoard = null;
                    }
                    _scoreBoard = new ScoreBoard(names, AutoPlayLast);
                    _scoreBoard.Show();
                }
            }

            var rand = new Random();
            var boardSize = playSetting.BoardSizeList[rand.Next(playSetting.BoardSizeList.Count)];
            _game.Initialize(Clients.Count, null, null, playSetting.ShuffleOrder);
            _board.Initialize(boardSize);
            State = States.Playing;
            var orderedClients = _game.PlayOrder.Select(p => Clients[p]);
            var message = $"{boardSize},{Clients.Count}," + 
                string.Join(",", orderedClients.Select(c => $"{c.ClientID},{c.Name}"));
            this.SendToAll("start:" + message);
            this.SendTurn();
            _serverForm.UpdateForm(true);
        }

        /// <summary>
        /// ロビーへ戻る
        /// </summary>
        public void MoveToLobbby() {
            Clients.RemoveAll(c => !c.IsConnect);   // 切断したプレイヤー削除
            this.SendToAll($"lobby:");
            _board.Initialize(1);
            _game.Initialize(0);
            State = States.Lobby;
            _serverForm.UpdateForm(true);
        }

        /// <summary>
        /// 盤画像の取得
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBoardImage() {
            return _board.DrawBoard(-1, new PointF(0, 0), 0);
        }

        /// <summary>
        /// ピースを置く
        /// </summary>
        /// <param name="places"></param>
        public bool CheckAndSetPiece(SetInfo si) {
            if (!_board.CheckPlace(_game.Turn, si)) return false;
            _board.SetPiece(_game.Turn, si);

            this.SendToAll($"set:{TurnPlayer.ClientID},{si.Piece},{si.Rotate},{si.Pos}");
            _game.SwitchPlayer();
            this.SendTurn();
            _serverForm.UpdateForm();
            return true;
        }

        /// <summary>
        /// 不正操作（順番飛ばす）
        /// </summary>
        public void Miss() {
            this.SendToAll($"miss:{TurnPlayer.ClientID}");
            Message($"{TurnPlayer.Name}の不正操作");
            Thread.Sleep(MessageDulation);
            _game.SwitchPlayer();
            this.SendTurn();
            _serverForm.UpdateForm();
        }

        /// <summary>
        /// リタイヤ
        /// </summary>
        public void GiveUp() {
            this.SendToAll($"giveup:{TurnPlayer.ClientID}");
            Thread.Sleep(MessageDulation);
            _game.GiveUp();
            if (_game.AlivePlayers == 0) {
                // ゲーム終了
                State = States.Gameover;
                AutoPlayLast--;

                // 各プレイヤーのマスをカウント
                for (int turnid = 0; turnid < _game.NumPlayers; turnid++) {
                    int count = 0;
                    for (int y = 0; y < _board.BoardSize; y++) {
                        for (int x = 0; x < _board.BoardSize; x++) {
                            if (_board.Cell[y, x] == turnid) ++count;
                        }
                    }
                    Clients[_game.PlayOrder[turnid]].CellCount = count;
                }
                var msg = "結果：" + string.Join("，", Clients.Select(c => $"{c.Name}:{c.CellCount}個"));
                _serverForm.Message(msg);
                // 勝者判定
                var maxCount = Clients.Max(c => c.CellCount);
                var winner = Clients.Where(c => c.CellCount == maxCount);
                _winIDs = winner.Select(c => c.ClientID).ToList();
                winner.ToList().ForEach(c => c.WinCount++);
                if (_scoreBoard != null) {
                    _scoreBoard.ShowScore(Clients.Select(c => c.WinCount).ToList(), AutoPlayLast);
                }

                this.SendToAll($"gameover:{string.Join(",", _winIDs)}");
            } else {
                this.SendTurn();
            }
            _serverForm.UpdateForm();
        }

        /// <summary>
        /// ターン情報を送信
        /// </summary>
        private void SendTurn() {
            this.SendToAll($"turn:{TurnPlayer.ClientID}");
        }

        /// <summary>
        /// 全プレイヤーにメッセージ送信
        /// </summary>
        /// <param name="msg"></param>
        private void SendToAll(string msg) {
            Thread.Sleep(50);
            _serverForm.Message("一斉送信:" + msg);
            Clients.ForEach(c => c.SendData(msg));
        }

        /// <summary>
        /// サーバー開始
        /// </summary>
        /// <returns></returns>
        public bool StartServer() {
            try {
                // 接続待ち受けスレッド開始
                this.Message("サーバー開始");
                _listenerThread = new Thread(new ThreadStart(WaitConnection));
                _listenerThread.Start();
                return true;
            } catch (Exception ex) {
                // エラー
                this.Message($"サーバー開始エラー：{ex.ToString()}");
                _listener.Stop();
                return false;
            }
        }

        /// <summary>
        /// クライアント接続待ち
        /// </summary>
        private void WaitConnection() {
            _listener = new TcpListener(IPAddress.Any, TCP_PORT);
            _listener.Start();

            try {
                while (true) {
                    var clientSocket = _listener.AcceptSocket();
                    if (State == States.Lobby && Clients.Count < MAX_CONNECTION) {
                        var client = new ClientHandler(clientSocket);
                        Clients.Add(client);
                        client.StartRead();
                        client.SendData($"id:{client.ClientID}");
                        this.Message($"{client.ClientID} が接続しました．");
                        _serverForm.UpdateForm();
                    } else {
                        clientSocket.Close();
                        Thread.Sleep(20);
                        clientSocket = null;
                    }
                }
            } catch (System.Threading.ThreadAbortException) {
                this.Message($"例外：System.Threading.ThreadAbortException");
                return;
            } catch (System.Net.Sockets.SocketException) {
                //this.Message($"例外：System.Net.Sockets.SocketException");
                return;
            } catch (Exception ex) {
                this.Message($"受信エラー：{ex.ToString()}");
                return;
            }
        }

        /// <summary>
        /// クライアント切断
        /// </summary>
        /// <param name="client"></param>
        public void DeleteClient(ClientHandler client) {
            if (State == States.Lobby) {
                Clients.Remove(client);
            } else {
                var player = Clients.IndexOf(client);
                if (player == _game.TurnPlayer) {
                    this.GiveUp();
                } else {
                    _game.Players[player].Alive = false;
                }
            }
            this.Message($"{client.ClientID} が切断しました．");
            _serverForm.UpdateForm();
        }

        /// <summary>
        /// サーバー停止
        /// </summary>
        public void StopServer() {
            Clients.ForEach(c => c.Close());

            if (_listener != null) {
                _listener.Stop();
                Thread.Sleep(20);
                _listener = null;
            }
            if (_listenerThread != null) {
                _listenerThread.Abort();
                _listenerThread = null;
            }
            this.Message($"サーバー停止");
        }

        /// <summary>
        /// メッセージ表示
        /// </summary>
        /// <param name="msg"></param>
        public void Message(string msg) {
            _serverForm.Message(msg);
        }

        /// <summary>
        /// フォーム更新
        /// </summary>
        public void UpdateForm(bool updateList = false) {
            _serverForm.UpdateForm(updateList);
        }

        /// <summary>
        /// 勝者の名前
        /// </summary>
        /// <returns></returns>
        public string WinnersName() {
            var names = Clients.Where(c => _winIDs.Contains(c.ClientID)).Select(c => c.Name);
            return string.Join("，", names);
        }
    }
}
