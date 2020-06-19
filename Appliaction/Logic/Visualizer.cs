﻿using System;
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
            var currentBlock = gameWorld.GetItem(position);
            if (currentBlock==null)
                return null;
            
            var currentBTextures = currentBlock.BlockType.Textures.GetOrderedTextures();
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

                if (neighboringBlock == null || neighboringBlock == Block.Either ||
                    neighboringBlock.BlockType == BaseBlocks.Glass || currentBlock.BlockType == BaseBlocks.CoalOre
                    || currentBlock.BlockType==BaseBlocks.OakLeaves || neighboringBlock.BlockType==BaseBlocks.OakLeaves)
                    result.Add(new FaceData(currentBTextures[i], i, TextureInfo.Brightness[element]));
            }

            return result.Count != 0 ? new VisualizerData(position, result) : null;
        }

        public VisualizerData UpdateOne(PointI position)
        {
            var (cPosition, ePosition) = gameWorld.Translate2LocalNotation(position);
            var blockPosition = ePosition.AsPointB();
            return ChooseBordersWithEmpty(position);
        }
    }
}