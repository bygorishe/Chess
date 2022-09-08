using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Chess.Enums;
using Chess.Models.FigClasses.Support;

namespace Chess.FigClasses
{
    public class King : Button, IPiece
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
        private void Passant()
        {

        }
        //public override bool Potential(int x2, int y2, ChessSide s) => 
        //HorizontalVertical(X, Y, x2, y2, Side, false) || Diagonal(X, Y, x2, y2, Side, false); //ходы короля аналогичны ферзю, но на 1 клетку, поэтому false

        public King(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            //this.Type = Type;
            Position = new Position(X, Y);
            Image img = new Image();
            if (this.Side == ChessSide.Black)
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/kingBlack.png", UriKind.Relative));
            else
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/kingWhite.png", UriKind.Relative));
            Content = img;
            Background = null;
        }
    }

}
