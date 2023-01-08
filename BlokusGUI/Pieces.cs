using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BlokusMod
{

    /// <summary>
    /// ピースクラス
    /// </summary>
    class Piece
    {
        private List<Point> _cells = new List<Point>();
        private List<Point> _edges = new List<Point>();
        private List<Point> _corners = new List<Point>();
        static List<int[]> RotateMatrices = new List<int[]> { new int[] {1,0,0,1 }, new int[] { 0,-1,1,0}, new int[] { -1, 0, 0, -1 }, new int[] { 0, 1, -1, 0 },
            new int[] { -1,0,0,1}, new int[] { 0,1,1,0}, new int[] { 1,0,0,-1}, new int[] { 0,-1,-1,0}};

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Piece(List<Point> cells)
        {
            _cells.AddRange(cells);

            var edges = new List<Point>() { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
            var corners = new List<Point>() { new Point(1, 1), new Point(-1, 1), new Point(-1, -1), new Point(1, -1) };

            foreach (var cell in _cells)
            {
                _edges.AddRange(edges.Select(e => new Point(e.X + cell.X, e.Y + cell.Y))
                    .Where(c => !_cells.Contains(c) && !_edges.Contains(c)));
            }
            foreach (var cell in _cells)
            {
                _corners.AddRange(corners.Select(e => new Point(e.X + cell.X, e.Y + cell.Y))
                    .Where(c => !_cells.Contains(c) && !_edges.Contains(c) && !_corners.Contains(c)));
            }
        }

        /// <summary>
        /// ピースを構成するセルリストを生成
        /// </summary>
        /// <param name="rotate"></param>
        /// <returns></returns>
        public List<Point> Cells(int rotate)
        {
            var rm = RotateMatrices[rotate % 8];
            return _cells.Select(c => new Point(c.X * rm[0] + c.Y * rm[1], c.X * rm[2] + c.Y * rm[3])).ToList();
        }

        /// <summary>
        /// ピースの辺に接するセルリストを生成
        /// </summary>
        /// <param name="rotate"></param>
        /// <returns></returns>
        public List<Point> Edges(int rotate)
        {
            var rm = RotateMatrices[rotate % 8];
            return _edges.Select(c => new Point(c.X * rm[0] + c.Y * rm[1], c.X * rm[2] + c.Y * rm[3])).ToList();
        }

        /// <summary>
        /// ピースの角に接するセルリストを生成
        /// </summary>
        /// <param name="rotate"></param>
        /// <returns></returns>
        public List<Point> Corners(int rotate)
        {
            var rm = RotateMatrices[rotate % 8];
            return _corners.Select(c => new Point(c.X * rm[0] + c.Y * rm[1], c.X * rm[2] + c.Y * rm[3])).ToList();
        }

        /// <summary>
        /// ピース形状取得（文字列）
        /// </summary>
        /// <param name="piece">ピース番号</param>
        /// <returns></returns>
        public string GetShapeString()
        {
            var grid = new bool[4, 5];
            var minX = _cells.Min(c => c.X);
            var minY = _cells.Min(c => c.Y);
            var maxX = 0;
            var maxY = 0;
            _cells.ForEach(p => {
                grid[p.Y - minY, p.X - minX] = true;
                if (p.X - minX > maxX) maxX = p.X - minX;
                if (p.Y - minY > maxY) maxY = p.Y - minY;
            });
            var shape = "";
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    shape += grid[y, x] ? "■" : "□";
                }
                shape += "\n";
            }
            return shape;
        }
    }
}
