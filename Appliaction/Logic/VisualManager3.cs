using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualManager3
    {
        private readonly IVisualizer<Block> visualizer;
        private readonly VisualWorld visualWorld;
        public readonly Queue<(PointI, PointI)> Ready;

        public World<VisualChunk, VisualizerData> World => visualWorld;

        public VisualManager3(IVisualizer<Block> visualizer, VisualWorld visualWorld)
        {
            this.visualizer = visualizer;
            this.visualWorld = visualWorld;
            Ready = new Queue<(PointI, PointI)>();
        }

        private object locker = new object();

        public PointI MakeFirstLunch()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
        }

        public void MakeShift(PointI offset, PointI playerPosition)
        {
            throw new System.NotImplementedException();
        }

        public void HandlerForAdd(Chunk<Block> chunk)
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                var ch = (Chunk<Block>) c;
                var result = visualizer.Visualize(ch);
                var (positions, textureData) = result.AdaptToStupidData();
                result.SimpleData = new RevisedData(positions, textureData);

                visualWorld[result.Position] = result;
                Ready.Enqueue((result.Position, result.Position));
            }, chunk);
        }

        public void HandlerForDelete()
        {
        }

        public void HandlerForUpdate()
        {
        }
    }
}