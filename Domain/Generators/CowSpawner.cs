using System;
using System.Collections.Generic;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Generators
{
    public class CowSpawner : IGenerator<Chunk<Block>, List<PointB>>
    {
        private readonly Random rnd = new Random();

        public List<PointB> Generate(Chunk<Block> source)
        {
            var count = rnd.Next(2);
            var result = new List<PointB>();
            if (count == 0)
                return result;
            Func<PointB> getRndPoint = () => new PointI(rnd.Next(16), 0, rnd.Next(16)).AsPointB();
            for (var i = 0; i < count; i++)
            {
                var point = getRndPoint();
                for (var j = Chunk<Block>.YLength - 3; j >= 0; j--)
                {
                    var position = point.Add(new PointB(0, (byte)j, 0));
                    var position1 = point.Add(new PointB(0, (byte)(j + 1), 0));
                    var position2 = point.Add(new PointB(0, (byte)(j + 2), 0));
                    var block = source[position];
                    if (block != null && block.BlockType == BaseBlocks.Grass &&
                        source[position1] == null &&
                        source[position2] == null)
                        result.Add(position1);
                }
            }

            return result;
        }
    }
}