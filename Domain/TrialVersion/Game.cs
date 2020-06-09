using System;
using System.Linq;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    using BlockWorld = World<Chunk<Block>, Block>;

    public class Game : IGame
    {
        public BlockWorld World { get; }
        public Player Player { get; private set; }

        private readonly WorldManager2 manager;

        public Game(int worldSize, PointI worldOffset, WorldManager2 manager)
        {
            var world = new GameWorld(worldOffset, worldSize);
            this.manager = manager;
            manager.SetWorld(world);
            World = world;
        }

        public void Start()
        {
            var ready = manager.MakeFirstLunch();
            Console.WriteLine("Ready ");
            Console.WriteLine("World " + World.Count);
            Player = new Player(DefineSpawn(ready), new Vector(0, 0, 1), 10);
        }

        public void Update()
        {
            manager.Update();
        }

        private Vector DefineSpawn(PointI ready)
        {
            var chunk = World[ready];
            for (byte x = 0; x < Chunk<Block>.XLength; x++)
            {
                for (byte z = 0; z < Chunk<Block>.ZLength; z++)
                {
                    for (byte y = 253; y > 0; y--)
                    {
                        var p0 = new PointB(x, y, z);
                        var p1 = new PointB(x, (byte) (y + 1), z);
                        var p2 = new PointB(x, (byte) (y + 2), z);
                        if (chunk[p0] != null && chunk[p1] == null && chunk[p2] == null)
                            return BlockWorld.GetAbsolutePosition(p1, ready).AsVector() + new Vector(0.5f, 0, 0.5f);
                    }
                }
            }

            throw new ArgumentException("Не удалось найти подходящего для спавна места", ready.ToString());

            /*var empty = Enumerable.Range(-252, 252)
                .Select(Math.Abs)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                .First(p =>
                    World.GetItem(World<Chunk<Block>, Block>.GetAbsolutePosition(new PointB(0, (byte) p, 0), ready)) !=
                    null &&
                    World.GetItem(
                        World<Chunk<Block>, Block>.GetAbsolutePosition(new PointB(0, (byte) (p + 1), 0), ready)) ==
                    null &&
                    World.GetItem(
                        World<Chunk<Block>, Block>.GetAbsolutePosition(new PointB(0, (byte) (p + 2), 0), ready)) ==
                    null);
            return (Vector) World<Chunk<Block>, Block>.GetAbsolutePosition(PointB.Zero, ready) +
                   new Vector(0.5f, empty + 1, 0.5f);*/
        }
    }
}