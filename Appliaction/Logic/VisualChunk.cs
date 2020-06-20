using System;
using System.Collections.Generic;
using System.Linq;
using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualChunk : Chunk<VisualizerData>
    {
        private readonly List<float>[] rowData;
        private readonly List<int>[] indices;
        public const int RowDataLevels = 16;
        private const int countInLevel = YLength / RowDataLevels;
        public IEnumerable<float>[] RowData => rowData;
        public IEnumerable<int>[] Indices => indices;

        public VisualChunk(PointI position) : base(position)
        {
            rowData = new List<float>[RowDataLevels];
            indices = new List<int>[RowDataLevels];
        }

        private int offset;

        public void AdaptToStupidData(int number)
        {
            if (number >= RowDataLevels || number < 0)
                throw new IndexOutOfRangeException("Некорректный индекс " + number);
            rowData[number] = new List<float>();
            indices[number] = new List<int>();
            for (var x = 0; x < XLength; x++)
            {
                for (var z = 0; z < ZLength; z++)
                {
                    for (var y = number * RowDataLevels; y < number * RowDataLevels + countInLevel; y++)
                    {
                        var vData = this[new PointI(x, y, z).AsPointB()];
                        if (vData == null) continue;
                        foreach (var face in vData.Faces)
                        {
                            rowData[number].AddRange(face.Vertex);
                            indices[number].AddRange(face.Indices.Select(ex => ex + offset));
                            offset += 4;
                        }
                    }
                }
            }
        }
    }
}