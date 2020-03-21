﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace tmp
{
    public static class Texture
    {
        public static int GetTexture(string name)
        {
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            SetImage(name, TextureTarget.Texture2D);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            SetTextureParameters(TextureTarget.Texture2D, (int)ArbTextureMirrorClampToEdge.MirrorClampToEdge, (int) TextureMagFilter.Nearest);
            
            
            GL.BindTexture(TextureTarget.Texture2D, 0);
            return texture;
        }

        public static int GetCubeMap(List<string> paths)
        {
            var texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            
            if(paths.Count != 6) throw new Exception("wrong count of textures for cubemap");

            for (var i = 0; i < 6; i++)
            {
                Console.WriteLine(paths[i]);
                SetImage(paths[i], TextureTarget.TextureCubeMapPositiveX + i);
            }

            
            SetTextureParameters(TextureTarget.TextureCubeMap, (int)ArbTextureMirrorClampToEdge.MirrorClampToEdge, (int) TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            return texture;
        }

        private static void SetImage(string name, TextureTarget textureTarget)
        {
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

            GL.TexImage2D(textureTarget, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 
                0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        }

        private static void SetTextureParameters(TextureTarget textureTarget, int paramWrap, int paramFilter)
        {
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, paramWrap);
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, paramWrap);
            GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, paramWrap);
            GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, paramFilter);
            GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, paramFilter);
        }
        
        
        //3dtextures/2darray

        private static void InitArray(List<string> path, int count)
        {
            const int width = 16;
            const int height = 16;
            var laysersCount = count;

            
            var texturesArray = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, texturesArray);
            var finalPixels = new byte[256,laysersCount];
            
            
            var pixelAll = new List<byte[]>();

            foreach (var img in path)
            {
                var pixels = new List<byte>();
                var image = Image.Load(img);
                image.Mutate(x => x.Flip(FlipMode.Vertical));
                var tempPixels = image.GetPixelSpan().ToArray();


                foreach (var p in tempPixels)
                {
                    pixels.Add(p.R);
                    pixels.Add(p.G);
                    pixels.Add(p.B);
                    pixels.Add(p.A);
                }
                pixelAll.Add(pixels.ToArray());
            }

            for (var i = 0; i < pixelAll.Count; i++)
            {
                var t = pixelAll[i];
                for (var j = 0; j < t.Length; j++)
                {
                    finalPixels[i, j] = t[j];
                }
            }

            GL.TexImage3D(TextureTarget.Texture2DArray,  0, PixelInternalFormat.Rgba, width, height, laysersCount, 0,
                PixelFormat.Rgb, PixelType.UnsignedByte, finalPixels);
        }
    }
}