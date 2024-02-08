namespace Gameplay
{
    public struct BoardCoordinate
    {
        public static BoardCoordinate Invalid => new BoardCoordinate(-1, -1);

        public static bool IsValid(BoardCoordinate coord) 
        {
            return coord.x >= 0 && coord.y >= 0;
        } 

        public int x;
        public int y;

        public BoardCoordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{x},{y}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BoardCoordinate))
                return false;

            BoardCoordinate c = (BoardCoordinate)obj;

            return x == c.x && y == c.y;
        }

        public static bool operator ==(BoardCoordinate c1, BoardCoordinate c2) 
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(BoardCoordinate c1, BoardCoordinate c2)
        {
            return !c1.Equals(c2);
        }
    }
}