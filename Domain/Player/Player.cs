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
            Mover = new SurvivalMover(position, direction, 0.2f, 1.6f);
            Hardness = hardness;
            Height = 1.8f;
        }
    }
}