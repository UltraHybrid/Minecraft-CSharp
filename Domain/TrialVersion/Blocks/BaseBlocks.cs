using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public static class BaseBlocks
    {
        public static readonly BlockType Dirt = new BlockType("Dirt", 3, TextureInfo.CreateSolid("dirt.png"));
        public static readonly BlockType Glass = new BlockType("Glass", 1, TextureInfo.CreateSolid("glass.png"));
        public static readonly BlockType Stone = new BlockType("Stone", 10, TextureInfo.CreateSolid("stone.png"));
        public static readonly BlockType Sand = new BlockType("Sand", 2, TextureInfo.CreateSolid("sand.png"));
        public static readonly BlockType CoalOre = new BlockType("CoalOre", 5, TextureInfo.CreateSolid("coal_ore.png"));

        public static readonly BlockType Cobblestone =
            new BlockType("Cobblestone", 10, TextureInfo.CreateSolid("cobblestone.png"));

        public static readonly BlockType Bedrock =
            new BlockType("Bedrock", int.MaxValue, TextureInfo.CreateSolid("bedrock.png"));

        public static readonly BlockType GrassPath = new BlockType("GrassPath", 3,
            TextureInfo.CreateWithTopAndBottom("grass_path_top.png", "dirt.png", "grass_path_side.png"));

        public static readonly BlockType Grass = new BlockType("Grass", 3,
            TextureInfo.CreateWithTopAndBottom("grass_top.png", "dirt.png", "grass_block_side.png"));

        public static readonly BlockType OakLog = new BlockType("OakLog", 4,
            TextureInfo.CreateWithTopAndBottom("oak_log_top.png", "oak_log_top.png", "oak_log.png"));

        public static readonly BlockType Snow = new BlockType("Snow", 2, TextureInfo.CreateSolid("snow.png"));

        private static readonly List<BlockType> allBlocks;

        public static IReadOnlyList<BlockType> AllBlocks => allBlocks;

        static BaseBlocks()
        {
            allBlocks = typeof(BaseBlocks).GetFields()
                .Select(field => (BlockType) field.GetValue(new object()))
                .ToList();
        }
    }
}