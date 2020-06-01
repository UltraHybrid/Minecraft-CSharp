using System.Collections.Generic;
using System.Linq;
using Assimp;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SharpDX.DXGI;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace tmp
{
    public class Mesh
    {
        private AssimpContext assimpContext = new AssimpContext();
        private const PostProcessSteps Flags =  PostProcessSteps.FlipUVs | PostProcessSteps.Triangulate;
        private DefaultShader ds;
        
        List<Vector3> vert = new List<Vector3>();
        List<uint> ind = new List<uint>();
        List<Vector2> texCords = new List<Vector2>();
        private int texture;
        public Mesh(string file, DefaultShader ds)
        {
            this.ds = ds;
            var scene = assimpContext.ImportFile(file, Flags);
            foreach (var mesh in scene.Meshes)
            {
                var size = (uint)vert.Count;
                texCords.AddRange(mesh.TextureCoordinateChannels[0].Select(f => new Vector2(f.X, f.Y)));
                vert.AddRange(mesh.Vertices.Select(c => new Vector3(c.X, c.Y, c.Z)));
                ind.AddRange(mesh.GetUnsignedIndices().Select(e => e + size));


                var material = scene.Materials[mesh.MaterialIndex];
                var textures = material.GetMaterialTextures((TextureType.Diffuse));
                var color = material.ColorDiffuse;
            }

            texture = Texture.InitTextureFromFile("./1/g.jpg");

            ds.SendData(vert.ToArray(), texCords.ToArray(), ind.ToArray());
        }

        public void Render(Matrix4 vpm)
        {
            ds.Use();
            ds.BindVao();
            ds.SetVPMatrix(Matrix4.CreateScale(0.1f) * Matrix4.CreateTranslation(600, 150, 600) * vpm);
            //GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.DrawElements(BeginMode.Triangles, ind.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
    
    public static class Extension
    {
        public static float[] Razvert(this Vector3D vector) => new[] {vector.X, vector.Y, vector.Z};
    }
}