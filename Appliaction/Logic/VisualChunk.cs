using System;
using System.Collections.Generic;
using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualChunk : Chunk<VisualizerData>
    {
        private readonly List<float>[] rowData;
        public const int RowDataLevels = 16;
        public const int CountInLevel = YLength / RowDataLevels;
        public IEnumerable<float>[] RowData => rowData;

        public VisualChunk(PointI position) : base(position)
        {
            rowData = new List<float>[RowDataLevels];
        }

        public void AdaptToStupidData(int number)
        {
            if (number >= RowDataLevels || number < 0)
                throw new IndexOutOfRangeException("Некорректный индекс " + number);
            rowData[number] = new List<float>();
            for (var x = 0; x < XLength; x++)
            {
                for (var z = 0; z < ZLength; z++)
                {
                    for (var y = number * CountInLevel; y < number * CountInLevel + CountInLevel; y++)
                    {
                        var vData = this[new PointI(x, y, z).AsPointB()];
                        if (vData == null) continue;
                        foreach (var face in vData.Faces)
                        {
                            rowData[number].Add(vData.Position.X);
                            rowData[number].Add(y);
                            rowData[number].Add(vData.Position.Z);

                            rowData[number].Add(face.Number);
                            rowData[number].Add(Texture.Textures[face.Name]);
                            rowData[number].Add(face.Luminosity);
                        }
                    }
                }
            }
        }
    }
}