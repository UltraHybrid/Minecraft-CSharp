using System;

namespace tmp.TrialVersion
{
    public static class BaseBlocks
    {
        public static readonly BlockItem Empty = new BlockItem("Empty", 0, null);
        public static readonly BlockItem Dirt = new BlockItem("Dirt", 1, "dirt.png");
        public static readonly BlockItem Bedrock = new BlockItem("Bedrock", int.MaxValue, "bedrock.png");
    }
}