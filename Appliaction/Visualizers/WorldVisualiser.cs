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
            var data = new List<(string, int)>();
            for (var i = 0; i < TextureInfo.Order.Length; i++)
            {
                var element = TextureInfo.Order[i];
                var offset = element switch
                {
                    TextureOrder.Left => new PointI(1, 0, 0),
                    TextureOrder.Back => new PointI(0, 0, 1),
                    TextureOrder.Right => new PointI(-1, 0, 0),
                    TextureOrder.Front => new PointI(0, 0, -1),
                    TextureOrder.Top => new PointI(0, 1, 0),
                    TextureOrder.Bottom => new PointI(0, -1, 0),
                    _ => throw new ArgumentOutOfRangeException()
                };

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if ((isSeeTop && !world.IsCorrectIndex(position.Add(offset))) ||
                    (world.IsCorrectIndex(position.Add(offset)) && world[position.Add(offset)] == null))
                    data.Add((textures[i], i));
            }

            return data.Count != 0 ? new VisualizerData(position, data) : null;
        }
    }

    public class VisualizerData
    {
        public readonly PointI Position;
        public readonly List<(string, int)> TextureNumber;

        public VisualizerData(PointI position, List<(string, int)> textureNumber)
        {
            this.Position = position;
            TextureNumber = textureNumber;
        }
    }
}