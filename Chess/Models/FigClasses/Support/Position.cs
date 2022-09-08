namespace Chess.Models.FigClasses.Support
{
    public class Position
    {
        public Position() : this(-1, -1)
        {
        }

        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y; 
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
