﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmp.Interfaces;

namespace tmp
{
    public class WorldManager2: IDisposable
    {
        private readonly IGenerator<int, Chunk<Block>> landscapeGenerator;
        private readonly ChunkManager<PointI, Chunk<Block>> manager;
        private GameWorld world;

        public event Action<Chunk<Block>> AddAlert;
        public event Action<Chunk<Block>> DeleteAlert;
        public event Action<Chunk<Block>> UpdateAlert;

        public WorldManager2(IGenerator<int, Chunk<Block>> landscapeGenerator)
        {
            this.landscapeGenerator = landscapeGenerator;
            this.manager =
                new ChunkManager<PointI, Chunk<Block>>((point) => this.landscapeGenerator.Generate(point.X, point.Z));
        }

        public void SetWorld(GameWorld gameWorld)
        {
            world = gameWorld;
        }

        private Chunk<Block> Generate(PointI position)
        {
            return landscapeGenerator.Generate(position.X, position.Z);
        }

        public PointI MakeFirstLunch()
        {
            MakeShift(PointI.Default, PointI.CreateXZ(world.Size / 2, world.Size / 2).Add(world.Offset));
            manager.Start();
            while (manager.IsEmpty)
            {
            }

            var answer = manager.Pop();
            world[answer.Position] = answer;
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
            Console.Write("Запланированы: ");
            foreach (var chunkPoint in nearestChunks)
            {
                manager.Push(chunkPoint);
                Console.Write(chunkPoint);
            }
        }

        protected virtual void AddNotifyAll(Chunk<Block> chunk)
        {
            AddAlert?.Invoke(chunk);
        }

        protected virtual void DeleteAlertAll(Chunk<Block> chunk)
        {
            DeleteAlert?.Invoke(chunk);
        }

        protected virtual void UpdateAlertAll(Chunk<Block> chunk)
        {
            UpdateAlert?.Invoke(chunk);
        }

        public void Dispose()
        {
            manager.Stop();
        }
    }
}