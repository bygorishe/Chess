using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Chess.Enums;
using static Chess.MainWindow;
using static System.Math;
using Chess.Models.FigClasses.Support;

namespace Chess.FigClasses
{
    public class Pawn : Button, IPiece
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
        private void TakePawn()
        {

        }
        void Transform()
        {

        }
        //public override bool Potential(int x2, int y2, ChessSide s)
        //{
        //    int temp = 1;
        //    if (Side == 0)  //в зависимости от стороны ход пешки либо вниз либо вверх
        //        temp = -1;
        //    if (NumOfFirstTurn == 0) //первый ход на одну или две клетки        
        //        return (x2 == X + temp || (x2 == X + 2 * temp && buttonMap[X + temp, Y].Type == 0)) && Y == y2 && s == ChessSide.NoSide || (s != Side && s != ChessSide.NoSide && x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1));
        //    //взятие на проходе
        //    else if (x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1) && s == ChessSide.NoSide &&  //клетка по диагонали свободна
        //        (X == 3 || X == 4) &&  //пешка противника сделала ход на две клекти (тут какбы своей X, но как бы да)
        //        buttonMap[X, Y + Sign(y2 - Y)].Side != Side && buttonMap[X, Y + Sign(y2 - Y)].NumOfFirstTurn == turnNumber + 1) //пешка протиника слева или справа, а также ходила на прошлом ходу
        //    {
        //        //chessBoard.Children.RemovebuttonMap[X, Y + Sign(y2 - Y)]);
        //        NoType button = new NoType(ChessSide.NoSide, ChessType.Notype, X, Y + Sign(y2 - Y));
        //        //button.Background = buttonMap[X, Y + Sign(y2 - Y)].Background;
        //        button.BorderBrush = Brushes.Black;
        //        Grid.SetRow(button, X);
        //        Grid.SetColumn(button, Y + Sign(y2 - Y));
        //        passantButton = button;
        //        return true;
        //    }
        //    else
        //        return (x2 == X + temp && Y == y2 && s == ChessSide.NoSide) || (s != Side && s != ChessSide.NoSide && x2 == X + temp && (Y == y2 + 1 || Y == y2 - 1));
        //}
        public Pawn(ChessSide Side, ChessType Type, int X, int Y)
        {
            this.Side = Side;
            //this.Type = Type;
            Position = new Position(X, Y);
            Image img = new Image();

            if (this.Side == ChessSide.Black)
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/pawnBlack.png", UriKind.Relative));
            else
                img.Source = new BitmapImage(new Uri("FigClasses/Support/Fig/pawnWhite.png", UriKind.Relative));
            Content = img;
            Background = null;
        }
    }

}
