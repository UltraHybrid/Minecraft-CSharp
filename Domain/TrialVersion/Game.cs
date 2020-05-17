using System;
using System.Linq;
using tmp.Interfaces;

namespace tmp
{
    public class Game : IGame
    {
        public IWorld<Block> World { get; }
        public Player Player { get; private set; }

        private readonly WorldManager manager;

        public Game(int worldSize, PointI worldOffset, WorldManager manager)
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
            Player = new Player(DefineSpawn(ready), new Vector(0, 0, 1), 10, 15);
        }

        public void Update()
        {
            manager.Update();
        }

        private Vector DefineSpawn(PointI ready)
        {
            var empty = Enumerable.Range(-252, 252)
                .Select(Math.Abs)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                .First(p =>
                    World.GetItem(World<Block>.GetAbsolutePosition(new PointB(0, (byte) p, 0), ready)) != null &&
                    World.GetItem(World<Block>.GetAbsolutePosition(new PointB(0, (byte) (p + 1), 0), ready)) ==
                    null &&
                    World.GetItem(World<Block>.GetAbsolutePosition(new PointB(0, (byte) (p + 2), 0), ready)) == null);
            return (Vector) World<Block>.GetAbsolutePosition(PointB.Default, ready) +
                   new Vector(0.5f, empty + 1, 0.5f);
        }
    }
}