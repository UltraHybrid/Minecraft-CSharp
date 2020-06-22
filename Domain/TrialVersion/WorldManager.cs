using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using tmp.Domain.Generators;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public delegate void BlockUpdateEvent(PointL position);

    public sealed class WorldManager : IWorldManager, IDisposable
    {
        private readonly IGenerator<PointI, Chunk<Block>> generator;
        private readonly ChunkManager<PointI, Chunk<Block>> manager;

        public World<Chunk<Block>, Block> World { get; }
        public event Action<Chunk<Block>> AddAlert;
        public event Action<Chunk<Block>> DeleteAlert;
        public event BlockUpdateEvent UpdateAlert;

        public WorldManager(IGenerator<PointI, Chunk<Block>> generator, 
            World<Chunk<Block>, Block> world)
        {
            this.generator = generator;
            manager = new ChunkManager<PointI, Chunk<Block>>(this.generator.Generate);
            World = world;
        }

        public PointI MakeFirstLunch()
        {
            MakeShift(PointI.Zero, PointI.CreateXZ(World.Size / 2, World.Size / 2).Add(World.Offset));
            manager.Start();
            while (manager.IsEmpty)
            {
            }

            var answer = manager.Pop();
            World[answer.Position] = answer;
            AddNotifyAll(answer);
            return answer.Position;
        }

        public void Update()
        {
            if (!manager.IsEmpty)
            {
                var chunk = manager.Pop();
                World[chunk.Position] = chunk;
                AddNotifyAll(chunk);
            }
        }

        public void MakeShift(PointI offset, PointI playerPosition)
        {
            World.Offset = World.Offset.Add(offset);
            var necessaryChunks = World.GetPointOfGaps();
            if (necessaryChunks.Count == 0)
                // ReSharper disable once RedundantJumpStatement
                return;

            var nearestChunks = necessaryChunks
                .OrderBy(p => p.GetDistance(playerPosition));
            foreach (var chunkPoint in nearestChunks)
            {
                manager.Push(chunkPoint);
            }
        }

        public void PutBlock(BlockType blockType, PointL position)
        {
            var (cPosition, ePosition) = World.Translate2LocalNotation(position);
            World[cPosition][ePosition] = blockType == null ? null : new Block(blockType, ePosition);
            OnUpdateAlert(position);
        }

        private void AddNotifyAll(Chunk<Block> chunk)
        {
            AddAlert?.Invoke(chunk);
        }

        private void DeleteAlertAll(Chunk<Block> chunk)
        {
            DeleteAlert?.Invoke(chunk);
        }

        private void OnUpdateAlert(PointL position)
        {
            UpdateAlert?.Invoke(position);
        }

        private bool isDisposed = false;

        ~WorldManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool fromDisposeMethod)
        {
            if (!isDisposed)
            {
                if (fromDisposeMethod)
                {
                    manager.Stop();
                }

                isDisposed = true;
            }
        }
    }
}