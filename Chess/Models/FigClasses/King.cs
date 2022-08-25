using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Chess.Enums;
using static Chess.FigClasses.Support.Moves;

namespace Chess.FigClasses
{
    public class King : NewButton
    {
        public override bool Potential(int x2, int y2, ChessSide s) => 
            HorizontalVertical(X, Y, x2, y2, Side, false) || Diagonal(X, Y, x2, y2, Side, false); //ходы короля аналогичны ферзю, но на 1 клетку, поэтому false

        public King(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            this.Type = Type;
            this.X = X;
            this.Y = Y;
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
