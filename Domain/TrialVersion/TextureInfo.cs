using System;
using SharpDX;

namespace tmp
{
    public class TextureInfo
    {
        public readonly string Left;
        public readonly string Back;
        public readonly string Right;
        public readonly string Front;
        public readonly string Top;
        public readonly string Bottom;
        public static TextureOrder[] Order;

        public TextureInfo(string left, string back, string right,
            string front, string top, string bottom)
        {
            Left = left;
            Back = back;
            Right = right;
            Front = front;
            Top = top;
            Bottom = bottom;
            Order = new[]
            {
                TextureOrder.Left, TextureOrder.Back, TextureOrder.Right,
                TextureOrder.Top, TextureOrder.Front, TextureOrder.Bottom
            };
        }

        public static TextureInfo CreateWithTopAndBottom(string top, string bottom, string side)
        {
            return new TextureInfo(side, side, side,
                side, top, bottom);
        }

        public static TextureInfo CreateSolid(string texture)
        {
            return new TextureInfo(texture, texture, texture,
                texture, texture, texture);
        }

        public string[] GetOrderedTextures()
        {
            var result = new string[6];
            for (var i = 0; i < Order.Length; i++)
            {
                result[i] = Order[i] switch
                {
                    TextureOrder.Left => Left,
                    TextureOrder.Back => Back,
                    TextureOrder.Right => Right,
                    TextureOrder.Front => Front,
                    TextureOrder.Top => Top,
                    TextureOrder.Bottom => Bottom,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return result;
        }
    }

    public enum TextureOrder
    {
        Left,
        Back,
        Right,
        Front,
        Top,
        Bottom
    }
}