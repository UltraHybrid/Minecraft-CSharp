using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace tmp
{
    public sealed class ChunkManager : IChunkManager
    {
        private readonly IGenerator<int, Chunk> generator;
        private World world;
        private readonly Queue<PointI> queue;

        public event Action<Chunk> Notify;

        public ChunkManager(IGenerator<int, Chunk> generator)
        {
            this.generator = generator;
        }

        public void SetWorld(World world) => this.world = world;

        public Chunk Load(int x, int z)
        {
            throw new NotImplementedException();
        }

        public Chunk Create(int x, int z)
        {
            if (world == null)
                throw new InvalidOperationException("The world is not defined");
            var chunk = generator.Generate(x + world.globalOffset.X, z + world.globalOffset.Z);
            var position = new PointI(x, 0, z).Add(world.globalOffset);
            chunk.Position = position;
            return chunk;
        }

        public void MakeShift(PointI shift, PointI playerPoint)
        {
            var t = new Stopwatch();
            t.Start();
            var futureWorld = new Chunk[world.Size, world.Size];
            var needGenerate = new List<PointI>();
            for (var i = 0; i < world.Size; i++)
            {
                for (var k = 0; k < world.Size; k++)
                {
                    var futurePoint = new PointI(i, 0, k);
                    var oldPoint = futurePoint.Add(shift);
                    if (oldPoint.X >= 0 && oldPoint.X < world.Size &&
                        oldPoint.Z >= 0 && oldPoint.Z < world.Size)
                    {
                        futureWorld[i, k] = world[oldPoint.X, oldPoint.Z];
                        if (futureWorld[i, k] == null)
                            needGenerate.Add(futurePoint);
                    }
                    else needGenerate.Add(futurePoint);
                }
            }

            world.globalOffset = world.globalOffset.Add(shift);
            var dictLock = new object();
            var dict = new Dictionary<PointI, Chunk>();
            var newChunks = Parallel.ForEach(needGenerate, p =>
            {
                var data = Create(p.X, p.Z);
                lock (dictLock)
                {
                    dict[p] = data;
                }
            });
            for (var i = 0; i < world.Size; i++)
            {
                for (var k = 0; k < world.Size; k++)
                {
                    if (futureWorld[i, k] != null)
                        world[i, k] = futureWorld[i, k];
                }
            }

            Console.WriteLine("Generate new chunks " + dict.Count);
            foreach (var pair in dict)
            {
                world[pair.Key.X, pair.Key.Z] = pair.Value;
            }

            dict.Select(p => p.Value)
                .OrderByDescending(p => p.Position.GetDistance(playerPoint))
                .ToList()
                .ForEach(NotifyAll);

            t.Stop();
            Console.WriteLine($"World Update {t.ElapsedMilliseconds}");
        }

        public void Save(Chunk chunk)
        {
            throw new NotImplementedException();
        }

        private void NotifyAll(Chunk chunk)
        {
            Notify?.Invoke(chunk);
        }
    }
}