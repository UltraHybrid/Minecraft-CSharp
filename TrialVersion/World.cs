using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace tmp.TrialVersion
{
    public class World : IEnumerable<Chunk>
    {
        private Chunk[,,] chunks;
        private const int maxCount = 5;

        public World()
        {
            chunks = new Chunk[maxCount, maxCount, maxCount];
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            return chunks.Cast<Chunk>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool CheckBounds(int x, int y, int z)
        {
            return 0 <= x && x < maxCount && 0 <= y && y < maxCount && 0 <= z && z < maxCount;
        }

        public Chunk this[int x, int y, int z]
        {
            get
            {
                if (!CheckBounds(x, y, z))
                    throw new ArgumentException();
                return chunks[x, y, z];
            }
            set
            {
                if (!CheckBounds(x, y, z))
                    throw new ArgumentException();
                chunks[x, y, z] = value;
            }
        }
    }
}