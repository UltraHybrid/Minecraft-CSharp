using System;
using System.Collections.Generic;

namespace tmp
{
    public class ChunkManager : IChunkManager
    {
        private readonly IGenerator<int, Chunk> generator;
        private readonly Queue<PointI> queue;

        public event Action Notify;

        public ChunkManager(IGenerator<int, Chunk> generator)
        {
            this.generator = generator;
            this.queue = new Queue<PointI>();
        }

        public Chunk Load(int x, int z)
        {
            throw new NotImplementedException();
        }

        public Chunk Create(int x, int z)
        {
            OnNotify();
            return null;
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