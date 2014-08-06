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
    class Bullet : Sprite
    {
        int _Count;
        int _Damage;
        bool _Alive;
        AmmoTypes _AmmoType;
        Vector2 _Destination;
        bool _Explode;
        int _AnimateCount;
        bool _Remove;
        Rectangle _Collided;
        ContentManager _Content;
        Ray bulletRay;
        public MapObject targetObject;

        public Bullet(ContentManager ContentManager, AmmoTypes newAmmoType, Vector2 newDestination)
        {
            Count = 0;
            AnimateCount = 0;
            AmmoType = newAmmoType;
            Content = ContentManager;
            Ray bulletRay= new Ray();

            switch (AmmoType)
            {
                case AmmoTypes.Bullet:
                    {
                        Texture = Content.Load<Texture2D>("BlueLaser");
                        Damage = 1;
                        break;
                    }
                case AmmoTypes.Rocket:
                    {
                        Texture = Content.Load<Texture2D>("Rocket");
                        Damage = 1;
                        break;
                    }
                case AmmoTypes.Shotgun:
                    {
                        Texture = Content.Load<Texture2D>("BlueLaser");
                        Damage = 1;
                        break;
                    }
                case AmmoTypes.Melee:
                    {
                        Texture = Content.Load<Texture2D>("CursorOn");
                        Damage = 5;
                        break;
                    }
            }

            Width = Texture.Height;
            Height = Width;
            Origin = new Vector2((Height / 2), Height / 2);
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Alive = true;
        }
        public Vector2 Center
        {
            get { return new Vector2(Width / 2, Height / 2); }
        }
        public Ray BulletRay
        {
            get { return bulletRay; }
            set { bulletRay = value; }
        }
        public void DetermineTargetObject(List<MapObject> mapObjectList)
        {
            float? shortestDistance= null;
             for (int i = 0; i < mapObjectList.Count; i++)
             {
                if (bulletRay.Intersects(mapObjectList[i].GetBoundingBox())!= null)
                {
                    if (shortestDistance == null || bulletRay.Intersects(mapObjectList[i].GetBoundingBox()) < shortestDistance)
                    {
                        shortestDistance = bulletRay.Intersects(mapObjectList[i].GetBoundingBox());
                        targetObject = mapObjectList[i];
                    }
                }
            }
        }
        public enum AmmoTypes
        {
            Bullet,
            Rocket,
            Shotgun,
            Melee,
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
        public void Update(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> ItemDropList, ContentManager Content)
        {

            switch (AmmoType)
            {
                case AmmoTypes.Bullet:
                    {
                        UpdateBullet(mapObjectList, enemyList, playerList, ItemDropList, Content);
                        break;
                    }
                case AmmoTypes.Rocket:
                    {
                        if (!(!Alive && Explode))
                        {
                            UpdateRocket(mapObjectList, enemyList, playerList, ItemDropList);
                        }
                        break;
                    }
                case AmmoTypes.Shotgun:
                    {
                        UpdateShotgun(mapObjectList, enemyList, playerList, ItemDropList);
                        break;
                    }
                case AmmoTypes.Melee:
                    {
                        UpdateMelee(mapObjectList, enemyList, playerList, ItemDropList, Content);
                        break;
                    }
            }
            Animate();
        }

        private bool Collision(Rectangle collisionArea, Vector2 newPosition, Rectangle rectangle)
        {
            collisionArea.X += (int)newPosition.X;
            collisionArea.Y += (int)newPosition.Y;

            if (rectangle.Intersects(collisionArea))
            {
                Collided = rectangle;
                return true;
            }

            return false;
        }
        private void UpdateBullet(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList, ContentManager Content)
        {
            Position += Velocity;
            Count++;


                for (int i = 0; i < playerList.Count; i++)
                {
                    if (Rectangle.Intersects(playerList[i].GetRectangle()))
                    {
                        Remove = true;
                        playerList[i].Damage(Damage);
                        return;
                    }
                }
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (Collision(Rectangle, new Vector2(Velocity.X, 0), enemyList[i].GetRectangle()) && Collision(Rectangle, new Vector2(0, Velocity.Y), enemyList[i].GetRectangle()))
                    {
                        if (enemyList[i].Alive)
                        {
                            Remove = true;
                            enemyList[i].Damage(Damage, itemDropList);
                            return;
                        }
                    }
                }

                //for (int i = 0; i < mapObjectList.Count; i++)
                //{
                //    if (Collision(Rectangle, new Vector2(Velocity.X, 0), mapObjectList[i].GetRectangle()) && Collision(Rectangle, new Vector2(0, Velocity.Y), mapObjectList[i].GetRectangle()))
                //    {
                //        Remove = true;
                //        return;
                //    }
                //}
                //if (Rectangle.Intersects(targetObject.GetRectangle()))
                    //    {
                    //        Remove = true;
                    //        return;
                    //    }
                Position += Velocity;
        }
        private void UpdateRocket(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList)
        {
            Position += Velocity;
            Count++;

            for (int i = 0; i < playerList.Count; i++)
            {
                if (Rectangle.Intersects(playerList[i].GetRectangle()))
                {
                    Explosion(playerList, enemyList, mapObjectList, itemDropList);
                    Explode = true;
                    return;
                }
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (Collision(Rectangle, new Vector2(Velocity.X, 0), enemyList[i].GetRectangle()) && Collision(Rectangle, new Vector2(0, Velocity.Y), enemyList[i].GetRectangle()))
                {
                    if (enemyList[i].Alive)
                    {
                        Explosion(playerList, enemyList, mapObjectList, itemDropList);
                        Explode = true;
                        return;
                    }
                }
            }

            for (int i = 0; i < mapObjectList.Count; i++)
            {
                if (Collision(Rectangle, Velocity, mapObjectList[i].GetRectangle()))
                {
                    Explosion(playerList, enemyList, mapObjectList, itemDropList);
                    Explode = true;
                    return;
                }
            }

            if (Vector2.DistanceSquared(Position, Destination) < 100)
            {
                Position = Destination;
                Explode = true;
                Explosion(playerList, enemyList, mapObjectList, itemDropList);
            }
            Position += Velocity;
        }
        private void UpdateGranade()
        {
        }
        private void UpdateShotgun(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList)
        {
            Position += Velocity;
            Count++;

            for (int i = 0; i < playerList.Count; i++)
            {
                if (Rectangle.Intersects(playerList[i].GetRectangle()))
                {
                    Remove = true;
                    playerList[i].Damage(Damage);
                    return;
                }
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (Collision(Rectangle, new Vector2(Velocity.X, 0), enemyList[i].GetRectangle()) && Collision(Rectangle, new Vector2(0, Velocity.Y), enemyList[i].GetRectangle()))
                {
                    if (enemyList[i].Alive)
                    {
                        Remove = true;
                        enemyList[i].Damage(Damage, itemDropList);
                        return;
                    }
                }
            }

            for (int i = 0; i < mapObjectList.Count; i++)
            {
                if (Collision(Rectangle, new Vector2(Velocity.X, 0), mapObjectList[i].GetRectangle()) && Collision(Rectangle, new Vector2(0, Velocity.Y), mapObjectList[i].GetRectangle()))
                {

                    Remove = true;
                    return;
                }
            }
            Position += Velocity;
        }
        private void UpdateMelee(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList, ContentManager Content)
        {

            Count++;
            Position += Velocity;
            if (Count > 0)
                Remove = true;
            for (int i = 0; i < playerList.Count; i++)
            {
                if (Rectangle.Intersects(playerList[i].GetRectangle()))
                {
                    Remove = true;
                    playerList[i].Damage(Damage);
                    return;
                }
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (Collision(Rectangle, new Vector2(Velocity.X, 0), enemyList[i].GetRectangle()) && Collision(Rectangle, new Vector2(0, Velocity.Y), enemyList[i].GetRectangle()))
                {
                    if (enemyList[i].Alive)
                    {
                        Remove = true;
                        enemyList[i].Damage(Damage, itemDropList);
                        return;
                    }
                }
            }
            for (int i = 0; i < mapObjectList.Count; i++)
            {
                if (Collision(Rectangle, new Vector2(Velocity.X, 0), mapObjectList[i].GetRectangle()) && Collision(Rectangle, new Vector2(0, Velocity.Y), mapObjectList[i].GetRectangle()))
                {

                    Remove = true;
                    return;
                }
            }


        }
        private void Explosion(List<Player> playerList, List<Enemy> enemyList, List<MapObject> mapObjectList, List<Drop> itemDropList)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (Vector2.DistanceSquared(Position, playerList[i].Position) < 2000)
                {
                    playerList[i].Damage(5);
                }
                else if (Vector2.DistanceSquared(Position, playerList[i].Position) < 5000)
                {
                    playerList[i].Damage(2);
                }
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (Vector2.DistanceSquared(Position, enemyList[i].Position) < 5000)
                {
                    enemyList[i].Damage(5, itemDropList);
                }
                else if (Vector2.DistanceSquared(Position, enemyList[i].Position) < 8000)
                {
                    enemyList[i].Damage(2, itemDropList);
                }
            }

            Origin = new Vector2(20, 20);
            Velocity = Vector2.Zero;
            AnimateCount = 0;
            Animate();
            Alive = false;
        }
        private void Animate()
        {
            AnimateCount++;
            string name = null;
            if (!Alive)
            {
                name = ("explosion_" + AnimateCount.ToString());
                Texture = Content.Load<Texture2D>(name);
                if (AnimateCount >= 74)
                    Remove = true;
            }
        }

        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
            }
        }

        public int Damage
        {
            get
            {
                return _Damage;
            }
            set
            {
                _Damage = value;
            }
        }

        public bool Alive
        {
            get
            {
                return _Alive;
            }
            set
            {
                _Alive = value;
            }
        }

        public AmmoTypes AmmoType
        {
            get
            {
                return _AmmoType;
            }
            set
            {
                _AmmoType = value;
            }
        }

        public Vector2 Destination
        {
            get
            {
                return _Destination;
            }
            set
            {
                _Destination = value;
            }
        }

        public bool Explode
        {
            get
            {
                return _Explode;
            }
            set
            {
                _Explode = value;
            }
        }

        public int AnimateCount
        {
            get
            {
                return _AnimateCount;
            }
            set
            {
                _AnimateCount = value;
            }
        }

        public bool Remove
        {
            get
            {
                return _Remove;
            }
            set
            {
                _Remove = value;
            }
        }

        public Rectangle Collided
        {
            get
            {
                return _Collided;
            }
            set
            {
                _Collided = value;
            }
        }

        public ContentManager Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
            }
        }
    }
}
