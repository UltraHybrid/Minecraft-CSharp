using System;
using System.Collections.Generic;
using System.Linq;
using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class Visualizer : IVisualizer<Block>
    {
        private readonly World<Chunk<Block>, Block> gameWorld;

        public Visualizer(World<Chunk<Block>, Block> gameWorld)
        {
            this.gameWorld = gameWorld;
        }

        public VisualChunk Visualize(Chunk<Block> worldChunk)
        {
            var result = new VisualChunk(worldChunk.Position);
            for (byte i = 0; i < Chunk<Block>.XLength; i++)
            {
                for (int j = 0; j < Chunk<Block>.YLength; j++)
                {
                    for (byte k = 0; k < Chunk<Block>.ZLength; k++)
                    {
                        var point = new PointB(i, (byte) j, k);
                        var block = worldChunk[point];
                        if (block != null)
                            result[point] =
                                ChooseBordersWithEmpty(
                                    World<Chunk<Block>, Block>.GetAbsolutePosition(point, worldChunk.Position));
                    }
                }
            }

            return result;
        }

        private VisualizerData ChooseBordersWithEmpty(PointI position)
        {
            var currentBTextures = gameWorld.GetItem(position).BlockType.Textures.GetOrderedTextures();
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

                var neighboringBlock = gameWorld.GetItem(position.Add(offset));

                if (neighboringBlock == null || neighboringBlock == Block.Either)
                    result.Add(new FaceData(currentBTextures[i], i, 15));
            }

            return result.Count != 0 ? new VisualizerData(position, result) : null;
        }

        public void UpdateOne(PointI position)
        {
            throw new System.NotImplementedException();
        }
    }
}