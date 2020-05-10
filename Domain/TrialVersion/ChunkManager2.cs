using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tmp
{
    public class ChunkManager2
    {
        private readonly IGenerator<int, Chunk> landscapeGenerator;
        private World2 world;
        private const int TaskCount = 4;
        private readonly Task<Chunk>[] tasks;
        private Queue<PointI> futureChunks;

        public event Action<Chunk> Notify;

        public ChunkManager2(IGenerator<int, Chunk> landscapeGenerator)
        {
            this.landscapeGenerator = landscapeGenerator;
            tasks = new Task<Chunk>[TaskCount];
            futureChunks = new Queue<PointI>();
        }

        public void SetWorld(World2 gameWorld)
        {
            world = gameWorld;
        }

        private Chunk Generate(PointI point)
        {
            var chunk = landscapeGenerator.Generate(point.X, point.Z);
            chunk.Position = point;
            return chunk;
        }

        public void Update()
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
                            NotifyAll(result);
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

            for (var i = 0; i < TaskCount; i++)
            {
                if (tasks[i] == null && futureChunks.Count > 0)
                {
                    var nextChunk = futureChunks.Dequeue();
                    tasks[i] = Task.Run(() => Generate(nextChunk));
                }
            }
        }

        public PointI FirstGeneration()
        {
            MakeShift(PointI.Default, PointI.CreateXZ(world.Size / 2, world.Size / 2).Add(world.GlobalOffset));
            Update();
            Task.WaitAll(tasks);
            var answer = tasks.First(x => x.Status == TaskStatus.RanToCompletion).Result.Position;
            Update();
            return answer;
        }

        public void MakeShift(PointI offset, PointI playerPosition)
        {
            futureChunks = new Queue<PointI>();
            world.GlobalOffset = world.GlobalOffset.Add(offset);
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

        protected virtual void NotifyAll(Chunk chunk)
        {
            Notify?.Invoke(chunk);
        }
    }
}