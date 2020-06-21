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
        public readonly Queue<PointI> ReadyToUpdate;
        public readonly Queue<(PointI, PointI)> ReadyToReplace;
        private readonly object replaceLocker = new object();

        public VisualWorld World => visualWorld;

        public VisualManager3(IVisualizer<Block> visualizer, VisualWorld visualWorld)
        {
            this.visualizer = visualizer;
            this.visualWorld = visualWorld;
            ReadyToUpdate = new Queue<PointI>();
            ReadyToReplace = new Queue<(PointI, PointI)>();
        }

        public void Update()
        {
        }

        public void HandlerForAdd(Chunk<Block> chunk)
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                var ch = (Chunk<Block>) c;
                var result = visualizer.Visualize(ch);
                visualWorld[result.Position] = result;
                for (var i = VisualChunk.RowDataLevels - 1; i >= 0; i--)
                {
                    result.AdaptToStupidData(i);
                    var queuePoint = result.Position.Add(new PointI(0, i, 0));
                    lock (replaceLocker)
                    {
                        ReadyToReplace.Enqueue((queuePoint, queuePoint));
                    }
                }
            }, chunk);
        }

        public void HandlerForDelete()
        {
        }

        private static readonly PointL[] PointWithNeighbors =
        {
            PointL.Zero,
            new PointL(1, 0, 0), new PointL(-1, 0, 0),
            new PointL(0, 1, 0), new PointL(0, -1, 0),
            new PointL(0, 0, 1), new PointL(0, 0, -1)
        };

        public void HandlerForUpdate(PointL position)
        {
            Console.WriteLine("Put " + position);

            foreach (var point in PointWithNeighbors)
            {
                var shiftPoint = position.Add(point);
                World.TrySetItem(shiftPoint, visualizer.UpdateOne(shiftPoint));
            }

            var (cPosition, ePosition) = World.Translate2LocalNotation(position);
            var queuePoint = cPosition.Add(new PointI(0, ePosition.Y / VisualChunk.CountInLevel, 0));
            World[cPosition].AdaptToStupidData(ePosition.Y / VisualChunk.CountInLevel);
            ReadyToUpdate.Enqueue(queuePoint);
        }
    }
}