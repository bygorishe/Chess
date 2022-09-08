using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Chess.Enums;
using Chess.Models.FigClasses.Support;
using static System.Math;

namespace Chess.FigClasses
{
    public class Knight : Button, IPiece
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
        //    (Abs(X - x2) == 2 && Abs(Y - y2) == 1) || (Abs(X - x2) == 1 && Abs(Y - y2) == 2); //ходы коня "Г" - две в сторону, один вбок  

        public Knight(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            //this.Type = Type;
            Position = new Position(X, Y);
            Image img = new Image();
            if (this.Side == ChessSide.Black)
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/knightBlack.png", UriKind.Relative));
            else
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/knightWhite.png", UriKind.Relative));
            Content = img;
            Background = null;
        }
    }

}
