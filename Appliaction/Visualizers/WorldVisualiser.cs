using System;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class WorldVisualiser : IVisualizer<Chunk, IEnumerable<VisualizerData>>
    {
        private readonly World world;

        public WorldVisualiser(World world)
        {
            this.world = world;
        }

        public IReadOnlyList<VisualizerData> GetVisibleFaces(Chunk chunk)
        {
            return chunk
                .Where(b => b != null)
                .Select(b => world.GetAbsolutePosition(b, chunk.Position))
                .Select(GetVisibleFaces)
                .Where(v => v != null)
                .ToList();
        }

        private VisualizerData GetVisibleFaces(PointI position)
        {
            var textures = world[position].BlockType.Textures.GetOrderedTextures();
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            var isSeeTop = world.IsCorrectIndex(position.Add(new PointI(0, 1, 0))) &&
                           world[position.Add(new PointI(0, 1, 0))] == null;
            var data = new List<FaceData>();
            for (var i = 0; i < TextureInfo.Order.Length; i++)
            {
                var element = TextureInfo.Order[i];
                var offset = element switch
                {
                    TextureSide.Left => new PointI(1, 0, 0),
                    TextureSide.Back => new PointI(0, 0, 1),
                    TextureSide.Right => new PointI(-1, 0, 0),
                    TextureSide.Front => new PointI(0, 0, -1),
                    TextureSide.Top => new PointI(0, 1, 0),
                    TextureSide.Bottom => new PointI(0, -1, 0),
                    _ => throw new ArgumentOutOfRangeException()
                };

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if ((isSeeTop && !world.IsCorrectIndex(position.Add(offset))) ||
                    (world.IsCorrectIndex(position.Add(offset)) && world[position.Add(offset)] == null))
                    data.Add(new FaceData(textures[i], i, 15));
            }

            return data.Count != 0 ? new VisualizerData(position, data) : null;
        }
    }

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