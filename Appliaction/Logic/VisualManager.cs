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
    public class VisualManager : IVisualManager
    {
        private readonly IVisualizer<Block> visualizer;
        private readonly Queue<PointI> ReadyToUpdate;
        private readonly Queue<(PointI, PointI)> ReadyToReplace;
        private readonly object replaceLocker = new object();
        public VisualWorld World { get; }

        public bool HasDataToAdd()
        {
            lock (replaceLocker)
            {
                return ReadyToReplace.Count != 0;
            }
        }

        public (PointI, PointI) GetDataToAdd()
        {
            lock (replaceLocker)
            {
                return ReadyToReplace.Dequeue();
            }
        }

        public bool HasDataToUpdate()
        {
            return ReadyToUpdate.Count != 0;
        }

        public PointI GetDataToUpdate()
        {
            return ReadyToUpdate.Dequeue();
        }

        public VisualManager(IVisualizer<Block> visualizer, VisualWorld visualWorld)
        {
            this.visualizer = visualizer;
            World = visualWorld;
            ReadyToUpdate = new Queue<PointI>();
            ReadyToReplace = new Queue<(PointI, PointI)>();
        }

        public void HandlerForAdd(Chunk<Block> chunk)
        {
            ThreadPool.QueueUserWorkItem((c) =>
            {
                var ch = (Chunk<Block>) c;
                var result = visualizer.Visualize(ch);
                World[result.Position] = result;
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
            var needToUpdate = new List<PointI>();

            foreach (var point in PointWithNeighbors)
            {
                var shiftPoint = position.Add(point);
                var (cPosition, ePosition) = World.Translate2LocalNotation(shiftPoint);
                var microChunkPosition = cPosition.Add(new PointI(0, ePosition.Y / VisualChunk.CountInLevel, 0));
                if (!needToUpdate.Contains(microChunkPosition))
                    needToUpdate.Add(microChunkPosition);
                World.TrySetItem(shiftPoint, visualizer.UpdateOne(shiftPoint));
            }

            foreach (var point in needToUpdate)
            {
                var coords = PointI.CreateXZ(point.X, point.Z);
                if (World.ContainsChunk(coords))
                {
                    World[coords].AdaptToStupidData(point.Y);
                    ReadyToUpdate.Enqueue(point);
                }
            }
        }
    }
}