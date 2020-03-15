using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace tmp
{
    public class Cube : Volume
    {
        private Vector3 position;
        private List<Vector3> _vertexes;
        private int[] _indices;
        
        public Cube(Vector3 position = default)
        {
            this.position = position;
            VertCount = 8;
            IndiceCount = 24;
            
            _vertexes = new List<Vector3>
            { 
                new Vector3(0f, 1f,  1f) + position,
                new Vector3(1f, 1f,  1f) + position,
                new Vector3(1f, 0f,  1f) + position,
                new Vector3(0f, 0f,  1f) + position,
                new Vector3(0f, 0f, 0f) + position,
                new Vector3(1f, 0f, 0f) + position,
                new Vector3(0f, 1f, 0f) + position,
                new Vector3(1f, 1f, 0f) + position, 
            };

            _indices =  new[]{
                //front
                0, 1, 2,
                2, 3, 0,
                //bot
                2, 3, 4,
                4, 5, 2,
                //back
                4, 6, 7,
                7, 5, 4,
                //left
                0, 3, 4,
                4, 6, 0,
                //right
                1, 7, 5,
                5, 2, 1,
                //top
                0, 6, 7,
                7, 1, 0
            };
        }

        public override List<Vector3> GetVertexes() => _vertexes;

        public override int[] GetIndices(int offset = 0)
        {
            if (offset == 0) return _indices;
            for (var i = 0; i < _indices.Length; i++)
                _indices[i] += offset;

            return _indices;
        }
        
        private Vector3[] _colors = 
        {
            new Vector3(1f, 0f, 0f),
            new Vector3( 0f, 0f, 1f), 
            new Vector3( 0f,  1f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3( 0f, 0f, 1f), 
            new Vector3( 0f,  1f, 0f),
            new Vector3(1f, 0f, 0f),
        };

        public override Vector3[] GetColorData() => _colors;

        public override void CalculateModelMatrix() => Matrix4.CreateTranslation(position);
    }
}