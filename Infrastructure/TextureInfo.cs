using System;
using System.Collections.Generic;

namespace tmp.Infrastructure
{
    public class TextureInfo
    {
        public readonly string Left;
        public readonly string Back;
        public readonly string Right;
        public readonly string Front;
        public readonly string Top;
        public readonly string Bottom;
        public readonly HitBox Appearance;

        public static TextureSide[] Order =
        {
            TextureSide.Left, TextureSide.Back, TextureSide.Right,
            TextureSide.Front, TextureSide.Top, TextureSide.Bottom
        };

        public static Dictionary<TextureSide, float> Brightness = new Dictionary<TextureSide, float>()
        {
            {TextureSide.Left, 0.6f},
            {TextureSide.Right, 0.6f},
            {TextureSide.Top, 1.0f},
            {TextureSide.Bottom, 0.5f},
            {TextureSide.Front, 0.8f},
            {TextureSide.Back, 0.8f},
        };

        public TextureInfo(string left, string back, string right,
            string front, string top, string bottom, HitBox appearance)
        {
            Left = left;
            Back = back;
            Right = right;
            Front = front;
            Top = top;
            Bottom = bottom;
            Appearance = appearance;
        }

        public static TextureInfo CreateWithTopAndBottom(string top, string bottom, string side, HitBox appearance)
        {
            return new TextureInfo(side, side, side,
                side, top, bottom, appearance);
        }

        public static TextureInfo CreateSolid(string texture, HitBox appearance)
        {
            return new TextureInfo(texture, texture, texture,
                texture, texture, texture, appearance);
        }

        public string[] GetOrderedTextures()
        {
            var result = new string[6];
            for (var i = 0; i < Order.Length; i++)
            {
                result[i] = Order[i] switch
                {
                    TextureSide.Left => Left,
                    TextureSide.Back => Back,
                    TextureSide.Right => Right,
                    TextureSide.Front => Front,
                    TextureSide.Top => Top,
                    TextureSide.Bottom => Bottom,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return result;
        }
    }

    public enum TextureSide
    {
        Left,
        Back,
        Right,
        Front,
        Top,
        Bottom
    }
}