using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
using ArbTextureMirroredRepeat = OpenTK.Graphics.OpenGL.ArbTextureMirroredRepeat;

namespace tmp
{
    public static class Texture
    {
        public static int GetTexture(string name) => GetTex(name);


        private static int GetTex(string name)
        {
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            //GL.CreateTextures(TextureTarget.Texture2D, 1, out int texture);
            
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
            //GL.TextureStorage2D(texture, 1, SizedInternalFormat.Rgba8, image.Width, image.Height);
            //GL.TextureSubImage2D(texture, 0, 0, 0, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)ArbTextureMirrorClampToEdge.MirrorClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)ArbTextureMirrorClampToEdge.MirrorClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)ArbTextureMirrorClampToEdge.MirrorClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);
            return texture;
        }
        
    }
}