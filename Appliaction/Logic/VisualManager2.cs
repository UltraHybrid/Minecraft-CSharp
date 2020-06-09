using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualManager2: IDisposable
    {
        private readonly IVisualizer<Block> visualizer;
        private readonly VisualWorld visualWorld;
        public readonly Queue<(PointI, PointI)> Ready;
        private readonly ChunkManager<Chunk<Block>, VisualChunk> manager;

        public World<VisualChunk, VisualizerData> World => visualWorld;

        public VisualManager2(IVisualizer<Block> visualizer, VisualWorld visualWorld)
        {
            this.visualizer = visualizer;
            this.visualWorld = visualWorld;
            manager = new ChunkManager<Chunk<Block>, VisualChunk>((chunk) =>
            {
                var result = visualizer.Visualize(chunk);
                var (positions, textureData) = result.AdaptToStupidData();
                result.SimpleData = new RevisedData(positions, textureData);
                return result;
            });
            Ready = new Queue<(PointI, PointI)>();
            manager.Start();
        }

        private void CheckStatusOfTasks()
        {
            if (!manager.IsEmpty)
            {
                var result = manager.Pop();
                visualWorld[result.Position] = result;
                Ready.Enqueue((result.Position, result.Position));
                Console.WriteLine("Faces!!!!!!!!!!!!!!!!! " + result.Position);
            }
        }

        public PointI MakeFirstLunch()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            CheckStatusOfTasks();
        }

        public void MakeShift(PointI offset, PointI playerPosition)
        {
            throw new System.NotImplementedException();
        }

        public void HandlerForAdd(Chunk<Block> chunk)
        {
            manager.Push(chunk);
        }

        public void HandlerForDelete()
        {
        }

        public void HandlerForUpdate()
        {
        }

        public void Dispose()
        {
            manager.Stop();
        }
    }
}