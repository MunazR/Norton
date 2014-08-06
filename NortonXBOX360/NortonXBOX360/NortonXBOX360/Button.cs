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

namespace NortonXBOX360
{
    class Button : Sprite
    {
        public Button()
        {
            Texture = null;
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Origin = Vector2.Zero;
            Rotation = 0;
        }
    }
}
