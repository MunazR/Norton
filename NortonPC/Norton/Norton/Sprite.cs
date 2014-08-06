using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Norton
{
    class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public int Height;
        public int Width;
        public Color color;

        public Sprite()
        {
        }
        public Vector2 Center
        {
            get { return new Vector2(Width / 2, Height / 2); }
        }
    }
}
