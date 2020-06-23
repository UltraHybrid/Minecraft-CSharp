using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MinecraftSharp.Loaders
{
    public static class Texture
    {
        public static Dictionary<string, int> Textures { get; } = new Dictionary<string, int>();
        public static int ArrayTex { get; private set; }

        public static int InitTextureFromFile(string name, bool isReflected = false)
        {
            GL.GenTextures(1, out int texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            var image = Image.Load(name);
            var pixels = SetImage(image, isReflected);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                image.Width, image.Width, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            SetTextureParameters(TextureTarget.Texture2D, (int) ArbTextureMirrorClampToEdge.MirrorClampToEdge,
                (int) TextureMagFilter.Nearest);


            GL.BindTexture(TextureTarget.Texture2D, 0);
            return texture;
        }

        public static int GetCubeMap(List<string> paths)
        {
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);

            if (paths.Count != 6) throw new Exception("wrong count of textures for cubemap");

            for (var i = 0; i < 6; i++)
            {
                var image = Image.Load(paths[i]);
                var pixels = SetImage(image);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                    image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            }


            SetTextureParameters(TextureTarget.TextureCubeMap, (int) ArbTextureMirrorClampToEdge.MirrorClampToEdge,
                (int) TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            return texture;
        }

        private static List<byte> SetImage(Image<Rgba32> image, bool isReflected = false)
        {
            if(!isReflected)
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

            return pixels;
        }

        private static void SetTextureParameters(TextureTarget textureTarget, int paramWrap, int paramFilter)
        {
            GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, paramFilter);
            GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, paramFilter);
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, paramWrap);
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, paramWrap);
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, paramWrap);
        }

        public static int InitCubeMapArray(Dictionary<string, string[]> dataTex)
        {
            const int size = 16;
            var layersCount = dataTex.Keys.Count;
            var cubeMapArray = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMapArray, cubeMapArray);

            GL.TexImage3D(TextureTarget.TextureCubeMapArray, 0, PixelInternalFormat.Rgba, size, size, layersCount * 6, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

            var cubeIndex = 0;
            foreach (var cubeTexInfo in dataTex)
            {
                for (var i = 0; i < 6; i++)
                {
                    var image = Image.Load(Path.Combine("Textures", cubeTexInfo.Value[i]));
                    var pixels = SetImage(image);
                    GL.TexSubImage3D(TextureTarget.TextureCubeMapPositiveX + i, 0, 0, 0, cubeIndex, size, size, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
                }

                cubeIndex++;
            }
            SetTextureParameters(TextureTarget.TextureCubeMapArray, (int) ArbTextureMirrorClampToEdge.MirrorClampToEdge,
                (int) TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMapArray);
            return cubeMapArray;
        }

        public static void InitArray(List<string> path)
        {
            const int width = 16;
            const int height = 16;
            var layersCount = path.Count;


            var texturesArray = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, texturesArray);
            var finalPixels = new byte[layersCount, 256 * 4];


            var pixelAll = new List<byte[]>();

            for (var id = 0; id < layersCount; id++)
            {
                var e = path[id];
                var name = Path.GetFileName(e);
                Textures.Add(name ?? throw new Exception("image path does not exist"), id);
                var image = Image.Load(e);

                pixelAll.Add(SetImage(image).ToArray());
                Console.WriteLine($"{name} : {id}");
            }

            for (var i = 0; i < pixelAll.Count; i++)
            {
                var t = pixelAll[i];
                for (var j = 0; j < t.Length; j++)
                {
                    finalPixels[i, j] = t[j];
                }
            }
            
            GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba, width, height, layersCount, 0, PixelFormat.Rgba, PixelType.UnsignedByte, finalPixels);

            SetTextureParameters(TextureTarget.Texture2DArray, (int) ArbTextureMirrorClampToEdge.MirrorClampToEdge, (int) TextureMagFilter.Nearest);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);

            ArrayTex = texturesArray;
        }
    }
}