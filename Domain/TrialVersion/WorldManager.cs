using System;
using System.Linq;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    public sealed class WorldManager : IWorldManager, IDisposable
    {
        private readonly IGenerator<PointI, Chunk<Block>> generator;
        private readonly ChunkManager<PointI, Chunk<Block>> manager;
        private readonly World<Chunk<Block>, Block> world;

        public event Action<Chunk<Block>> AddAlert;
        public event Action<Chunk<Block>> DeleteAlert;
        public event BlockUpdateEvent UpdateAlert;

        public WorldManager(World<Chunk<Block>, Block> world,
            IGenerator<PointI, Chunk<Block>> generator)
        {
            this.generator = generator;
            this.world = world;
            manager = new ChunkManager<PointI, Chunk<Block>>(this.generator.Generate);
        }

        public PointI MakeFirstLaunch()
        {
            MakeShift(PointI.Zero, PointI.CreateXZ(world.Size / 2, world.Size / 2).Add(world.Offset));
            manager.Start();
            while (manager.IsEmpty)
            {
            }

            var answer = manager.Pop();
            world[answer.Position] = answer;
            AddNotifyAll(answer);
            return answer.Position;
        }

        public void Update()
        {
            if (!manager.IsEmpty)
            {
                var chunk = manager.Pop();
                world[chunk.Position] = chunk;
                AddNotifyAll(chunk);
            }
        }

        public void MakeShift(PointI offset, PointI playerPosition)
        {
            world.Offset = world.Offset.Add(offset);
            var necessaryChunks = world.GetPointOfGaps();
            if (necessaryChunks.Count == 0)
                // ReSharper disable once RedundantJumpStatement
                return;

            var nearestChunks = necessaryChunks
                .OrderBy(p => p.GetDistance(playerPosition));
            foreach (var chunkPoint in nearestChunks)
            {
                manager.Push(chunkPoint);
                //Console.Write(chunkPoint);
            }
        }

        public void PutBlock(BlockType blockType, PointL position)
        {
            var (cPosition, ePosition) = world.Translate2LocalNotation(position);
            world[cPosition][ePosition] = blockType == null ? null : new Block(blockType, ePosition);
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