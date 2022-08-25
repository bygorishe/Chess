using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Chess.Enums;
using static Chess.FigClasses.Support.Moves;

namespace Chess.FigClasses
{
    public class Bishop : NewButton
    {
        public override bool Potential(int x2, int y2, ChessSide s) =>
            Diagonal(X, Y, x2, y2, Side, true);

        public Bishop(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            this.Type = Type;
            this.X = X;
            this.Y = Y;
            Image img = new Image();
            if (this.Side == ChessSide.Black)
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/bishopBlack.png", UriKind.Relative));
            else
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/bishopWhite.png", UriKind.Relative));
            Content = img;
            Background = null;
        }
    }

}
