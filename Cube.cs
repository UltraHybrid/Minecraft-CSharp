using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace tmp
{
    public class Cube : Volume
    {
        private Vector3 position;
        private List<Vector3> vertexes;
        private int[] indices;
        private List<Vector2> textureCords;
        
        public Cube(Vector3 position = default)
        {
            this.position = position;
            VertCount = 24;
            IndiceCount = 36;
            
            vertexes = new List<Vector3> {
                //left
                new Vector3(0f, 0f,  0f) + position,
                new Vector3(1f, 1f,  0f) + position,
                new Vector3(1f, 0f,  0f) + position,
                new Vector3(0f, 1f,  0f) + position,

                //back
                new Vector3(1f, 0f,  0f) + position,
                new Vector3(1f, 1f,  0f) + position,
                new Vector3(1f, 1f,  1f) + position,
                new Vector3(1f, 0f,  1f) + position,

                //right
                new Vector3(0f, 0f,  1f) + position,
                new Vector3(1f, 0f,  1f) + position,
                new Vector3(1f, 1f,  1f) + position,
                new Vector3(0f, 1f,  1f) + position,

                //top
                new Vector3(1f, 1f,  0f) + position,
                new Vector3(0f, 1f,  0f) + position,
                new Vector3(1f, 1f,  1f) + position,
                new Vector3(0f, 1f,  1f) + position,

                //front
                new Vector3(0f, 0f,  0f) + position, 
                new Vector3(0f, 1f,  1f) + position, 
                new Vector3(0f, 1f,  0f) + position,
                new Vector3(0f, 0f,  1f) + position,

                //bottom
                new Vector3(0f, 0f,  0f) + position, 
                new Vector3(1f, 0f,  0f) + position,
                new Vector3(1f, 0f,  1f) + position,
                new Vector3(0f, 0f,  1f) + position

            };
            
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

        public Vector2[] GetTextureCoords() => new[]
            {
                // left
                new Vector2(0.0f, 0.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
                new Vector2(0.0f, 1.0f),

                // back
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),

                // right
                new Vector2(-1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),

                // top
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 0.0f),
                new Vector2(-1.0f, 1.0f),

                // front
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 0.0f),

                // bottom
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(-1.0f, 1.0f),
                new Vector2(-1.0f, 0.0f)
            };

        public override List<Vector3> GetVertexes() => vertexes;

        public override int[] GetIndices(int offset = 0)
        {
            if (offset == 0) return indices;
            for (var i = 0; i < indices.Length; i++)
                indices[i] += offset;

            return indices;
        }
        
        public override void CalculateModelMatrix() => Matrix4.CreateTranslation(position);
    }
}