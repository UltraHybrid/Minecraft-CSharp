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
        public readonly Queue<PointI> Ready;
        public readonly Queue<(PointI, PointI)> Ready2;
        private object readyLocker = new object();

        public VisualWorld World => visualWorld;

        public VisualManager3(IVisualizer<Block> visualizer, VisualWorld visualWorld)
        {
            this.visualizer = visualizer;
            this.visualWorld = visualWorld;
            Ready = new Queue<PointI>();
            Ready2 = new Queue<(PointI, PointI)>();
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
                visualWorld[result.Position] = result;
                //var (positions, textureData) = result.AdaptToStupidData();
                for (var i = 0; i < 16; i++)
                {
                    result.AdaptToStupidData(i);
                    var queuePoint = result.Position.Add(new PointI(0, i, 0));
                    lock (readyLocker)
                    {
                        Ready2.Enqueue((queuePoint, queuePoint));
                    }
                }

                //result.SimpleData = new RevisedData(positions, textureData);
                //Ready.Enqueue((result.Position, result.Position));
            }, chunk);
        }

        public void HandlerForDelete()
        {
        }

        public void HandlerForUpdate(PointI position)
        {
            Console.WriteLine("Put " + position);
            var data = visualizer.UpdateOne(position);
            World.TrySetItem(position, data);
            var neighbors = new[]
            {
                new PointI(1, 0, 0), new PointI(-1, 0, 0),
                new PointI(0, 1, 0), new PointI(0, -1, 0),
                new PointI(0, 0, 1), new PointI(0, 0, -1)
            };
            
            for (var i = 0; i < neighbors.Length; i++)
            {
                var point = position.Add(neighbors[i]);
                var r = World.TrySetItem(point, visualizer.UpdateOne(point));
                Console.Error.WriteLine(r);
            }

            var (cPosition, ePosition) = World.Translate2LocalNotation(position);
            var a = cPosition.Add(new PointI(0, ePosition.Y / 16, 0));
            World[cPosition].AdaptToStupidData(ePosition.Y / 16);
            Ready.Enqueue(a);
        }
    }
}