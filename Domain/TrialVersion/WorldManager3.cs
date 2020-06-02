using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using tmp.Interfaces;

namespace tmp
{
    public class WorldManager3 : IChunkManager<Block>
    {
        private readonly IGenerator<int, Chunk<Block>> landscapeGenerator;
        private GameWorld world;
        private Queue<PointI> futureChunks;

        public event Action<Chunk<Block>> AddAlert;
        public event Action<Chunk<Block>> DeleteAlert;
        public event Action<Chunk<Block>> UpdateAlert;

        public WorldManager3(IGenerator<int, Chunk<Block>> landscapeGenerator)
        {
            this.landscapeGenerator = landscapeGenerator;
            futureChunks = new Queue<PointI>();
        }

        public void SetWorld(GameWorld gameWorld)
        {
            world = gameWorld;
        }

        private Chunk<Block> Generate(PointI position)
        {
            return landscapeGenerator.Generate(position.X, position.Z);
        }

        private object locker = new object();

        private void AssignTasks()
        {
            if (futureChunks.Count != 0)
                ThreadPool.QueueUserWorkItem((point) =>
                {
                    var p = (PointI) point;
                    var result = landscapeGenerator.Generate(p.X, p.Z);
                    lock (locker)
                    {
                        world[result.Position] = result;
                    }

                    AddNotifyAll(result);
                }, futureChunks.Dequeue());
        }

        public PointI MakeFirstLunch()
        {
            MakeShift(PointI.Default, PointI.CreateXZ(world.Size / 2, world.Size / 2).Add(world.Offset));
            var firstPoint = futureChunks.Dequeue();
            var answer = landscapeGenerator.Generate(firstPoint.X, firstPoint.Z);
            world[answer.Position] = answer;
            return answer.Position;
        }

        public void Update()
        {
            AssignTasks();
        }

        public void MakeShift(PointI offset, PointI playerPosition)
        {
            futureChunks = new Queue<PointI>();
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
                futureChunks.Enqueue(chunkPoint);
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
    }
}