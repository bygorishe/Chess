using System.Windows.Controls;
using System.Windows.Media;
using Chess.Enums;
using Chess.Models.FigClasses.Support;

namespace Chess.FigClasses
{
    public class NoType : Button, IPiece
    {
        private SolidColorBrush nobrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)); //если прсто null то появляются проблемы с выделением кнопки, не решаемые setZIndex
        public ChessSide Side { get; }
        public bool FirstTurn { get; set; } = true;
        //public ChessType Type { get; }
        public Position Position { get; set; }
        public bool Possible(Position position)
        {
            //
            return true;
        }
        public void Move()
        {
            //
        }
        //public override bool Potential(int x2, int y2, ChessSide s) => true;
        public NoType(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            //this.Type = Type;
            Position = new Position(X, Y);
            Background = nobrush;
        }
    }
}
