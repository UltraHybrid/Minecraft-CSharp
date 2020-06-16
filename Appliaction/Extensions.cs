using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace tmp
{
    public static class Extensions
    {
        public static Vector3 Convert(this PointI point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static void Print(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static (List<Vector3> positions, List<uint> indexData) AdaptToStupidData(
            this Chunk<VisualizerData> chunk)
        {
            var vertex = new List<Vector3>();
            var indexData = new List<uint>();
            uint count = 0;
            foreach (var visData in chunk.Where(x => x != null))
            {
                
                foreach (var face in visData.Faces)
                {
                    var position = visData.Position.Convert();
                    foreach (var v in Cube.GetVertexes()[face.Number])
                    {
                        vertex.Add(v + position);
                    }

                    indexData.AddRange(Cube.GetSideIndices().Select(e => e + count));
                    count += 6;
                }
            }

            return (vertex, indexData);
        }


        public static Vector3 Convert(this Vector vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}