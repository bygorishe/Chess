using System.Windows.Media;
using Chess.Enums;

namespace Chess.FigClasses
{
    public class NoType : NewButton
    {
        private SolidColorBrush nobrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)); //если прсто null то появляются проблемы с выделением кнопки, не решаемые setZIndex
        public override bool Potential(int x2, int y2, ChessSide s) => true;
        public NoType(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            this.Type = Type;
            this.X = X;
            this.Y = Y;
            Background = nobrush;
        }
    }
}
