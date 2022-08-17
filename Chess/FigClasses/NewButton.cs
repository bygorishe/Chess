using System.Windows.Controls;
using Chess.Enums;

namespace Chess
{
    public abstract class NewButton : Button
    {
        public ChessSide Side { get; set; }
        public int NumOfFirstTurn { get; set; } = 0;
        public ChessType Type { get; protected set; }
        public int X { get; set; }
        public int Y { get; set; }
        public abstract bool Potential(int x2, int y2, ChessSide s);
    }
}
