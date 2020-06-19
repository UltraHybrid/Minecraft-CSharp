using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace tmp.Shaders
{
    public class LineShader
    {
        private int shaderProgram, vPMatrixLocation, vbo, vao;

        public LineShader()
        {
            shaderProgram = Shaders.GetLineShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
            SendData();
        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.LineWidth(1);
        }

        public void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        public void SendData()
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