using System.Collections.Generic;
using OpenTK;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualizerData
    {
        public readonly PointL Position;
        public readonly List<FaceData> Faces;

        public VisualizerData(PointL position, List<FaceData> faces)
        {
            Position = position;
            Faces = faces;
        }
    }

    public class FaceData
    {
        public float[] Vertex { get; }
        public int[] Indices { get; }

        public FaceData(float[] vertex, int[] indices)
        {
            Vertex = vertex;
            Indices = indices;
        }
    }
}