namespace tmp.Player
{
    public class PlayerPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public PlayerPoint(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static PlayerPoint operator +(PlayerPoint p1, PlayerPoint p2)
        {
            return new PlayerPoint(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
    }
}