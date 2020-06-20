﻿using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class Piece
    {
        public readonly PointL Center;
        public readonly int Radius;
        private readonly IWorld<Chunk<Block>, Block> world;

        public Piece(IWorld<Chunk<Block>, Block> world, PointL center, int radius)
        {
            this.world = world;
            Center = center;
            Radius = radius;
        }

        public bool ContainsPosition(PointL position)
        {
            return Center.GetDistance(position) <= Radius;
        }

        public Block GetItem(PointL position)
        {
            if (!ContainsPosition(position))
                return Block.Either;
            return world.GetItem(position);
        }
    }
}