using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using SFML.Graphics;

namespace tmp
{
    public class Textures
    {
        private Texture s;
        private Image image;

        public void Load()
        {
            s = new Texture(new Image("grass.png"));
        }
    }
}