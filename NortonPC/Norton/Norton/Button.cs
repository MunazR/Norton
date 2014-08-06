using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Norton
{
    class Button
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle rectangle;

        public Button()
        {
            texture = null;
            position = Vector2.Zero;
        }

        public Rectangle GetRectangle
        {
            get
            {
                rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                return rectangle;
            }
        }
    }
}
