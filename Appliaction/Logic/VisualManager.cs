using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tmp.Interfaces;

namespace tmp.Logic
{
    public class VisualManager : IChunkManager<VisualizerData>
    {
        private readonly IVisualizer<Block> visualizer;
        private readonly VisualWorld visualWorld;
        private const int TaskCount = 4;
        private readonly Task<VisualChunk>[] tasks;
        private readonly Queue<Chunk<Block>> needVisualize;
        public readonly Queue<(PointI, PointI)> Ready;

        public World<VisualChunk, VisualizerData> World => visualWorld;

        public VisualManager(IVisualizer<Block> visualizer, VisualWorld visualWorld)
        {
            this.visualizer = visualizer;
            this.visualWorld = visualWorld;
            tasks = new Task<VisualChunk>[TaskCount];
            needVisualize = new Queue<Chunk<Block>>();
            Ready = new Queue<(PointI, PointI)>();
        }

        private void AssignTasks()
        {
            for (var i = 0; i < TaskCount; i++)
            {
                if (tasks[i] == null && needVisualize.Count > 0)
                {
                    var nextChunk = needVisualize.Dequeue();
                    tasks[i] = Task.Run(() =>
                    {
                        var chunk = visualizer.Visualize(nextChunk);
                        var (positions, textureData) = chunk.AdaptToStupidData();
                        chunk.SimpleData = new RevisedData(positions, textureData);
                        return chunk;
                    });
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
                            visualWorld[result.Position] = result;
                            tasks[i] = null;
                            Ready.Enqueue((result.Position, result.Position));
                            //Console.WriteLine("Faces!!!!!!!!!!!!!!!!!");
                            break;
                        }
                        case TaskStatus.Faulted:
                        {
                            Console.WriteLine("Не удалось рассчитать грани для чанка " + tasks[i].Exception);
                            tasks[i] = null;
                            break;
                        }
                    }
            }
        }

        public PointI MakeFirstLunch()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            CheckStatusOfTasks();
            //Console.WriteLine(needVisualize.Count);
            AssignTasks();
        }

        public void MakeShift(PointI offset, PointI playerPosition)
        {
            throw new System.NotImplementedException();
        }

        public void HandlerForAdd(Chunk<Block> chunk)
        {
            needVisualize.Enqueue(chunk);
        }

        public void HandlerForDelete()
        {
        }

        public void HandlerForUpdate()
        {
        }
    }
}