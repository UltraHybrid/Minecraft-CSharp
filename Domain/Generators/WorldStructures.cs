using System;
using System.Collections.Generic;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public static class WorldStructures
    {
        public class Tree
        {
            public static readonly List<PointI> TrunkCoordinates;
            public static List<PointI> TreeCrownCoordinates;
            public static BlockType TrunkMaterial = BaseBlocks.OakLog;
            public static BlockType CrownMaterial = BaseBlocks.OakLeaves;
            public static PointI ShiftToCenter = new PointI(-2,0,-2);

            static Tree()
            {
                TrunkCoordinates = new List<PointI>();
                for (var y = 0; y < 6; y++)
                {
                    TrunkCoordinates.Add(new PointI(2, y, 2));
                }

                TreeCrownCoordinates = new List<PointI>();

                for (var y = 2; y < 4; y++)
                {
                    for (var x = 0; x < 5; x++)
                    for (var z = 0; z < 5; z++)
                    {
                        if (x == 2 && z == 2)
                            continue;
                        TreeCrownCoordinates.Add(new PointI(x, y, z));
                    }
                }

                for (var y = 4; y < 6; y++)
                {
                    for (var x = 1; x < 4; x++)
                    for (var z = 1; z < 4; z++)
                    {
                        if (x == 2 && z == 2)
                            continue;
                        TreeCrownCoordinates.Add(new PointI(x, y, z));
                    }
                }

                TreeCrownCoordinates.Add(new PointI(2, 6, 2));
            }
        }
    }
}