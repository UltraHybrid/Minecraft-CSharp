using OpenTK;

namespace tmp
{
    public class Cube : Volume
    {
        public Cube()
        {
            VertCount = Vertexes.Length;
            IndiceCount = Indices.Length;
        }
        
        private static readonly Vector3[] Vertexes =
        {
            //left
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),

            //back
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 0f, 1f),

            //right
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 1f),

            //top
            new Vector3(1f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 1f),

            //front
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f),

            //bottom
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, 1f),
            new Vector3(0f, 0f, 1f)
        };
        
        private static readonly Vector3[] TexCords = {
            // left
            new Vector3(0.0f, 0.0f, 0f),
            new Vector3(-1.0f, 1.0f, 0f),
            new Vector3(-1.0f, 0.0f, 0f),
            new Vector3(0.0f, 1.0f, 0f),

            // back
            new Vector3(0.0f, 0.0f, 1f),
            new Vector3(0.0f, 1.0f, 1f),
            new Vector3(-1.0f, 1.0f, 1f),
            new Vector3(-1.0f, 0.0f, 1f),

            // right
            new Vector3(-1.0f, 0.0f, 2f),
            new Vector3(0.0f, 0.0f, 2f),
            new Vector3(0.0f, 1.0f, 2f),
            new Vector3(-1.0f, 1.0f, 2f),

            // top
            new Vector3(0.0f, 0.0f, 3f),
            new Vector3(0.0f, 1.0f, 3f),
            new Vector3(-1.0f, 0.0f, 3f),
            new Vector3(-1.0f, 1.0f, 3f),

            // front
            new Vector3(0.0f, 0.0f, 4f),
            new Vector3(1.0f, 1.0f, 4f),
            new Vector3(0.0f, 1.0f, 4f),
            new Vector3(1.0f, 0.0f, 4f),

            // bottom
            new Vector3(0.0f, 0.0f, 5f),
            new Vector3(0.0f, 1.0f, 5f),
            new Vector3(-1.0f, 1.0f, 5f),
            new Vector3(-1.0f, 0.0f, 5f)
        };
        
        private static readonly int[] Indices = {
            //left
            0, 1, 2, 0, 3, 1,
            //back
            4, 5, 6, 4, 6, 7,
            //right
            8, 9, 10, 8, 10, 11,
            //top
            13, 14, 12, 13, 15, 14,
            //front
            16, 17, 18, 16, 19, 17,
            //bottom 
            20, 21, 22, 20, 22, 23
        };

        public static Vector3[] GetTextureCoords() => TexCords;

        public static Vector3[] GetVertexes() => Vertexes;

        public static int[] GetIndices() => Indices;
    }
}