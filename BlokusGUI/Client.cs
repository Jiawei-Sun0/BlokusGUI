using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BlokusMod
{

    public enum States { Unconnect, Prestart, Playing, Miss, Giveup, Gameover, Preplaying }

    /// <summary>
    /// クライアントクラス
    /// シングルトンパターン
    /// </summary>
    public class Client {
        const int TCP_PORT = 1001;
        private static Client _instace = new Client();      // 唯一のインスタンス
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        private MainForm _clientForm;                   // フォームのインスタンス
        private TcpClient _client = null;
        private Thread _clientThread = null;
        private int _myID = 0;
        private List<int> _winIDs;
        public States State { get; set; } = States.Unconnect;   // ゲームの遷移状態
        public bool IsMyTurn { get { return _myID == _game.Players[_game.TurnPlayer].ID; } }
        public bool IsMyChoice { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Client() {

        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Client GetInstance() {
            return _instace;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="clientForm"></param>
        public void Init(MainForm clientForm) {
            _clientForm = clientForm;
        }

        /// <summary>
        /// サーバーに接続
        /// </summary>
        /// <returns></returns>
        public bool StartClient(string serverIP, string name) {
            if (_client != null) return false;
            try {
                _client = new TcpClient(serverIP, TCP_PORT);
                _clientThread = new Thread(new ThreadStart(this.ReceiveData));
                _clientThread.Start();
                this.SendData($"name:{name}");
                this.Message($"サーバー接続");
                State = States.Prestart;
                return true;
            } catch (Exception ex) {
                this.Message($"サーバー接続エラー: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// データ受信スレッド
        /// </summary>
        private void ReceiveData() {
            var stream = _client.GetStream();
            var buffer = new Byte[1024];
            while (true) {
                try {
                    var receiveSize = stream.Read(buffer, 0, buffer.Length);
                    if (receiveSize > 0) {
                        var receiveData = new Byte[receiveSize];
                        Buffer.BlockCopy(buffer, 0, receiveData, 0, receiveSize);
                        var receiveStr = Encoding.UTF8.GetString(receiveData);
                        this.Message($"受信:{receiveStr}");
                        // 自ID取得
                        if (receiveStr.StartsWith("id:")) {
                            _myID = int.Parse(receiveStr.Substring(3));
                        }
                        // ゲーム開始
                        if (receiveStr.StartsWith("start:")) {
                            var gameData = receiveStr.Substring(6).Split(',');
                            var boardSize = int.Parse(gameData[0]);
                            var numPlayers = int.Parse(gameData[1]);
                            var ids = gameData.Where((c, idx) => (idx > 1 && idx % 2 == 0)).Select(c => int.Parse(c)).ToArray();
                            var names = gameData.Where((c, idx) => (idx > 1 && idx % 2 == 1)).Select(c => c).ToArray();
                            _game.Initialize(numPlayers, ids, names);
                            _board.Initialize(boardSize);
                            State = States.Preplaying;
                            _clientForm.UpdateForm(true);
                        }
                        // ロビー
                        if (receiveStr.StartsWith("lobby:")) {
                            State = States.Prestart;
                            _clientForm.UpdateForm();
                        }
                        // プレイヤー交代
                        if (receiveStr.StartsWith("turn:")) {
                            var id = int.Parse(receiveStr.Substring(5));
                            _game.SwitchPlayer(id);
                            State = States.Playing;
                            if (IsMyTurn) IsMyChoice = true;
                            _clientForm.UpdateForm();
                        }
                        // ピースセット
                        if (receiveStr.StartsWith("set:")) {
                            var values = receiveStr.Substring(4).Split(',');
                            _game.SwitchPlayer(int.Parse(values[0]));
                            var si = new SetInfo(int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]));
                            _board.SetPiece(_game.Turn, si);
                            _game.SetPiece(si);
                            _clientForm.UpdateForm();
                        }
                        // 不正操作
                        if (receiveStr.StartsWith("miss:")) {
                            State = States.Miss;
                            _clientForm.UpdateForm();
                        }
                        // ギブアップ
                        if (receiveStr.StartsWith("giveup:")) {
                            var id = int.Parse(receiveStr.Substring(7));
                            _game.Players.Where(p => p.ID == id).First().Alive = false;
                            State = States.Giveup;
                            _clientForm.UpdateForm();
                        }
                        // ゲーム終了
                        if (receiveStr.StartsWith("gameover:")) {
                            _winIDs = receiveStr.Substring(9).Split(',').Select(c => int.Parse(c)).ToList();
                            State = States.Gameover;
                            _clientForm.UpdateForm();
                        }
                    } else {
                        this.Message($"サーバー切断");
                    }

                } catch (System.Threading.ThreadAbortException) {
                    //this.Message($"例外：ThreadAbortException");
                    return;
                }catch(Exception ex) {
                    this.Message($"受信エラー：{ex.Message}");
                    this.StopClient();
                    return;
                }
            }
        }

        /// <summary>
        /// ピース設置
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="setcells"></param>
        public void SetPiece(SetInfo si) {
            var msg = $"set:{si.Piece},{si.Rotate},{si.Pos}";
            SendData(msg);
            if (IsMyTurn) IsMyChoice = false;
        }

        /// <summary>
        /// ギブアップ
        /// </summary>
        public void GiveUp() {
            SendData($"giveup:");
            if (IsMyTurn) IsMyChoice = false;
        }

        /// <summary>
        /// データ送信
        /// </summary>
        /// <param name="msg"></param>
        private void SendData(string msg) {
            try {
                var stream = _client.GetStream();
                var buffer = Encoding.UTF8.GetBytes(msg);
                stream.Write(buffer, 0, buffer.Length);
                this.Message($"送信:{msg}");
            } catch (Exception ex) {
                this.Message($"送信エラー：{ex.Message}");
            }
        }

        /// <summary>
        /// サーバー切断
        /// </summary>
        public void StopClient() {
            if (_client!=null && _client.Connected) {
                _client.Close();
            }
            if (_clientThread != null) {
                _clientThread.Abort();
            }
            this.Message("サーバー切断");
        }

        /// <summary>
        /// 勝者の名前
        /// </summary>
        /// <returns></returns>
        public string WinnersName() {
            var names = _game.Players.Where(c => _winIDs.Contains(c.ID)).Select(c => c.Name);
            return string.Join("，", names);
        }

        /// <summary>
        /// メッセージ表示
        /// </summary>
        /// <param name="msg"></param>
        public void Message(string msg) {
            _clientForm.Message(msg);
        }
    }
}
