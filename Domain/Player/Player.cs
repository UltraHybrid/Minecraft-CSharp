namespace tmp
{
    public class Player
    {
        public int Hardness { get; private set; }
        public readonly EntityMover Mover;
        public float Height { get; }

        public Player(Vector position, Vector direction, int hardness)
        {
            //Mover = new FreeFlyMover(position, direction);
            Mover = new SurvivalMover(position, direction, 0.25f, 1.6f, 0.1f);
            Hardness = hardness;
            Height = 1.8f;
        }
    }
}