using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using Python.Runtime;
using System.IO;
using System.Diagnostics;

namespace BlokusMod
{
    class Cpu {
        private static Cpu _instace = new Cpu();      // 唯一のインスタンス
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        private Board _board = Board.GetInstance();     // ボードのインスタンス
        private Client _client = Client.GetInstance();  // クライアントのインスタンス
        private SetInfo si = new SetInfo(0, 0, 0);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Cpu() {
            Debug.WriteLine("cpu init.");
            //myCPU();
        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Cpu GetInstance() {
            return _instace;
        }

        /// <summary>
        /// ピースを置く
        /// </summary>
        public void Turn() {
            if (!_client.IsMyChoice) return;

            var pieceList = new int[] { 15, 14, 13, 12, 11, 10, 9, 19, 18, 8, 7, 6, 5, 4, 17, 16, 3, 2, 1, 0 };
            var rotateList = Shuffle(Enumerable.Range(0, 8).ToArray());

            foreach (var piece in pieceList) {
                if (_game.Players[_game.TurnPlayer].PiecesUsed[piece]) continue;
                for (var x = 0; x < _board.BoardSize; x++) {
                    for (var y = 0; y < _board.BoardSize; y++) {
                        var pos = new Point(x, y);
                        foreach (var r in rotateList) {
                            si = new SetInfo(piece, r, pos);
                            if (_board.CheckPlace(_game.Turn, si)) {
                                _board.SetPiece(_game.Turn, si);
                                _client.IsMyChoice = false;
                                _game.SetPiece(si);
                                _client.SetPiece(si);
                                return;
                            }
                        }
                    }
                }
            }
            _client.GiveUp();
        }

        public void myCPU()
        {
            var PYTHON_HOME = Environment.ExpandEnvironmentVariables(@"C:\Users\sunjiawei\AppData\Local\Programs\Python\Python38");
            AddEnvPath(
              PYTHON_HOME,
              Path.Combine(@"C:\Users\sunjiawei\source\repos\BlokusGUI\BlokusGUI")
            );

            PythonEngine.PythonHome = PYTHON_HOME;
            //Console.WriteLine($"西巴老马：{PythonEngine.PythonPath}");
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(@"C:\Users\sunjiawei\source\repos\BlokusGUI\BlokusGUI");
                dynamic cpu = Py.Import("cpuAI");
                Console.WriteLine(cpu.check(_board,si));
            }
        }
        public static void AddEnvPath(params string[] paths)
        {
            var envPaths = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator).ToList();
            foreach (var path in paths)
            {
                if (path.Length > 0 && !envPaths.Contains(path))
                {
                    envPaths.Insert(0, path);
                }
            }
            Environment.SetEnvironmentVariable("PATH", string.Join(Path.PathSeparator.ToString(), envPaths), EnvironmentVariableTarget.Process);
        }

        public string test()
        {
            return "i am stirng from c#......";
        }
        /// <summary>
        /// 配列をシャッフル
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private int[] Shuffle(int[] list) {
            var rnd = new Random();
            for(var i=0; i<list.Length; i++) {
                var r = rnd.Next(list.Length);
                (list[i], list[r]) = (list[r], list[i]);
            }
            return list;
        }
    }
}
