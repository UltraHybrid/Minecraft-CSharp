using OpenTK;

namespace tmp
{
    public class Cube : Volume
    {
        private (int, int, int) position;
        private int[] verts;
        private int[] indices;
        
        public Cube((int x, int y, int z) position = default)
        {
            this.position = position;
            VertCount = 8;
            IndiceCount = 24;
            
            verts = new[]
            {
                -1, -1, -1,
                0, -1, -1,
                0, 0, -1,
                -1, 0, -1,
                -1, 0, 0,
                0, 0, 0,
                0, -1, 0,
                -1, -1, 0
            };

            indices = new[]
            {
                0, 1, 2, 3,
                2, 3, 4, 5,
                0, 3, 4, 7,
                4, 5, 6, 7,
                1, 2, 5, 6,
                0, 1, 6, 7
            };
        }

        public override int[] GetVerts() => verts;

        public override int[] GetIndices(int offset = 0)
        {
            if (offset == 0) return indices;
            for (var i = 0; i < indices.Length; i++)
                indices[i] += offset;

            return indices;
        }

        public override Vector3[] GetColorData() => new[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 1f, 0f),
        };

        public override void CalculateModelMatrix() => Matrix4.CreateTranslation(new Vector3(position.Item1, position.Item2, position.Item3));
    }
}