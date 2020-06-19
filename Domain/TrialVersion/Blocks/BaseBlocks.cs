using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.TrialVersion.Blocks
{
    public static class BaseBlocks
    {
        public static readonly BlockType Dirt =
            new BlockType("Dirt", 3, BlockCore.Land,
                TextureInfo.CreateSolid("dirt.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType Glass =
            new BlockType("Glass", 1, BlockCore.Transparent,
                TextureInfo.CreateSolid("glass.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType Stone =
            new BlockType("Stone", 10, BlockCore.Rock,
                TextureInfo.CreateSolid("stone.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType Sand =
            new BlockType("Sand", 2, BlockCore.Granular,
                TextureInfo.CreateSolid("sand.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType CoalOre =
            new BlockType("CoalOre", 5, BlockCore.Ore,
                TextureInfo.CreateSolid("coal_ore.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType IronOre =
            new BlockType("IronOre", 8, BlockCore.Ore,
                TextureInfo.CreateSolid("iron_ore.png", Geometry.Unit),
                Basis.UnitBasis);

        public static readonly BlockType GoldOre =
            new BlockType("GoldOre", 10, BlockCore.Ore,
                TextureInfo.CreateSolid("gold_ore.png", Geometry.Unit),
                Basis.UnitBasis);

        public static readonly BlockType DiamondOre =
            new BlockType("DiamondOre", 15, BlockCore.Ore,
                TextureInfo.CreateSolid("diamond_ore.png", Geometry.Unit),
                Basis.UnitBasis);

        public static readonly BlockType Cobblestone =
            new BlockType("Cobblestone", 10, BlockCore.Rock,
                TextureInfo.CreateSolid("cobblestone.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType Bedrock =
            new BlockType("Bedrock", int.MaxValue, BlockCore.Rock,
                TextureInfo.CreateSolid("bedrock.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType GrassPath =
            new BlockType("GrassPath", 3, BlockCore.Land,
                TextureInfo.CreateWithTopAndBottom("grass_path_top.png", "dirt.png", "grass_path_side.png",
                    Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType Grass =
            new BlockType("Grass", 3, BlockCore.Land,
                TextureInfo.CreateWithTopAndBottom("grass_top.png", "dirt.png", "grass_block_side.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType OakLog =
            new BlockType("OakLog", 4, BlockCore.Wood,
                TextureInfo.CreateWithTopAndBottom("oak_log_top.png", "oak_log_top.png", "oak_log.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType OakLeaves =
            new BlockType("OakLeaves", 1, BlockCore.Transparent,
                TextureInfo.CreateSolid("oak_leaves.png", Geometry.Unit),
                Basis.UnitBasis
            );

        public static readonly BlockType Snow =
            new BlockType("Snow", 2, BlockCore.Other,
                TextureInfo.CreateSolid("snow.png", Geometry.Unit),
                Basis.UnitBasis
            );

        //public static readonly BlockType Fence =
        //    new BlockType("Fence", 3, BlockCore.Wood,
        //        TextureInfo.CreateWithTopAndBottom("", "", "", new HitBox(0.25f, 1f, 0.25f, Basis.UnitBasis)),
        //        new Basis(PointF.Zero, 0.25f * Vector3.UnitX, 1.5f * Vector3.UnitY, 0.25f * Vector3.UnitZ));

        public static readonly BlockType Corpuscle =
            new BlockType("", int.MaxValue, BlockCore.Other,
                null,
                Basis.UnitBasis
            );

        private static readonly List<BlockType> allBlocks;

        public static IReadOnlyList<BlockType> AllBlocks => allBlocks;

        static BaseBlocks()
        {
            allBlocks = typeof(BaseBlocks).GetFields()
                .Select(field => (BlockType) field.GetValue(new object()))
                .Where(x => !x.Equals(Corpuscle))
                .ToList();
        }
    }
}