using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Input;

namespace tmp
{
    public class Cube : Volume
    {
        private Vector3 position;
        private List<Vector3> vertexes;
        private int[] indices;
        private List<Vector2> textureCords;
        private List<int> texId;
        
        public Cube(Vector3 position = default, List<int> texId = null)
        {
            this.texId = texId ?? new int[7].ToList();
            this.position = position;
            VertCount = 24;
            IndiceCount = 36;

            textureCords = new List<Vector2>()
            {
                //front
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
            };
            
            
            indices =  new int[] {
                //left
                0,1,2,0,3,1,

                //back
                4,5,6,4,6,7,

                //right
                8,9,10,8,10,11,

                //top
                13,14,12,13,15,14,

                //front
                16,17,18,16,19,17,

                //bottom 
                20,21,22,20,22,23
            };
        }

        public Vector3[] GetTextureCoords() => new[]
            {
                // left
                new Vector3(0.0f, 0.0f, texId[0]),
                new Vector3(-1.0f, 1.0f, texId[0]),
                new Vector3(-1.0f, 0.0f, texId[0]),
                new Vector3(0.0f, 1.0f, texId[0]),

                // back
                new Vector3(0.0f, 0.0f, texId[1]),
                new Vector3(0.0f, 1.0f, texId[1]),
                new Vector3(-1.0f, 1.0f, texId[1]),
                new Vector3(-1.0f, 0.0f, texId[1]),

                // right
                new Vector3(-1.0f, 0.0f, texId[2]),
                new Vector3(0.0f, 0.0f, texId[2]),
                new Vector3(0.0f, 1.0f, texId[2]),
                new Vector3(-1.0f, 1.0f, texId[2]),

                // top
                new Vector3(0.0f, 0.0f, texId[3]),
                new Vector3(0.0f, 1.0f, texId[3]),
                new Vector3(-1.0f, 0.0f, texId[3]),
                new Vector3(-1.0f, 1.0f, texId[3]),

                // front
                new Vector3(0.0f, 0.0f, texId[4]),
                new Vector3(1.0f, 1.0f, texId[4]),
                new Vector3(0.0f, 1.0f, texId[4]),
                new Vector3(1.0f, 0.0f, texId[4]),

                // bottom
                new Vector3(0.0f, 0.0f, texId[5]),
                new Vector3(0.0f, 1.0f, texId[5]),
                new Vector3(-1.0f, 1.0f, texId[5]),
                new Vector3(-1.0f, 0.0f, texId[5])
            };

        public List<Vector3> GetVertexesWithoutOffset() => GetVertexe(new Vector3(0, 0, 0));
        public List<Vector3> GetVertexes() => GetVertexe(position);
        public List<Vector3> GetVertexe(Vector3 position1=default)
        { 
            return  new List<Vector3> {
                //left
                new Vector3(0f, 0f,  0f) + position1,
                new Vector3(1f, 1f,  0f) + position1,
                new Vector3(1f, 0f,  0f) + position1,
                new Vector3(0f, 1f,  0f) + position1,

                //back
                new Vector3(1f, 0f,  0f) + position1,
                new Vector3(1f, 1f,  0f) + position1,
                new Vector3(1f, 1f,  1f) + position1,
                new Vector3(1f, 0f,  1f) + position1,

                //right
                new Vector3(0f, 0f,  1f) + position1,
                new Vector3(1f, 0f,  1f) + position1,
                new Vector3(1f, 1f,  1f) + position1,
                new Vector3(0f, 1f,  1f) + position1,

                //top
                new Vector3(1f, 1f,  0f) + position1,
                new Vector3(0f, 1f,  0f) + position1,
                new Vector3(1f, 1f,  1f) + position1,
                new Vector3(0f, 1f,  1f) + position1,

                //front
                new Vector3(0f, 0f,  0f) + position1, 
                new Vector3(0f, 1f,  1f) + position1, 
                new Vector3(0f, 1f,  0f) + position1,
                new Vector3(0f, 0f,  1f) + position1,

                //bottom
                new Vector3(0f, 0f,  0f) + position, 
                new Vector3(1f, 0f,  0f) + position,
                new Vector3(1f, 0f,  1f) + position,
                new Vector3(0f, 0f,  1f) + position

            };   
        }

        public int[] GetIndices(int offset = 0)
        {
            if (offset == 0) return indices;
            for (var i = 0; i < indices.Length; i++)
                indices[i] += offset;

            return indices;
        }
        
        public void CalculateModelMatrix() => Matrix4.CreateTranslation(position);
    }
}