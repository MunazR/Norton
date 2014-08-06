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
    class Bullet
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;
        public float rotation;
        public int count =0;
        public int damage;
        private int width;
        private int height;
        public bool alive;
        public string ammoType;
        ContentManager content;
        public Vector2 initialPosition;
        public Vector2 destination;
        private bool explode;
        private int animateCount = 0;
        public bool remove = false;
        public Rectangle collided;
        private int meleeCount=0;

        public Bullet(ContentManager contentManager, string newAmmoType, Vector2 newDestination)
        {
            ammoType = newAmmoType;
            content = contentManager;
            switch (ammoType)
            {
                case "Bullet":
                {
                    texture = content.Load<Texture2D>("BlueLaser");
                    damage = 1;


                    break;
                }
                case "Rocket":
                {
                    texture = content.Load<Texture2D>("Rocket");
                    damage = 10;
                    break;
                }
                case "Shotgun":
                {
                    texture = content.Load<Texture2D>("BlueLaser");
                    damage = 1;
                    break;
                }
                case "Melee":
                {
                    texture = content.Load<Texture2D>("CursorOn");
                    damage = 5;
                    break;
                }
            }
            width = texture.Height;
            height = width;
            origin = new Vector2((height / 2), height / 2);
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            alive = true;
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }
        public void Update(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> ItemDropList, ContentManager content)
        {
            
            switch (ammoType)
            {
                case "Bullet":
                    {
                        UpdateBullet(mapObjectList, enemyList, playerList, ItemDropList, content);
                        break;
                    }
                case "Rocket":
                    {
                        if (!alive && explode)
                        {
                          
                        }
                        else
                        {
                            UpdateRocket(mapObjectList, enemyList, playerList, ItemDropList);
                        }
                        break;
                    }
                case "Shotgun":
                    {
                         UpdateShotgun(mapObjectList, enemyList, playerList, ItemDropList);
                         break;
                    }
                case "Melee":
                    {
                        UpdateMelee(mapObjectList, enemyList, playerList, ItemDropList,content);
                        break;
                    }
            }
            Animate();
        }   
        public Vector2 Center
        {
            get { return new Vector2(width / 2, height / 2); }
        }
        
        //Checks for collision
        private bool Collision(Rectangle collisionArea, Vector2 newPosition, Rectangle rectangle)
        {
            collisionArea.X += (int)newPosition.X;
            collisionArea.Y += (int)newPosition.Y;

            if (rectangle.Intersects(collisionArea))
            {
                collided = rectangle;
                return true;
            }

            return false;
        }

        //Shoots bullet
        private void UpdateBullet(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList, ContentManager content)
        {
            position += velocity;
            count++;

            foreach (Player player in playerList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), player.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), player.GetRectangle))
                {
                    remove = true;
                    player.Damage(damage);
                    return;
                }
            }

            foreach (Enemy enemy in enemyList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), enemy.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), enemy.GetRectangle))
                {
                    if (enemy.Alive)
                    {
                        remove = true;
                        enemy.Damage(damage, itemDropList);
                        return;
                    }
                }
            }

            foreach (MapObject obj in mapObjectList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), obj.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), obj.GetRectangle) && obj.Collision)
                {

                    remove = true;
                    return;
                }
            }
            position += velocity;
        }
        private void UpdateRocket(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList)
        {
            if (!alive) return;
            position += velocity;
            count++;

            foreach (Player player in playerList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), player.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), player.GetRectangle))
                {
                    Explode(playerList,enemyList,mapObjectList,itemDropList);
                    player.Damage(damage);
                    return;
                }
            }
           
            foreach (Enemy enemy in enemyList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), enemy.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), enemy.GetRectangle))
                {
                    if (enemy.Alive)
                    {
                        Explode(playerList, enemyList, mapObjectList, itemDropList);
                        enemy.Damage(damage, itemDropList);
                        return;
                    }
                }
            }

            foreach (MapObject obj in mapObjectList)
            {
                if (Collision(Rectangle, velocity, obj.GetRectangle))
                {
                    Explode(playerList, enemyList, mapObjectList, itemDropList);
                    explode = true;
                    return;
                }
            }
            if (Vector2.DistanceSquared(position,destination)<100)
            {
                position = destination;
                explode = true;
                Explode(playerList, enemyList, mapObjectList, itemDropList);
            }
            position += velocity;
        }
        private void UpdateGranade()
        {
        }
        private void UpdateShotgun(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList)
        {
            position += velocity;
            count++;
            
            foreach (Player player in playerList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), player.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), player.GetRectangle))
                {
                    remove = true;
                    player.Damage(damage);
                    return;
                }
            }

            foreach (Enemy enemy in enemyList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), enemy.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), enemy.GetRectangle))
                {
                    if (enemy.Alive)
                    {
                        remove = true;
                        enemy.Damage(damage, itemDropList);
                        return;
                    }
                }
            }

            foreach (MapObject obj in mapObjectList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), obj.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), obj.GetRectangle) && obj.Collision)
                {

                    remove = true;
                    return;
                }
            }
            position += velocity;
        }
        private void UpdateMelee(List<MapObject> mapObjectList, List<Enemy> enemyList, List<Player> playerList, List<Drop> itemDropList, ContentManager content)
        {
 
            count++;
            position += velocity;
                remove = true;
            foreach (Player player in playerList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), player.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), player.GetRectangle))
                {
                    remove = true;
                    player.Damage(damage);
                    return;
                }
            }

            foreach (Enemy enemy in enemyList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), enemy.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), enemy.GetRectangle))
                {
                    if (enemy.Alive)
                    {
                        remove = true;
                        enemy.Damage(damage, itemDropList);
                        return;
                    }
                }
            }
            foreach (MapObject obj in mapObjectList)
            {
                if (Collision(Rectangle, new Vector2(velocity.X, 0), obj.GetRectangle) && Collision(Rectangle, new Vector2(0, velocity.Y), obj.GetRectangle) && obj.Collision)
                {

                    remove = true;
                    return;
                }
            }
        

        }
        private void Explode(List<Player> playerList,List<Enemy> enemyList,List<MapObject> mapObjectList,List<Drop> itemDropList)
        {
            foreach (Player player in playerList)
            {
                if (Vector2.DistanceSquared(position,player.position)<2000)
                {
                    player.Damage(5);
                }
                else if (Vector2.DistanceSquared(position, player.position) < 5000)
                {
                    player.Damage(2);
                }
            }

            foreach (Enemy enemy in enemyList)
            {
                if (Vector2.DistanceSquared(position, enemy.position) < 5000)
                {
                   enemy.Damage(5,itemDropList);
                }
                else if (Vector2.DistanceSquared(position, enemy.position) < 8000)
                {
                    enemy.Damage(2,itemDropList);
                }
            }
            //foreach (MapObject obj in mapObjectList)
            //{
            //}
            origin = new Vector2(20, 20);
            velocity = Vector2.Zero;
            animateCount = 0;
            Animate();
            alive = false;
        }
        private void Animate()
        {
            animateCount++;
            string name = null;
            if (!alive)
            {
                name = ("explosion_" + animateCount.ToString());
                texture = content.Load<Texture2D>(name);
                if (animateCount >= 73)
                {
                    animateCount = 74;
                    remove = true;
                }
            }
        }
    }
}
