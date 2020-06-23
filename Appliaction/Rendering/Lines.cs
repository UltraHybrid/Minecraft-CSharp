using System.Collections.Generic;
using System.Linq;
using MinecraftSharp.Domain;
using MinecraftSharp.Infrastructure.SimpleMath;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Shader = MinecraftSharp.Shaders.Shaders;

namespace MinecraftSharp.Rendering
{
    public class Lines : IRender
    {
        private readonly int[] vbo, vao;
        private readonly int shaderProgram, vPMatrixLocation;
        private readonly IMover viewer;
        private readonly Vector3 offset;
        private Vector3[] aim;
        private List<Vector3> axes;

        public Lines(Game game)
        {
            vbo = new int[2];
            vao = new int[2];
            aim = Aimm(1, 1);
            axes = new List<Vector3>();
            offset = game.Player.Height* Vector3.UnitY;
            viewer = game.Player.Mover;
            shaderProgram = Shaders.Shaders.GetLineShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
            SendData();
        }

        public void Render(Matrix4 viewProjectionMatrix)
        {
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao[0]);
            GL.LineWidth(3);
            var front = viewer.Front.Convert();
            var position = viewer.Position.Convert();
            

            if (aim.Length != 0)
            {
                GL.BindVertexArray(vao[1]);
                var m = Matrix4.Identity;
                SetVPMatrix(m);
                GL.DrawArrays(PrimitiveType.Lines, 0, aim.Length / 2);
            }
            else
            {
                SetVPMatrix(Matrix4.CreateTranslation(position + front + offset) * viewProjectionMatrix);
                GL.DrawArrays(PrimitiveType.Lines, 0, axes.Count / 2);
            }
        }

        public void Update()
        {
        }

        private void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        private void SendData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * axes.Count,
                axes.ToArray(), BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * aim.Length,
                aim, BufferUsageHint.StaticDraw);

        }

        private void GenBuffers()
        {
            GL.GenVertexArrays(2, vao);
            GL.GenBuffers(2, vbo);
        }

        private void InstallAttributes()
        {
            for (var i = 0; i < 2; i++)
            {
                GL.BindVertexArray(vao[i]);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[i]);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes * 2, 0);
                GL.EnableVertexAttribArray(0);

                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes * 2,
                    Vector3.SizeInBytes);
                GL.EnableVertexAttribArray(1);

                GL.BindVertexArray(0);
            }
        }

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

        public void Axes()
        {
            aim = new Vector3[]{};
            axes = a;
            SendData();
        }

        public void Aim(int h, int w)
        {
            aim = Aimm(h, w);
            axes = new List<Vector3>();
            SendData();
        }

        private Vector3[] Aimm(int w, int h) => new [] {
            new Vector3(-0.01f, 0, 0),
            new Vector3(1, 1, 1),
            new Vector3(0.01f, 0, 0),
            new Vector3(1, 1, 1),
            
            new Vector3(0,(float)w/h*-0.01f, 0),
            new Vector3(1, 1, 1),
            new Vector3(0, (float)w/h*0.01f, 0),
            new Vector3(1, 1, 1),
        };

        private Vector3[] Get(Parallelogram p)
        {
            return new[]
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
        }
    }
}