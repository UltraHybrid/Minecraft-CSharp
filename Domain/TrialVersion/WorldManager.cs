using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmp.Interfaces;

namespace tmp
{
    public class WorldManager : IChunkManager<Block>
    {
        private readonly IGenerator<int, Chunk<Block>> landscapeGenerator;
        private GameWorld world;
        private const int TaskCount = 2;
        private readonly Task<Chunk<Block>>[] tasks;
        private Queue<PointI> futureChunks;

        public event Action<Chunk<Block>> AddAlert;
        public event Action<Chunk<Block>> DeleteAlert;
        public event Action<Chunk<Block>> UpdateAlert;

        public WorldManager(IGenerator<int, Chunk<Block>> landscapeGenerator)
        {
            this.landscapeGenerator = landscapeGenerator;
            tasks = new Task<Chunk<Block>>[TaskCount];
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

        private void AssignTasks()
        {
            for (var i = 0; i < TaskCount; i++)
            {
                if (tasks[i] == null && futureChunks.Count > 0)
                {
                    var nextChunk = futureChunks.Dequeue();
                    tasks[i] = Task.Run(() => Generate(nextChunk));
                }
            }
        }

        private void CheckStatusOfTasks()
        {
            for (var i = 0; i < TaskCount; i++)
            {
                if (tasks[i] != null)
                    switch (tasks[i].Status)
                    {
                        case TaskStatus.RanToCompletion:
                        {
                            var result = tasks[i].Result;
                            world[result.Position] = result;
                            tasks[i] = null;
                            AddNotifyAll(result);
                            break;
                        }
                        case TaskStatus.Faulted:
                        {
                            Console.WriteLine("Не удалось сгенерировать чанк " + tasks[i].Exception);
                            tasks[i] = null;
                            break;
                        }
                    }
            }
        }

        public PointI MakeFirstLunch()
        {
            MakeShift(PointI.Default, PointI.CreateXZ(world.Size / 2, world.Size / 2).Add(world.Offset));
            AssignTasks();
            Task.WaitAll(tasks);
            var answer = tasks.First(x => x.Status == TaskStatus.RanToCompletion).Result.Position;
            CheckStatusOfTasks();
            return answer;
        }

        public void Update()
        {
            CheckStatusOfTasks();
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