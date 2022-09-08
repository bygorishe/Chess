using Chess.Enums;
using Chess.Models.FigClasses.Support;

namespace Chess.FigClasses
{
    public interface IPiece
    {
        public ChessSide Side { get; }
        public bool FirstTurn { get; set; }
        //public ChessType Type { get; }
        public Position Position { get; set; }
        public bool Possible(Position position);
        public void Move();
    }
}
