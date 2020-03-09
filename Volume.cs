using OpenTK;
using OpenTK.Graphics;

namespace tmp
{
    public abstract class Volume
    {
        public Vector3 Position = Vector3.Zero;
        public int VertCount;
        public int IndiceCount;
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatix = Matrix4.Identity;

        public abstract int[] GetVerts();
        public abstract int[] GetIndices(int offset = 0);
        public abstract Vector3[] GetColorData();
        public abstract void CalculateModelMatrix();
    }
}