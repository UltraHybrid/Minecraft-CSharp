using System;
using System.Diagnostics;
using System.Linq;

namespace tmp
{
    public class Game2
    {
        public World2 World { get; }
        public Player Player { get; private set; }
        private readonly ChunkManager2 manager;

        public Game2(int worldSize, PointI worldOffset, ChunkManager2 manager)
        {
            this.manager = manager;
            World = new World2(worldOffset, worldSize);
            manager.SetWorld(World);
        }

        public void Start()
        {
            var ready = manager.FirstGeneration();
            Console.WriteLine("Ready " + ready);
            Console.WriteLine("World " + World.Count);
            Player = new Player(DefineSpawn(ready), new Vector(0, 0, 1), 10, 15);
        }

        public void Update()
        {
            var a = new Stopwatch();
            a.Start();
            manager.Update();
            a.Stop();
            //Console.WriteLine(a.ElapsedMilliseconds);
        }

        private Vector DefineSpawn(PointI ready)
        {
            var empty = Enumerable.Range(-252, 252)
                .Select(Math.Abs)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                .First(p => World.GetBlock(World2.GetAbsolutePosition(new PointB(0, (byte) p, 0), ready)) != null &&
                            World.GetBlock(World2.GetAbsolutePosition(new PointB(0, (byte) (p+1), 0), ready)) == null &&
                            World.GetBlock(World2.GetAbsolutePosition(new PointB(0, (byte) (p+2), 0), ready)) == null);
            return (Vector) World2.GetAbsolutePosition(PointB.Default, ready) + new Vector(0.5f, empty + 1, 0.5f);
        }
    }
}