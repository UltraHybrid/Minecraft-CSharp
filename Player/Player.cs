namespace tmp.Player
{
    public class Player
    {
        public PlayerPoint Position;
        public PlayerPoint Focus;
        public int Hardness;
        public PlayerPoint Direction;
        public float Speed { get; set; }

        public Player(PlayerPoint position, PlayerPoint focus, PlayerPoint direction, int hardness)
        {
            Position = position;
            Focus = focus;
            Hardness = hardness;
            Direction = direction;
        }
    }
}