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

        public IEnumerable<VisualizerData> GetVisibleFaces(Chunk chunk)
        {
            var x = chunk.Position.X;
            var z = chunk.Position.Z;
            return chunk
                .Where(b => b != null)
                .Select(b => world.GetAbsolutPosition(b, x, z))
                .Select(GetVisibleFaces)
                .Where(v => v != null);
        }

        private VisualizerData GetVisibleFaces(PointI position)
        {
            var textures = world[position].BlockType.Textures.GetOrderedTextures();
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
                if (world.IsCorrectIndex(position.Add(offset)) && world[position.Add(offset)] == null)
                    data.Add((textures[i], i));
            }

            return data.Count!=0?new VisualizerData(position, data): null;
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