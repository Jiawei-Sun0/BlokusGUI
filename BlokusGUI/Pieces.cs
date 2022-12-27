using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlokusGUI {

    /// <summary>
    /// 位置情報
    /// </summary>
    struct Pos {
        public int X { get; set; }
        public int Y { get; set; }
        public Pos(int x, int y) {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// ピースクラス
    /// シングルトンパターンを適用
    /// </summary>
    class Pieces {
        private static Pieces _instace = new Pieces();    // 唯一のインスタンス

        private List<List<Pos>> _pieces = new List<List<Pos>>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Pieces() {
            // ピースの形を定義
            _pieces.Add(new List<Pos>() { new Pos(0, 0) });
            _pieces.Add(new List<Pos>() { new Pos(0, 0), new Pos(0, 1) });
            _pieces.Add(new List<Pos>() { new Pos(-1, 0), new Pos(0, 0), new Pos(1, 0) });
            _pieces.Add(new List<Pos>() { new Pos(0, -1), new Pos(0, 0), new Pos(-1, 0) });
            _pieces.Add(new List<Pos>() { new Pos(0, 0), new Pos(1, 0), new Pos(0, 1), new Pos(1, 1) });
            _pieces.Add(new List<Pos>() { new Pos(0, -1), new Pos(0, 0), new Pos(1, 0), new Pos(2, 0) });
            _pieces.Add(new List<Pos>() { new Pos(-1, 0), new Pos(0, 0), new Pos(1, 0), new Pos(2, 0) });
            _pieces.Add(new List<Pos>() { new Pos(-1, 0), new Pos(0, 0), new Pos(0, 1), new Pos(1, 1) });
            _pieces.Add(new List<Pos>() { new Pos(-1, 0), new Pos(0, 0), new Pos(0, -1), new Pos(1, 0) });
            _pieces.Add(new List<Pos>() { new Pos(-2, 0), new Pos(-1, 0), new Pos(0, 0), new Pos(1, 0), new Pos(2, 0) });
            _pieces.Add(new List<Pos>() { new Pos(-2, 0), new Pos(-1, 0), new Pos(0, 0), new Pos(0, -1), new Pos(0, 1) });
            _pieces.Add(new List<Pos>() { new Pos(-1, -1), new Pos(-1, 0), new Pos(0, 0), new Pos(1, 0), new Pos(0, -1) });
            _pieces.Add(new List<Pos>() { new Pos(0, -1), new Pos(-1, 0), new Pos(0, 0), new Pos(1, 0), new Pos(0, 1) });
            _pieces.Add(new List<Pos>() { new Pos(-1, -1), new Pos(0, -1), new Pos(0, 0), new Pos(0, 1), new Pos(1, 1) });
            _pieces.Add(new List<Pos>() { new Pos(0, -1), new Pos(-1, 0), new Pos(0, 0), new Pos(1, 0), new Pos(2, 0) });
            _pieces.Add(new List<Pos>() { new Pos(-1, 1), new Pos(0, 1), new Pos(0, 0), new Pos(1, 0), new Pos(1, -1) });
            _pieces.Add(new List<Pos>() { new Pos(-1, -1), new Pos(0, 0), new Pos(-1, 1) });
            _pieces.Add(new List<Pos>() { new Pos(-1, 1), new Pos(0, 0), new Pos(1, -1) });
            _pieces.Add(new List<Pos>() { new Pos(0, -1), new Pos(0, 0), new Pos(1, 1), new Pos(2, 1) });
            _pieces.Add(new List<Pos>() { new Pos(-1, -1), new Pos(0, 0), new Pos(1, -1), new Pos(2, 0) });
        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Pieces GetInstance() {
            return _instace;
        }

        /// <summary>
        /// ピースの個数
        /// </summary>
        /// <returns></returns>
        public int NumPieces() {
            return _pieces.Count();
        }

        /// <summary>
        /// ピース形状取得（位置リスト）
        /// </summary>
        /// <param name="piece">ピース番号</param>
        /// <returns></returns>
        public List<Pos> GetShape(int piece) {
            return _pieces[piece];
        }

        /// <summary>
        /// ピース形状取得（文字列）
        /// </summary>
        /// <param name="piece">ピース番号</param>
        /// <returns></returns>
        public string GetShapeString(int piece) {
            var grid = new bool[4, 5];
            var minX = _pieces[piece].Min(c => c.X);
            var minY = _pieces[piece].Min(c => c.Y);
            var maxX = 0;
            var maxY = 0;
            _pieces[piece].ForEach(p => {
                grid[p.Y - minY, p.X - minX] = true;
                if (p.X - minX > maxX) maxX = p.X - minX;
                if (p.Y - minY > maxY) maxY = p.Y - minY;
            });
            var shape = "";
            for (int y = 0; y <= maxY; y++) {
                for (int x = 0; x <= maxX; x++) {
                    shape += grid[y, x] ? "■" : " 　";
                }
                shape += "\n";
            }
            return shape;
        }
        public void RotatePiece(int piece, int type) {
            for (int i = 0; i < _pieces[piece].Count(); i++)
            {
                var p = _pieces[piece][i];
                if (type == 1)
                {
                    var term = p.X;
                    p.X = -p.Y;
                    p.Y = term;
                }
                if (type == 0)
                {
                    var term = p.X;
                    p.X = p.Y;
                    p.Y = -term;
                }
                _pieces[piece][i] = p;
            }
            //_pieces[piece].ForEach(p => Debug.WriteLine($"fact:{p.X},{p.Y}"));

            return;
        }
        public void ResetRotation(int piece) {

        }

    }
}
