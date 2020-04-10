using System;
using System.Collections.Generic;

namespace tmp
{
    public class ChunkManager : IChunkManager
    {
        private readonly IGenerator<int, Chunk> generator;
        private World world;
        private readonly Queue<PointI> queue;

        public event Action Notify;

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
            var chunk = generator.Generate(x + world.gloabalOffset.X, z + world.gloabalOffset.Z);
            chunk.Position = new PointI(x, 0, z).Add(world.gloabalOffset);
            world[x, z] = chunk;
            return chunk;
        }

        public void Save(Chunk chunk)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnNotify()
        {
            Notify?.Invoke();
        }
    }
}