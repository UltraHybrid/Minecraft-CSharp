using OpenTK;
using OpenTK.Graphics.OpenGL4;
using tmp.Domain;
using Shader = tmp.Shaders.Shaders;

namespace tmp.Rendering
{
    public class Lines : IRender
    {
        private int vbo, vao;
        private readonly int shaderProgram, vPMatrixLocation;
        private IMover viewer;
        public Lines(IMover viewer)
        {
            this.viewer = viewer;
            shaderProgram = Shader.GetLineShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
            SendData();
        }
        
        public void Render(Matrix4 viewProjectionMatrix)
        {
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.LineWidth(3);
            var front = viewer.Front.Convert();
            var position = viewer.Position.Convert();
            SetVPMatrix(Matrix4.CreateTranslation(position + front + new Vector3(0, 1.7f, 0)) * viewProjectionMatrix);
            GL.DrawArrays(PrimitiveType.Lines, 0, 6);
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        private void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        private void SendData()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * coords.Length,
                coords, BufferUsageHint.StaticDraw);
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
            
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes * 2, Vector3.SizeInBytes);
            GL.EnableVertexAttribArray(1);
            
            GL.BindVertexArray(0);
        }
        
        private Vector3[] coords = {
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
    }
}