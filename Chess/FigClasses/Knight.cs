using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Chess.Enums;
using static System.Math;

namespace Chess.FigClasses
{
    public class Knight : NewButton
    {
        public override bool Potential(int x2, int y2, ChessSide s) =>
            (Abs(X - x2) == 2 && Abs(Y - y2) == 1) || (Abs(X - x2) == 1 && Abs(Y - y2) == 2); //ходы коня "Г" - две в сторону, один вбок  

        public Knight(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            this.Type = Type;
            this.X = X;
            this.Y = Y;
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
