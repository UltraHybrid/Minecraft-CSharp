using System.Linq;

namespace tmp
{
    public class Game
    {
        public readonly World World;
        public readonly Player Player;
        public readonly ChunkManager manager;

        public Game(int worldSize, ChunkManager manager)
        {
            this.manager = manager;
            var startOffset = new PointI(100 * worldSize, 0, 100 * worldSize);
            World = new World(worldSize, startOffset);
            manager.SetWorld(World);
            for (var x = 0; x < World.Size; x++)
            for (var z = 0; z < World.Size; z++)
            {
                manager.Create(x, z);
            }

            var halfSize = worldSize / 2;
            var empty = World[halfSize, halfSize].First(b =>
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                b != null && World[halfSize, halfSize][b.Position.Add(new PointB(0, 1, 0))] == null &&
                World[halfSize, halfSize][b.Position.Add(new PointB(0, 2, 0))] == null);
            var playerSpawn = (Vector) World.GetAbsolutPosition(empty, World[halfSize, halfSize].Position) +
                              new Vector(0.5f, 1, 0.5f);
            Player = new Player(playerSpawn,
                new Vector(0, 0, 1), 10, 15);
        }
    }
}