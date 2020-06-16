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
        
        public static readonly Vector3[][] Vertexes =
        {
            //left
            new[]{
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 0f, 1f)},

            //back
            new[]{
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 1f)},

            //right
            new[]{
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 1f)},

            //top
            new[]{
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 1f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 1f, 0f)},

            //front
            new[]{
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 0f, 0f)},

            //bottom
            new[]{
            new Vector3(1f, 0f, 1f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f)},
        };

        private static readonly Vector3[] TexCords = {
            // left
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 0f, 0f),

            // back
            new Vector3(1f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),

            // right
            new Vector3(1f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),

            // top
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 0f, 0f),

            // front
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 0f, 0f),

            // bottom
            new Vector3(1f, 1f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
        };
        
        private static readonly int[] Indices = {
            //left
            0, 1, 2, 0, 2, 3,
            //back
            4, 5, 6, 4, 7, 6,
            //right
            8, 9, 10, 8, 10, 11,
            //top
            13, 14, 12, 13, 15, 14,
            //front
            16, 17, 18, 16, 19, 17,
            //bottom 
            20, 21, 22, 20, 22, 23
        };

        private static readonly uint[] SideIndices = {0, 1, 2, 0, 2, 3};

        public static Vector3[] GetTextureCoords() => TexCords;
        

        public static Vector3[][] GetVertexes() => Vertexes;
        

        public static int[] GetIndices() => Indices;
        
        public static uint[] GetSideIndices() => SideIndices;
    }
}