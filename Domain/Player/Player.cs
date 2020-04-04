namespace tmp
{
    public class Player
    {
        public int Hardness { get; private set; }
        public readonly FreeFlyMover Mover;
        public float Height { get; }

        public Player(Vector position, Vector direction, int hardness, float speed)
        {
            Mover = new FreeFlyMover(position, direction, speed);
            Hardness = hardness;
            Height = 1.8f;
        }
    }
}