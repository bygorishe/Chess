using System.Windows.Controls;
using Chess.Enums;

namespace Chess
{
    public interface IPiece
    {
        public ChessSide Side { get; }
        public bool FirstTurn { get; set; }
        public ChessType Type { get; }
        public int X { get; set; }
        public int Y { get; set; }
        public abstract bool Potential(int x2, int y2, ChessSide s);
    }
}
