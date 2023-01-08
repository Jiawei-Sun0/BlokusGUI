using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace BlokusMod
{

    /// <summary>
    /// 駒配置情報クラス
    /// </summary>
    public class SetInfo
    {
        private readonly int BOARD_SIZE = Board.GetInstance().BoardSize;
        public int Piece { get; private set; }
        public int Rotate { get; private set; }
        public Point Cell { get; private set; }
        public int Pos { get { return Cell.Y * BOARD_SIZE + Cell.X; } }
        public SetInfo(int piece, int rotate, Point cell)
        {
            Piece = piece;
            Rotate = rotate;
            Cell = cell;
        }
        public SetInfo(int piece, int rotate, int pos)
        {
            Piece = piece;
            Rotate = rotate;
            Cell = new Point(pos % BOARD_SIZE, pos / BOARD_SIZE);
        }
    }

    /// <summary>
    /// ボード（盤）クラス
    /// シングルトンパターンを適用
    /// </summary>
    class Board
    {
        private const int BOARD_IMG_SIZE = 600; // ボード画像サイズ
        private const int MARGIN = 5;      // ボード周囲の幅
        private const int CELL_NONE = -1;   // 駒のないセル値
        public Color[] PieceColors = {
            Color.DodgerBlue, Color.OrangeRed, Color.ForestGreen, Color.DarkViolet,
            Color.Peru, Color.Gold, Color.Turquoise, Color.HotPink};    // プレイヤーの色
        public Brush[] PieceBrushes = {
            Brushes.DodgerBlue, Brushes.OrangeRed, Brushes.ForestGreen, Brushes.DarkViolet,
            Brushes.Peru, Brushes.Gold, Brushes.Turquoise, Brushes.HotPink};
        private static Board _instace = new Board();    // 唯一のインスタンス
        private Game _game = Game.GetInstance();        // ゲームのインスタンス
        public List<Piece> Pieces { get; private set; } = new List<Piece>();    // ピース定義
        private List<Point> _startPoint = new List<Point>();    // プレイヤーの開始マス
        private float _cellSize = 0;    // 描画マスサイズ
        public List<int> Scores { get; set; } = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        public List<int> Records { get; set; }
        public int BoardSize { get; private set; } = 0; // ボードのマス数
        public int[,] Cell { get; private set; }        // [y,x]位置のマスの状態 -1:無　0-7:プレイヤーマス

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private Board()
        {
            // ピースの定義
            Pieces.Add(new Piece(new List<Point>() { new Point(0, 0) }));   // 0
            Pieces.Add(new Piece(new List<Point>() { new Point(0, 0), new Point(0, 1) }));  // 1
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, 0), new Point(1, 0) }));    // 2
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, 0), new Point(0, -1) }));   // 3
            Pieces.Add(new Piece(new List<Point>() { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) }));    // 4
            Pieces.Add(new Piece(new List<Point>() { new Point(0, 0), new Point(0, -1), new Point(1, 0), new Point(2, 0) }));   // 5
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, 0), new Point(1, 0), new Point(2, 0) }));   // 6
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, 0), new Point(0, 1), new Point(1, 1) }));   // 7
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, 0), new Point(1, 0), new Point(0, -1) }));  // 8
            Pieces.Add(new Piece(new List<Point>() { new Point(-2, 0), new Point(-1, 0), new Point(0, 0), new Point(1, 0), new Point(2, 0) }));     // 9
            Pieces.Add(new Piece(new List<Point>() { new Point(-2, 0), new Point(-1, 0), new Point(0, 0), new Point(0, -1), new Point(0, 1) }));    // 10
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, 0), new Point(1, 0), new Point(-1, -1), new Point(0, -1) }));   // 11
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) }));     // 12
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, -1), new Point(0, -1), new Point(0, 0), new Point(0, 1), new Point(1, 1) }));    // 13
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 0), new Point(0, -1), new Point(0, 0), new Point(1, 0), new Point(2, 0) }));     // 14
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 1), new Point(0, 1), new Point(0, 0), new Point(1, 0), new Point(1, -1) }));     // 15
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, -1), new Point(0, 0), new Point(-1, 1) }));  // 16
            Pieces.Add(new Piece(new List<Point>() { new Point(-1, 1), new Point(0, 0), new Point(1, -1) }));   // 17
            Pieces.Add(new Piece(new List<Point>() { new Point(0, -1), new Point(0, 0), new Point(1, 1), new Point(2, 1) }));   // 18
            Pieces.Add(new Piece(new List<Point>() { new Point(1, -1), new Point(0, 0), new Point(1, 1), new Point(0, 2) }));   // 19
        }

        /// <summary>
        /// 唯一のインスタンス取得
        /// </summary>
        /// <returns>インスタンス</returns>
        public static Board GetInstance()
        {
            return _instace;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="n">ボードのマス数</param>
        public void Initialize(int n)
        {
            BoardSize = n;
            Cell = new int[BoardSize, BoardSize];
            for (var y = 0; y < BoardSize; y++)
            {
                for (var x = 0; x < BoardSize; x++)
                {
                    Cell[y, x] = CELL_NONE;
                }
            }
            _cellSize = (float)(BOARD_IMG_SIZE - MARGIN * 2) / BoardSize;
            _startPoint.Clear();
            var middle = (n - 1) / 2;
            _startPoint.Add(new Point(0, 0));
            _startPoint.Add(new Point(n - 1, n - 1));
            _startPoint.Add(new Point(n - 1, 0));
            _startPoint.Add(new Point(0, n - 1));
            _startPoint.Add(new Point(middle, 0));
            _startPoint.Add(new Point(middle, n - 1));
            _startPoint.Add(new Point(0, middle));
            _startPoint.Add(new Point(n - 1, middle));
        }

        /// <summary>
        /// ボードを描画する
        /// </summary>
        /// <param name="hold">選択中のピース番号（非選択:<0）</param>
        /// <param name="pos">マウスの位置（0～1）</param>
        /// <returns></returns>
        public Bitmap DrawBoard(int hold, PointF pos, int rotate)
        {
            var bmp = new Bitmap(BOARD_IMG_SIZE, BOARD_IMG_SIZE);
            var g = Graphics.FromImage(bmp);
            // 背景と枠の描画
            g.FillRectangle(Brushes.AntiqueWhite, 0, 0, bmp.Width, bmp.Height);
            var gridPen = new Pen(Color.DarkBlue, 2);
            var padding = 3;
            for (var i = 0; i < BoardSize + 1; i++)
            {
                g.DrawLine(gridPen, MARGIN + i * _cellSize, MARGIN, MARGIN + i * _cellSize, BOARD_IMG_SIZE - MARGIN);
                g.DrawLine(gridPen, MARGIN, MARGIN + i * _cellSize, BOARD_IMG_SIZE - MARGIN, MARGIN + i * _cellSize);
            }
            // マスの描画
            for (var y = 0; y < BoardSize; y++)
            {
                for (var x = 0; x < BoardSize; x++)
                {
                    if (Cell[y, x] >= 0)
                    {
                        // 設置済みマス描画
                        g.FillRectangle(new SolidBrush(PieceColors[Cell[y, x]]),
                            MARGIN + padding + x * _cellSize, MARGIN + padding + y * _cellSize,
                            _cellSize - padding * 2, _cellSize - padding * 2);
                    }
                    else if (_startPoint[_game.Turn] == new Point(x, y) && BoardSize > 1)
                    {
                        // 開始点描画
                        g.FillEllipse(new SolidBrush(PieceColors[_game.Turn]),
                            MARGIN + padding + x * _cellSize, MARGIN + padding + y * _cellSize,
                            _cellSize - padding * 2, _cellSize - padding * 2);
                    }
                }
            }
            // 操作ピースの描画
            if (hold >= 0 && hold < Pieces.Count())
            {
                var cursor = CursorCell(pos);
                var shape = Pieces[hold].Cells(rotate);
                shape.ForEach(p => {
                    var px = cursor.X + p.X;
                    var py = cursor.Y + p.Y;
                    if (px >= 0 && py >= 0 && px < BoardSize && py < BoardSize)
                    {
                        g.FillRectangle(new SolidBrush(PieceColors[_game.Turn]),
                            MARGIN + padding + px * _cellSize, MARGIN + padding + py * _cellSize,
                            _cellSize - padding * 2, _cellSize - padding * 2);
                    }
                });
            }

            return bmp;
        }

        /// <summary>
        /// ピースが置けるかチェック
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <param name="piece">ピース</param>
        /// <param name="pos">マスの位置</param>
        /// <returns>true: 有効な場所 false: 無効な場所</returns>
        public bool CheckPlace(int player, SetInfo si)
        {
            // 値検証
            if (si.Piece < 0 || si.Piece >= Pieces.Count()) return false;
            if (si.Rotate < 0 || si.Rotate > 7) return false;
            if (si.Pos < 0 || si.Pos >= BoardSize * BoardSize) return false;

            // ピースが盤外でないかチェック
            var cells = Pieces[si.Piece].Cells(si.Rotate).Select(c => new Point(c.X + si.Cell.X, c.Y + si.Cell.Y));
            if (cells.Any(p => p.X < 0 || p.Y < 0 || p.X >= BoardSize || p.Y >= BoardSize))
            {
                //Debug.WriteLine("ピースが盤外");
                return false;
            }
            // ピースの重なりがないかチェック
            if (cells.Any(p => Cell[p.Y, p.X] >= 0))
            {
                //Debug.WriteLine("ピースの重なり");
                return false;
            }
            // 開始点に置かれている
            if (cells.Any(p => p == _startPoint[_game.Turn])) return true;
            // 自マスと辺で接している
            var edges = Pieces[si.Piece].Edges(si.Rotate).
                Select(c => new Point(c.X + si.Cell.X, c.Y + si.Cell.Y)).
                Where(c => c.X >= 0 && c.X < BoardSize && c.Y >= 0 && c.Y < BoardSize);
            if (edges.Any(p => Cell[p.Y, p.X] == player))
            {
                //Debug.WriteLine("自マスと辺で接");
                return false;
            }
            // 自マスと角で接している
            var corners = Pieces[si.Piece].Corners(si.Rotate).
                Select(c => new Point(c.X + si.Cell.X, c.Y + si.Cell.Y)).
                Where(c => c.X >= 0 && c.X < BoardSize && c.Y >= 0 && c.Y < BoardSize);
            if (corners.Any(p => Cell[p.Y, p.X] == player)) return true;

            //Debug.WriteLine("自マスと角で接なし");
            return false;
        }

        /// <summary>
        /// ピースを置く（ピース指定）
        /// </summary>
        /// <param name="player">プレイヤー番号(0-7)</param>
        /// <param name="hold">選択ピース番号</param>
        /// <param name="pos">マス位置</param>
        /// <returns>true: 置いた false: 置けなかった</returns>
        public void SetPiece(int player, SetInfo si)
        {
            var cells = Pieces[si.Piece].Cells(si.Rotate).Select(c => new Point(c.X + si.Cell.X, c.Y + si.Cell.Y));
            cells.ToList().ForEach(p => { Cell[p.Y, p.X] = player; });
        }

        /// <summary>
        /// カーソル位置のセルを得る
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Point CursorCell(PointF pos)
        {
            var x = (int)((pos.X * BOARD_IMG_SIZE - MARGIN) / _cellSize);
            var y = (int)((pos.Y * BOARD_IMG_SIZE - MARGIN) / _cellSize);
            return new Point(x, y);
        }
        public void CalculateScore()
        {
            Scores = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
            Records = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
            for (var i = 0; i < _game.NumPlayers; i++) // to solve point == 0 bug
            {
                Scores[i] = 0;
                Records[i] = 0;
            }
            for (var y = 0; y < BoardSize; y++)
            {
                for (var x = 0; x < BoardSize; x++)
                {
                    if (Cell[y, x] >= 0 && Cell[y, x] < _game.NumPlayers)
                    {
                        Scores[Cell[y, x]] += 1;
                        Records[Cell[y, x]] += 1;
                    }
                }
            }
        }
    }
}
