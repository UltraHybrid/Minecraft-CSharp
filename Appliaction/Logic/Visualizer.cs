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
            for (var i = 0; i < Chunk<Block>.XLength; i++)
            {
                for (var j = 0; j < Chunk<Block>.YLength; j++)
                {
                    for (var k = 0; k < Chunk<Block>.ZLength; k++)
                    {
                        var point = new PointI(i, j, k).AsPointB();
                        result[point] = ChooseBordersWithEmpty(
                            World<Chunk<Block>, Block>.GetAbsolutePosition(point, worldChunk.Position));
                    }
                }
            }

            return result;
        }

        private VisualizerData ChooseBordersWithEmpty(PointL position)
        {
            var currentBlock = gameWorld.GetItem(position);
            if (currentBlock == null)
                return null;

            var currentBTextures = currentBlock.BlockType.Textures.GetOrderedTextures();
            var currentCore = currentBlock.BlockType.Core;
            var result = new List<FaceData>();
            for (var i = 0; i < TextureInfo.Order.Length; i++)
            {
                var element = TextureInfo.Order[i];
                if (currentCore == BlockCore.Ore || currentCore == BlockCore.Transparent)
                {
                    result.Add(new FaceData(currentBTextures[i], i, TextureInfo.Brightness[element]));
                    continue;
                }

                var offset = element switch
                {
                    TextureSide.Left => new PointL(1, 0, 0),
                    TextureSide.Back => new PointL(0, 0, 1),
                    TextureSide.Right => new PointL(-1, 0, 0),
                    TextureSide.Front => new PointL(0, 0, -1),
                    TextureSide.Top => new PointL(0, 1, 0),
                    TextureSide.Bottom => new PointL(0, -1, 0),
                    _ => throw new ArgumentOutOfRangeException()
                };

                var neighboringBlock = gameWorld.GetItem(position.Add(offset));
                
                if (neighboringBlock == null || neighboringBlock == Block.Either ||
                    neighboringBlock.BlockType.Core == BlockCore.Transparent)
                    result.Add(new FaceData(currentBTextures[i], i, TextureInfo.Brightness[element]));
            }

            return result.Count != 0 ? new VisualizerData(position, result) : null;
        }

        public VisualizerData UpdateOne(PointL position)
        {
            return ChooseBordersWithEmpty(position);
        }
    }
}