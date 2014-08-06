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
    class Sprite
    {
        Texture2D _Texture;
        Vector2 _Position;
        Vector2 _Velocity;
        Vector2 _Origin;
        float _Rotation;
        int _Height;
        int _Width;


        public Sprite()
        {
            _Texture = null;
            _Position = Vector2.Zero;
            _Velocity = Vector2.Zero;
            _Origin = Vector2.Zero;
            _Rotation = 0;
        }
        public virtual Rectangle GetRectangle()
        {
            //Rectangle rectangle = new Rectangle((int)Position.X - (int)Center.X, (int)Position.Y - (int)Center.Y, Width, Height);
            Rectangle rectangle = new Rectangle((int)Position.X - (Width/2), (int)Position.Y-(Height/2), Width, Height);
            return rectangle;
        }
        public Texture2D Texture
        {
            get
            {
                return _Texture;
            }
            set
            {
                _Texture = value;
            }
        }
        public Vector2 Center
        {
            get { return new Vector2(Width / 2, Height / 2); }

        }
        public float PositionX
        {
            get
            {
                return _Position.X;
            }
            set
            {
                _Position.X = value;
            }
        }

        public float PositionY
        {
            get
            {
                return _Position.Y;
            }
            set
            {
                _Position.Y = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return _Velocity;
            }
            set
            {
                _Velocity = value;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return _Origin;
            }
            set
            {
                _Origin = value;
            }
        }

        public float Rotation
        {
            get
            {
                return _Rotation;
            }
            set
            {
                _Rotation = value;
            }
        }

        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }

        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }
    }
}
