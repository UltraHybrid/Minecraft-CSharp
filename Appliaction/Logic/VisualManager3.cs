using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using tmp.Interfaces;

namespace tmp.Logic
{
    public class VisualManager3 : IChunkManager<VisualizerData>
    {
        private readonly IVisualizer<Block> visualizer;
        private readonly VisualWorld visualWorld;
        private readonly Queue<Chunk<Block>> needVisualize;
        public readonly Queue<(PointI, PointI)> Ready;

        public World<VisualChunk, VisualizerData> World => visualWorld;

        public VisualManager3(IVisualizer<Block> visualizer, VisualWorld visualWorld)
        {
            this.visualizer = visualizer;
            this.visualWorld = visualWorld;
            needVisualize = new Queue<Chunk<Block>>();
            Ready = new Queue<(PointI, PointI)>();
        }

        private object locker = new object();

        private void AssignTasks()
        {
            if (needVisualize.Count != 0)
                ThreadPool.QueueUserWorkItem((chunk) =>
                {
                    var ch = (Chunk<Block>) chunk;
                    var result = visualizer.Visualize(ch);
                    var (positions, textureData) = result.AdaptToStupidData();
                    result.SimpleData = new RevisedData(positions, textureData);
                    lock (locker)
                    {
                        visualWorld[result.Position] = result;
                        Ready.Enqueue((result.Position, result.Position));
                    }
                }, needVisualize.Dequeue());
        }

        private void CheckStatusOfTasks()
        {
        }

        public PointI MakeFirstLunch()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
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