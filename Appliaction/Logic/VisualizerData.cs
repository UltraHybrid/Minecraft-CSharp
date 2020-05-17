﻿using System.Collections.Generic;

namespace tmp
{
    public class VisualizerData
    {
        public readonly PointI Position;
        public readonly List<FaceData> Faces;

        public VisualizerData(PointI position, List<FaceData> faces)
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