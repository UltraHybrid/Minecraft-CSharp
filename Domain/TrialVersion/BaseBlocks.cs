using System.Collections.Generic;
using System.Linq;

namespace tmp
{
    public static class BaseBlocks
    {
        public static readonly BlockType Dirt = new BlockType("Dirt", 3, FillOneTexture("dirt.png"));
        public static readonly BlockType Glass = new BlockType("Glass", 1, FillOneTexture("glass.png"));
        public static readonly BlockType Stone = new BlockType("Stone", 10, FillOneTexture("stone.png"));
        public static readonly BlockType Sand = new BlockType("Sand", 2, FillOneTexture("sand.png"));
        public static readonly BlockType CoalOre = new BlockType("CoalOre", 5, FillOneTexture("coal_ore.png"));

        public static readonly BlockType Cobblestone =
            new BlockType("Cobblestone", 10, FillOneTexture("cobblestone.png"));

        public static readonly BlockType Bedrock =
            new BlockType("Bedrock", int.MaxValue, FillOneTexture("bedrock.png"));

        public static readonly BlockType GrassPath = new BlockType("GrassPath", 3,
            new[]
            {
                "grass_path_side.png", "grass_path_side.png", "grass_path_side.png",
                "grass_path_top.png", "grass_path_side.png", "dirt.png"
            });

        public static readonly BlockType Grass = new BlockType("Grass", 3, new[]
        {
            "grass_block_side.png", "grass_block_side.png", "grass_block_side.png",
            "snow.png", "grass_block_side.png", "dirt.png"
        });

        public static readonly BlockType OakLog = new BlockType("OakLog", 4, new[]
        {
            "oak_log.png", "oak_log.png", "oak_log.png",
            "oak_log_top.png", "oak_log.png", "oak_log_top.png"
        });
        
        public static readonly BlockType Snow = new BlockType("Snow", 2, FillOneTexture("snow.png"));

        private static readonly List<BlockType> allBlocks;

        public static IReadOnlyList<BlockType> AllBlocks => allBlocks;

        static BaseBlocks()
        {
            allBlocks = typeof(BaseBlocks).GetFields()
                .Select(field => (BlockType) field.GetValue(new object()))
                .ToList();
        }

        private static string[] FillOneTexture(string name)
        {
            var result = new string[6];
            for (var i = 0; i < 6; i++)
                result[i] = name;
            return result;
        }
    }
}