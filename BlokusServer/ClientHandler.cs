using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlokusMod {
    public class ClientHandler {
        private Server _server = Server.GetInstance();   // サーバーのインスタンス
        private Socket _socket = null;  // 接続ソケット
        private NetworkStream _networkStream;   // データ送受信ストリーム
        private byte[] _buffer;    //受信バッファ
        private DateTime _lastSend; // 前回データ送信時刻
        public string Name { get; private set; } // プレイヤー名
        public bool IsConnect { get { return _socket != null; } }   // 接続有無
        public int CellCount { get; set; }  // 置いたセル数
        public int WinCount { get; set; }   // 勝利数

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="socket">クライアント接続ソケット</param>
        /// <param name="server">サーバークラスのインスタンス</param>
        public ClientHandler(Socket socket) {
            _socket = socket;
            _networkStream = new NetworkStream(socket);
            _buffer = new byte[1024];   // 受信バッファサイズ1024byte
            Name = $"[{ClientID}]";
        }

        /// <summary>
        /// クライアント識別番号
        /// </summary>
        public IntPtr ClientID {
            get { return _socket?.Handle ?? (IntPtr)0; }
        }

        /// <summary>
        /// データ受信待ち開始
        /// </summary>
        public void StartRead() {
            _networkStream.BeginRead(_buffer, 0, _buffer.Length, 
                new AsyncCallback(this.OnReadComplete), null);
        }

        /// <summary>
        /// データ受信完了
        /// </summary>
        /// <param name="result"></param>
        private void OnReadComplete(IAsyncResult result) {
            
            try {
                if (_networkStream == null) return;

                //受信データ量取得
                var bytesRead = _networkStream.EndRead(result);
                if (bytesRead > 0) {

                    // データ受信
                    var bufStr = new byte[bytesRead];
                    Buffer.BlockCopy(_buffer, 0, bufStr, 0, bytesRead);
                    var msg = Encoding.UTF8.GetString(bufStr);
                    _server.Message($"受信[{ClientID.ToString()}]:{msg}");

                    // 名前の受信
                    if (msg.StartsWith("name:") && _server.State == States.Lobby) {
                        Name = msg.Substring(5);
                        _server.UpdateForm(true);
                    }
                    var turnID = _server.TurnPlayer.ClientID;
                    if (this.ClientID == turnID && _server.State == States.Playing) {
                        // ピースセット受信
                        if (msg.StartsWith("set:")) {
                            var valid = false;
                            var strValues = msg.Substring(4).Split(',');
                            if (strValues.All(c => int.TryParse(c, out _))) {
                                var piece = int.Parse(strValues[0]);
                                var rotate = int.Parse(strValues[1]);
                                var pos = int.Parse(strValues[2]);
                                if (_server.CheckAndSetPiece(new SetInfo(piece, rotate, pos))) valid = true;
                            }
                            if (!valid) {
                                _server.Miss();
                            }
                        }
                        // リタイヤ受信
                        if (msg.StartsWith("giveup:")) {
                            _server.GiveUp();
                        }
                    }

                    //次の受信を待つ
                    this.StartRead();
                } else {
                    // クライアント切断時
                    _server.DeleteClient(this);
                    this.Close();
                }

            } catch (System.Net.Sockets.SocketException) {
                //スレッドが閉じられた時に発生
                _server.Message($"例外：System.Net.Sockets.SocketException");
                return;
            } catch (System.IO.IOException) {
                //スレッドが閉じられた時に発生
                _server.Message($"例外：System.IO.IOException");
                _server.DeleteClient(this);
                this.Close();
                return;
            } catch (Exception ex) {
                _server.Message($"受信エラー：{ex.ToString()}");
            }
        }

        /// <summary>
        /// データ送信
        /// </summary>
        /// <param name="buffer"></param>
        public void SendData(string msg) {
            var buffer = Encoding.UTF8.GetBytes(msg);
            if (_networkStream == null) return;
            try {
                var interval = TimeSpan.FromMilliseconds(Server.SEND_INTERVAL);
                while (DateTime.Now - _lastSend < interval) Thread.Sleep(1);
                //Debug.WriteLine($"Send {Name}:{DateTime.Now:ss.fff}:{msg}");
                _networkStream.BeginWrite(buffer, 0, buffer.Length,
                    new AsyncCallback(this.OnSendComplete), null);
            } catch (Exception) {

            }
        }

        /// <summary>
        /// データ送信完了
        /// </summary>
        /// <param name="result"></param>
        private void OnSendComplete(IAsyncResult result) {
            _networkStream.EndWrite(result);
            _lastSend = DateTime.Now;
        }

        /// <summary>
        /// 切断
        /// </summary>
        public void Close() {
            if (_networkStream != null) {
                _networkStream.Close();
                _networkStream = null;
            }
            if (_socket != null) {
                _socket.Close();
                Thread.Sleep(20);//これを入れないとNullReferenceExceptionが起きる
                _socket = null;
            }
        }
    }
}
