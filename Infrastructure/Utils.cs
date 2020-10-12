using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Infrastructure
{
    public static class Utils
    {
        public static IEnumerable<PointI> TripleFor(int xCount, int yCount, int zCount)
        {
            for (var i = 0; i < xCount; i++)
            for (var j = 0; j < yCount; j++)
            for (var k = 0; k < zCount; k++)
                yield return new PointI(i, j, k);
        }

        public static IEnumerable<(int, int)> DualFor(int xCount, int zCount)
        {
            for (var i = 0; i < xCount; i++)
            for (var k = 0; k < zCount; k++)
                yield return (i, k);
        }
    }
}