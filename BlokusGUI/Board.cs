using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace BlokusGUI {
    /// <summary>
    /// ボード（盤）クラス
    /// シングルトンパターンを適用
    /// </summary>
    class Board {
        const int BOARD_IMG_SIZE = 600; // ボード画像サイズ
        public Brush[] PieceColors = {
            Brushes.DodgerBlue, Brushes.OrangeRed, Brushes.ForestGreen, Brushes.DarkViolet,
            Brushes.Peru, Brushes.Gold, Brushes.Turquoise, Brushes.HotPink};    // プレイヤーの色

        private static Board _instance = new Board();    // 唯一のインスタンス

        public int BoardSize { get; private set; } = 0; // ボードのマス数
        public int[,] Cell { get; private set; }        // [y,x]位置のマスの状態 -1:無　0-7:プレイヤーマス
        public List<Pos> Startpoint { get; private set; }
        private float _cellSize = 0;    // 描画マスサイズ
        private float _margin = 0;      // ボード周囲の幅

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Board() {
        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Board GetInstance() {
            return _instance;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="boardSize">ボードのマス数</param>
        public void Initialize(int boardSize) {
            BoardSize = boardSize;
            Cell = new int[BoardSize, BoardSize];
            Startpoint = new List<Pos>
            {
                new Pos(0, 0),
                new Pos(BoardSize - 1, BoardSize - 1),
                new Pos(BoardSize - 1, 0),
                new Pos(0, BoardSize - 1),
                new Pos((BoardSize - 1) / 2, 0),
                new Pos((BoardSize - 1) / 2, BoardSize - 1),
                new Pos(0, (BoardSize - 1) / 2),
                new Pos(BoardSize - 1, (BoardSize - 1) / 2)
            };
            for (var y=0; y< BoardSize; y++) {
                for (var x = 0; x < BoardSize; x++) {
                    Cell[y, x] = -1;
                }
            }
            _cellSize = (float)BOARD_IMG_SIZE / (BoardSize + 1);
            _margin = _cellSize / 2;
        }

        /// <summary>
        /// ボードを描画する
        /// </summary>
        /// <param name="hold">選択中のピース番号（非選択:<0）</param>
        /// <param name="pos">マウスの位置（0～1）</param>
        /// <returns></returns>
        public Bitmap DrawBoard(int hold, PointF pos, int player, bool first, int rotate) {
            var bmp = new Bitmap(BOARD_IMG_SIZE, BOARD_IMG_SIZE);
            var g = Graphics.FromImage(bmp);
            // 背景と枠の描画
            g.FillRectangle(Brushes.AntiqueWhite, 0, 0, bmp.Width, bmp.Height);
            var gridPen = new Pen(Color.DarkBlue, 2);
            var padding = 3;
            for (var i = 0; i < BoardSize+1; i++) {
                g.DrawLine(gridPen, _margin + i * _cellSize, _margin, _margin + i * _cellSize, BOARD_IMG_SIZE - _margin);
                g.DrawLine(gridPen, _margin, _margin + i * _cellSize, BOARD_IMG_SIZE - _margin, _margin + i * _cellSize);
            }
            // マスの描画
            for (var y = 0; y < BoardSize; y++)
            {
                for (var x = 0; x < BoardSize; x++)
                {
                    if (Cell[y, x] >= 0)
                    {
                        g.FillRectangle(PieceColors[Cell[y, x]],
                            _margin + padding + x * _cellSize, _margin + padding + y * _cellSize,
                            _cellSize - padding * 2, _cellSize - padding * 2);
                    }
                }
            }
            // 操作ピースの描画
            var pieces = Pieces.GetInstance();
            if (hold >= 0 && hold < pieces.NumPieces()) {
                var x = (int)((pos.X * BOARD_IMG_SIZE - _margin) / _cellSize);
                var y = (int)((pos.Y * BOARD_IMG_SIZE - _margin) / _cellSize);
                var shape = pieces.GetShape(hold);
                //shape.ForEach(p => Debug.WriteLine($"POINT:{p.Y},{p.X}"));
                if (first)
                {
                    var px = Startpoint[player].X;
                    var py = Startpoint[player].Y;
                    if (px >= 0 && py >= 0 && px < BoardSize && py < BoardSize)
                    {
                        g.FillRectangle(Brushes.Yellow,
                            _margin + padding + px * _cellSize, _margin + padding + py * _cellSize,
                            _cellSize - padding * 2, _cellSize - padding * 2);
                    }
                }
                shape.ForEach(p => {
                    var px = x + p.X;
                    var py = y + p.Y;
                    //Debug.WriteLine($"key:{py},{px}");
                    if (px >= 0 && py >= 0 && px < BoardSize && py < BoardSize) {
                        g.FillRectangle(Brushes.Gray,
                            _margin + padding + px * _cellSize, _margin + padding + py * _cellSize,
                            _cellSize - padding * 2, _cellSize - padding * 2);
                    }
                });
            }

            return bmp;
        }

        /// <summary>
        /// ピースを置く
        /// </summary>
        /// <param name="player">プレイヤー番号(0-7)</param>
        /// <param name="hold">選択ピース番号</param>
        /// <param name="pos">マウス位置</param>
        /// <returns>true: 置いた false: 置けなかった</returns>
        public bool SetPiece(int player, int hold, PointF pos, bool first, int rotate) {
            var pieces = Pieces.GetInstance();
            if (hold < 0 || hold >= pieces.NumPieces()) return false;
            var x = (int)((pos.X * BOARD_IMG_SIZE - _margin) / _cellSize);
            var y = (int)((pos.Y * BOARD_IMG_SIZE - _margin) / _cellSize);
            var shape = pieces.GetShape(hold);
            
            // ピースが盤外でないかチェック
            if (shape.Any(p => p.X + x < 0 || p.Y + y < 0 || p.X + x >= BoardSize || p.Y + y >= BoardSize)) return false;
            // ピースの重なりがないかチェック
            if (shape.Any(p => Cell[p.Y + y, p.X + x] >= 0)) return false;
            if (shape.Any(p => Cell[p.Y + y, p.X + x + 1 >= BoardSize? BoardSize - 1: p.X + x + 1] == player
                               || Cell[p.Y + y + 1 >= BoardSize ? BoardSize - 1 : p.Y + y + 1, p.X + x] == player
                               || Cell[p.Y + y - 1 < 0 ? 0 : p.Y + y - 1, p.X + x] == player
                               || Cell[p.Y + y, p.X + x - 1 < 0 ? 0 : p.X + x - 1] == player)) return false;
            if (first)
            {
                if (!shape.Any(p => p.Y + y == Startpoint[player].Y && p.X + x == Startpoint[player].X)) return false;
                shape.ForEach(v => { Cell[v.Y + y, v.X + x] = player; });
                return true;
                
            }
            else
            {
                for (int i = -1; i < 2; i += 2)
                {
                    for (int j = -1; j < 2; j += 2)
                    {
                        foreach (var p in shape)
                        {
                            int py = p.Y + y + i;
                            int px = p.X + x + j;
                            if (px >= BoardSize)
                                px = BoardSize - 1;
                            if (py >= BoardSize)
                                py = BoardSize - 1;
                            if (px < 0)
                                px = 0;
                            if (py < 0)
                                py = 0;
                            if (Cell[py, px] == player)
                            {
                                shape.ForEach(v => { Cell[v.Y + y, v.X + x] = player; });
                                return true;
                            }
                        }
                    }
                }
            }
            // ピースを置く
            return false;
        }
    }
}
