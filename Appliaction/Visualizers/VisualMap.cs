using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace tmp
{
    public class VisualMap
    {
        public PointI Offset;
        public ConcurrentDictionary<PointI, IReadOnlyList<VisualizerData>> Data;
        public int Size;
        public ConcurrentQueue<PointI> Ready;
        public readonly IVisualizer<Chunk, IEnumerable<VisualizerData>> visualizer;

        public VisualMap(int size, PointI startOffset,
            IVisualizer<Chunk, IEnumerable<VisualizerData>> visualizer)
        {
            Size = size;
            Offset = startOffset;
            this.visualizer = visualizer;
            Data = new ConcurrentDictionary<PointI, IReadOnlyList<VisualizerData>>();
            Ready = new ConcurrentQueue<PointI>();
        }

        public void HandleNewChunk(Chunk chunk)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    var data = visualizer.GetVisibleFaces(chunk);
                    Data[chunk.Position] = data;
                    Ready.Enqueue(chunk.Position);
                    //Extensions.Print("Calculate" + chunk.Position, ConsoleColor.Blue);
                }
                catch (Exception e)
                {
                    //Extensions.Print("Calculate" + chunk.Position, ConsoleColor.Red);
                    Console.WriteLine(e);
                }
            });
            /*while (!task.IsCompleted)
            {
            }

            if (task.Status == TaskStatus.Faulted)
            {
                Extensions.Print("Was Error", ConsoleColor.Red);
            }*/
        }

        private bool CheckBounds(int x, int z)
        {
            return 0 <= x && x < Size && 0 <= z && z < Size;
        }

        public IEnumerable<VisualizerData> this[int x, int z]
        {
            get
            {
                if (!CheckBounds(x, z))
                    throw new ArgumentException();
                return new List<VisualizerData>();
            }
            set
            {
                if (!CheckBounds(x, z))
                    throw new ArgumentException();
            }
        }
    }
}