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
    class Camera
    {
        public Matrix transform;
        public Viewport view;
        Vector2 center;
        public Vector2 inverse = Vector2.Zero;

        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(GameTime gameTime, Player player, Rectangle room)
        {
            Vector2 initialCenter = center;


            center = new Vector2(player.Position.X - view.Width / 2, player.Position.Y - view.Height / 2);

            center = Bounds(0, 0, room.Height, room.Width, new Vector2(center.X, center.Y));

            inverse += initialCenter - center;

            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }

        public Rectangle Rectangle()
        {
            return new Rectangle((int)center.X, (int)center.Y, view.Width, view.Height);
        }

        public Vector2 Bounds(int minX, int minY, int maxX, int maxY, Vector2 position)
        {
            //X Value
            if (position.X + view.Width > maxX)
                position.X = maxX - view.Width;
            else if (position.X < minX)
                position.X = minX;

            //Y Value
            if (position.Y + view.Height > maxY)
                position.Y = maxY - view.Height;
            else if (position.Y < minY)
                position.Y = minY;

            return position;
        }
    }
}
