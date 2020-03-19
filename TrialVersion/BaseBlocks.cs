using System;

namespace tmp.TrialVersion
{
    public static class BaseBlocks
    {
        public static readonly BlockItem Empty = new BlockItem("Empty", 0, null);
        public static readonly BlockItem Dirt = new BlockItem("Dirt", 3, "dirt.png");
        public static readonly BlockItem Glass = new BlockItem("Glass", 1, "glass.png");
        public static readonly BlockItem Stone = new BlockItem("Stone", 10, "stone.png");
        public static readonly BlockItem Sand = new BlockItem("Sand", 2, "sand,png");
        public static readonly BlockItem CoalOre = new BlockItem("CoalOre", 5, "coal_ore.png");
        public static readonly BlockItem Cobblestone = new BlockItem("Cobblestone", 10, "cobblestone.png");
        public static readonly BlockItem Bedrock = new BlockItem("Bedrock", int.MaxValue, "bedrock.png");
    }
}