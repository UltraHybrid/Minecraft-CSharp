using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace tmp
{
    public class Texture
    {
        private int texture1;
        private int texture2;

        public int GetTexture1() => InitTexture("./Textures/sand.png");
        public int GetTexture2() => InitTexture("./Textures/cobblestone.png");


        private int InitTexture(string name)
        {
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            var image = Image.Load(name);
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            var tempPixels = image.GetPixelSpan().ToArray();
            
            var pixels = new List<byte>();

            foreach (var p in tempPixels)
            {
                pixels.Add(p.R);
                pixels.Add(p.G);
                pixels.Add(p.B);
                pixels.Add(p.A);
            }
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return texture;
        }
        
    }
}