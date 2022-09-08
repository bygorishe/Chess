using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Chess.Enums;
using Chess.Models.FigClasses.Support;
using static Chess.FigClasses.Support.Moves;
namespace Chess.FigClasses
{
    public class Queen : Button, IPiece
    {
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
        //public override bool Potential(int x2, int y2, ChessSide s) =>
        //    HorizontalVertical(X, Y, x2, y2, Side, true) || Diagonal(X, Y, x2, y2, Side, true);
        public Queen(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            //this.Type = Type;
            Position = new Position(X, Y);
            Image img = new Image();
            if (this.Side == ChessSide.Black)
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/queenBlack.png", UriKind.Relative));
            else
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/queenWhite.png", UriKind.Relative));
            Content = img;
            Background = null;
        }
    }

}
