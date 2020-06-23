using System;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Generators
{
    public class SimpleTreeSpawner : IGenerator<Chunk<Block>, Chunk<Block>>
    {
        public Chunk<Block> Generate(Chunk<Block> source)
        {
            var rnd = new Random();
            var chance = rnd.Next(3);
            if (chance != 0)
                return source;
            var treePosition = PointI.CreateXZ(rnd.Next(2, 13), rnd.Next(2, 13)).AsPointB();
            var treeRoot = PointI.Zero;
            for (byte y = 255 - 7; y > 0; y--)
            {
                var point = treePosition.Add(new PointB(0, y, 0));
                if (source[point] != null)
                {
                    treeRoot = point.Add(new PointB(0, 1, 0)).AsPointI();
                    break;
                }
            }

            if (treeRoot.Equals(PointI.Zero))
                return source;
            
            treeRoot = treeRoot.Add(WorldStructures.Tree.ShiftToCenter);
            foreach (var coord in WorldStructures.Tree.TrunkCoordinates)
            {
                var point = treeRoot.Add(coord).AsPointB();
                source[point] ??= new Block(WorldStructures.Tree.TrunkMaterial, point);
            }

            foreach (var coord in WorldStructures.Tree.TreeCrownCoordinates)
            {
                var point = treeRoot.Add(coord).AsPointB();
                source[point] ??= new Block(WorldStructures.Tree.CrownMaterial, point);
            }

            return source;
        }
    }
}