using System;
using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public class WorldVisualizer3 : IVisualizer<Chunk, IEnumerable<VisualizerData>>
    {
        private readonly World2 world;

        public WorldVisualizer3(World2 world)
        {
            this.world = world;
        }

        public IReadOnlyList<VisualizerData> GetVisibleFaces(Chunk data)
        {
            return data.Where(b => b != null)
                .Select(b => World2.GetAbsolutePosition(b.Position, data.Position))
                .Select(ChooseBordersWithEmpty)
                .Where(x => x != null)
                .ToList();
        }

        private VisualizerData ChooseBordersWithEmpty(PointI position)
        {
            var currentBTextures = world.GetBlock(position).BlockType.Textures.GetOrderedTextures();
            var result = new List<FaceData>();
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
                if (world.GetBlock(position.Add(offset)) == null)
                    // ReSharper disable once HeuristicUnreachableCode
                    result.Add(new FaceData(currentBTextures[i], i, 15));
            }

            return result.Count != 0 ? new VisualizerData(position, result) : null;
        }
    }
}