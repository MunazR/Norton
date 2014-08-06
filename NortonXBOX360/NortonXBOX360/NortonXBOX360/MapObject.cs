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

namespace NortonXBOX360
{
    class MapObject : Sprite
    {
        bool _Visible;
        bool _Seen;

        public MapObject(Texture2D texture)
        {
            Texture = texture;
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Origin = Vector2.Zero;
            Rotation = 0.0f;
            Visible = true;
            this.Height = Texture.Height;
            this.Width = this.Height;
        }

        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        public bool Seen
        {
            get
            {
                return _Seen;
            }
            set
            {
                _Seen = value;
            }
        }
        public override Rectangle GetRectangle()
        {
                return new Rectangle((int)Position.X, (int)Position.Y, this.Width, this.Height);
        }
        public BoundingBox GetBoundingBox()
        {
                return new BoundingBox(new Vector3((int)Position.X, (int)Position.Y, 0),new Vector3(this.Width, this.Height,1));
        }
    }
}
