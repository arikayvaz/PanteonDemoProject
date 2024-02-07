namespace Gameplay
{
    public struct GameBoardCoordinates
    {
        public static GameBoardCoordinates Invalid => new GameBoardCoordinates(-1, -1);

        public static bool IsValid(GameBoardCoordinates coord) 
        {
            return coord.x >= 0 && coord.y >= 0;
        } 

        public int x;
        public int y;

        public GameBoardCoordinates(int x, int y)
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
            if (!(obj is GameBoardCoordinates))
                return false;

            GameBoardCoordinates c = (GameBoardCoordinates)obj;

            return x == c.x && y == c.y;
        }

        public static bool operator ==(GameBoardCoordinates c1, GameBoardCoordinates c2) 
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(GameBoardCoordinates c1, GameBoardCoordinates c2)
        {
            return !c1.Equals(c2);
        }
    }
}