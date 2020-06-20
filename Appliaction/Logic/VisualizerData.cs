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
            this.Position = position;
            Faces = faces;
        }
    }

    public class FaceData
    {
        public string Name { get; }
        public int Number { get; }
        public float Luminosity { get; }

        public FaceData(string name, int number, float luminosity)
        {
            Name = name;
            Number = number;
            Luminosity = luminosity;
        }
    }
}