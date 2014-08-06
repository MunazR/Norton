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
    class MapObject
    {
        Texture2D objTexture;
        Vector2 objPosition;
        Vector2 objSpeed;
        Vector2 objOrigin;
        float objRotation;
        bool objVisible;
        bool collision;
        public bool seen;

        public MapObject()
        {
            seen = false;
            objTexture = null;
            objPosition = Vector2.Zero;
            objSpeed = Vector2.Zero;
            objOrigin = Vector2.Zero;
            objRotation = 0.0f;
            objVisible = true;
            collision = false;
        }
        public Texture2D Texture
        {
            get
            {
                return objTexture;
            }
            set
            {
                objTexture = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return objPosition;
            }
            set
            {
                objPosition = value;
            }
        }

        public Vector2 Speed
        {
            get
            {
                return objSpeed;
            }
            set
            {
                objSpeed = value;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return objOrigin;
            }
            set
            {
                objOrigin = value;
            }
        }
        public float rotation
        {
            get
            {
                return objRotation;
            }
            set
            {
                objRotation = value;
            }
        }

        public bool Collision
        {
            get
            {
                return collision;
            }
            set
            {
                collision = value;
            }
        }

        public Rectangle GetRectangle
        {
            get
            {
                return new Rectangle((int)objPosition.X, (int)objPosition.Y, objTexture.Width, objTexture.Height);
            }
        }
    }
}
