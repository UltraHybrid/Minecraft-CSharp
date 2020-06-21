using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using tmp.Domain;
using tmp.Infrastructure.SimpleMath;
using Shader = tmp.Shaders.Shaders;

namespace tmp.Rendering
{
    public class Lines
    {
        private int vbo, vao;
        private readonly int shaderProgram, vPMatrixLocation;
        private IMover viewer;

        public Lines(IMover viewer)
        {
            coords = a;
            this.viewer = viewer;
            shaderProgram = Shader.GetLineShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
            SendData();
        }

        public void Render(Matrix4 viewProjectionMatrix, float higth)
        {
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.LineWidth(3);
            var front = viewer.Front.Convert();
            var position = viewer.Position.Convert();
            SetVPMatrix(Matrix4.CreateTranslation(position + front + new Vector3(0, higth, 0)) * viewProjectionMatrix);
            GL.DrawArrays(PrimitiveType.Lines, 0, coords.Count / 2);
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        private void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        private void SendData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * coords.Count,
                coords.ToArray(), BufferUsageHint.StaticDraw);
        }

        private void GenBuffers()
        {
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
        }

        private void InstallAttributes()
        {
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes * 2, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes * 2,
                Vector3.SizeInBytes);
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);
        }

        private List<Vector3> coords;

        private List<Vector3> a = new List<Vector3>
        {
            //x - red
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0.1f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            //y - green
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0.1f, 0f),
            new Vector3(0f, 1f, 0f),
            //z - blue
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 0.1f),
            new Vector3(0f, 0f, 1f),
        };

        public void lines()
        {
            coords = a;
            SendData();
        }

        public void Add(Parallelogram p)
        {
            coords = new List<Vector3>
            {
                p.p0.Convert(),
                new Vector3(1, 0, 0),
                p.p1.Convert(),
                new Vector3(1, 0, 0),

                p.p1.Convert(),
                new Vector3(1, 0, 0),
                p.p2.Convert(),
                new Vector3(1, 0, 0),

                p.p2.Convert(),
                new Vector3(1, 0, 0),
                p.p3.Convert(),
                new Vector3(1, 0, 0),

                p.p3.Convert(),
                new Vector3(1, 0, 0),
                p.p0.Convert(),
                new Vector3(1, 0, 0),
            };
            
            SendData();
        }
    }
}