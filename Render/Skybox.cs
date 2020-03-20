using System.IO;
using System.Linq;
using OpenTK.Graphics.ES20;

namespace tmp
{
    public class Skybox
    {
        private int skyBoxShaderProgram;
        private int vao;
        private int vbo;
        private int texture;

        public int InitTextures(string textureStorage)
        {
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            return Texture.GetCubeMap(Directory.GetFiles(textureStorage, "*.png").ToList());
        }
    }
}