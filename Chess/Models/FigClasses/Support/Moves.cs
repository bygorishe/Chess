using Chess.Enums;
using static System.Math;
using static Chess.MainWindow;  //хзхз мб хуйня и так нельзя

namespace Chess.FigClasses.Support
{
    public class Moves
    {
        public static bool HorizontalVertical(int x1, int y1, int x2, int y2, ChessSide s, bool check)  //верх низ лево право
        {
            if (x1 == x2) //горизонтальные 
            {
                int signY = Sign(y1 - y2), y = y1 - signY;
                if (check)  //больше одной клетки ход
                    while (y != y2 && buttonMap[x1, y].Type == 0) //если на пути клетки пустые
                        y -= signY;
                return buttonMap[x1, y].Side != s && y == y2;
            }
            else if (y1 == y2) //вертикальные
            {
                int signX = Sign(x1 - x2), x = x1 - signX;
                if (check)
                    while (x != x2 && buttonMap[x, y1].Type == 0)
                        x -= signX;
                return buttonMap[x, y1].Side != s && x == x2;
            }
            else
                return false;
        }
        public static bool Diagonal(int x1, int y1, int x2, int y2, ChessSide s, bool check)
        {
            if (Abs(x1 - x2) == Abs(y1 - y2))  //если клетка находиться на диагонали
            {
                int signX = Sign(x1 - x2),
                signY = Sign(y1 - y2),
                x = x1 - signX, y = y1 - signY;
                if (check) //аналогично с вертикальными и горизонтальными
                    while (x != x2 && y != y2 && buttonMap[x, y].Type == 0)
                    {
                        x -= signX;
                        y -= signY;
                    }
                return buttonMap[x, y].Side != s && x == x2 && y == y2;
            }
            else
                return false;
        }
    }
}
