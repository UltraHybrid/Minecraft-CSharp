using System;
using System.Linq;

namespace tmp
{
    public class Game: IGame
    {
        public IWorld World => world;
        private World world;
        public Player Player { get; }
        public readonly ChunkManager Manager;
        public PointI Previous;

        public Game(ChunkManager manager, World world)
        {
            Manager = manager;
            world = world;
            var rnd = new Random();
            var number = rnd.Next(world.GlobalOffset.X * Chunk.XLength,
                (world.GlobalOffset.X + world.Size - 1) * Chunk.XLength);
            var playerPosition = new PointI(number, 0, number);
            manager.MakeShift(PointI.Default, playerPosition);

            Player = InitPlayer(playerPosition);
            Previous = world.Convert2ChunkPoint((PointI) Player.Mover.Position);
        }

        private Player InitPlayer(PointI playerPosition)
        {
            Console.WriteLine(playerPosition);
            var empty = Enumerable.Range(-252, 252)
                .Select(Math.Abs)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                .First(p => world[playerPosition.Add(new PointI(0, p, 0))] != null &&
                            world[playerPosition.Add(new PointI(0, p + 1, 0))] == null &&
                            world[playerPosition.Add(new PointI(0, p + 2, 0))] == null);
            var playerSpawn = (Vector) playerPosition + new Vector(0.5f, empty + 1, 0.5f);
            return new Player(playerSpawn,
                new Vector(0, 0, 1), 10, 15);
        }

        public void Start()
        {
        }

        public void Update()
        {
            var currentChunk = world.Convert2ChunkPoint((PointI) Player.Mover.Position);
            var shift = currentChunk.Add(-Previous);
            if (!Equals(shift, PointI.Default))
            {
                Manager.MakeShift(shift, currentChunk);
                Console.WriteLine("!!!!!!!" + shift);
                Previous = currentChunk;
            }
        }
    }
}