using System;
using System.Collections.Generic;
using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualChunk : Chunk<VisualizerData>
    {
        public RevisedData SimpleData;
        public List<float>[] RowData;

        public VisualChunk(PointI position) : base(position)
        {
            RowData = new List<float>[16];
        }

        public void AdaptToStupidData(int number)
        {
            RowData[number] = new List<float>();
            for (var x = 0; x < XLength; x++)
            {
                for (var z = 0; z < ZLength; z++)
                {
                    for (var y = number * 16; y < number * 16 + 16; y++)
                    {
                        var block = this[new PointI(x, y, z).AsPointB()];
                        if (block == null) continue;
                        foreach (var face in block.Faces)
                        {
                            RowData[number].Add(Position.X * 16 + x);
                            RowData[number].Add(y);
                            RowData[number].Add(Position.Z * 16 + z);

                            RowData[number].Add(face.Number);
                            RowData[number].Add(Texture.textures[face.Name]);
                            RowData[number].Add(face.Luminosity);
                        }
                    }
                }
            }
        }
    }
}