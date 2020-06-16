﻿using System.Collections.Generic;
 using OpenTK;

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

    public class RevisedData
    {
        public List<Vector3> Positions { get; }
        public List<uint> TexturesData { get; }

        public RevisedData(List<Vector3> positions, List<uint> texturesData)
        {
            Positions = positions;
            TexturesData = texturesData;
        }
    }
}