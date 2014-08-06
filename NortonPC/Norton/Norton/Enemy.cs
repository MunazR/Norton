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
    class Enemy : Sprite
    {
        public Texture2D textureBottom;
        public int health;
        public Vector2 position = new Vector2(0,0);
        public Color color = Color.White;
        public float rotationTop = 0.0f;
        public float rotationBottom = 0.0f;
        public float rotationMid = 0.0f;
        public Vector2 origin = Vector2.Zero;
        public float scale = 1.0f;
        public SpriteEffects effects = SpriteEffects.None;
        public float layerDepth = 1.0f;
        private bool alive = true;
        public Vector2 velocity;
        public int shootDelay = 0;
        public List<Bullet> BulletList = new List<Bullet>();
        public bool EnableUp=true;
        public bool EnableDown = true;
        public bool EnableLeft = true;
        public bool EnableRight = true;
        private Vector2 destination;
        private bool[,] map; 
        private List<Point> path = new List<Point> { };
        private int pathIndex =0;
        public float speed;
        public bool remove= false;
        private int animateCountTop=0;
        private int animateCountBottom = 0;
        private int animateDrill = 0;
        public string enemyType;
        public Weapon weapon1;
        public Weapon weapon2;
        private Vector2 direction = Vector2.Zero;
        public bool aggro;
        public bool moved;
        public bool reload;
        private ContentManager content;
        public bool drill;
        public bool los;

        public Enemy(ContentManager contentManager, string newEnemyType)
        {
            content = contentManager;
            velocity = Vector2.Zero;
            origin = Center;
            destination = position;
            alive = true;
            enemyType = newEnemyType;
            switch (newEnemyType)
            {
                case "Chaser":
                    {
                        weapon1 = new Weapon(content, Weapon.WeaponType.Drill);
                        texture = content.Load<Texture2D>("ChaserTop");
                        textureBottom = content.Load<Texture2D>("PlayerWalk0");
                        speed = 2.5f;
                        this.Height = 40;
                        this.Width = 40;
                        health = 5;
                        break;
                    }
                case "Shooter":
                    {
                        weapon1 = new Weapon(content, Weapon.WeaponType.AssaultRifle);
                        texture = content.Load<Texture2D>("ShooterTop");
                        textureBottom = content.Load<Texture2D>("PlayerWalk0");
                        speed = 2.5f;
                        this.Height = 40;
                        this.Width = 40;
                        health = 5;
                        break;
                    }
                case "Heavy":
                    {
                        weapon1 = new Weapon(content, Weapon.WeaponType.Shotgun);
                        weapon2 = new Weapon(content, Weapon.WeaponType.RocketLauncher);

                        texture = content.Load<Texture2D>("HeavyTop");
                        textureBottom = content.Load<Texture2D>("PlayerWalk0");
                        speed = 1.5f;
                        this.Height = 60;
                        this.Width = 60;
                        health = 40;
                        break;
                    }
                case "Turret":
                    {
                        weapon1 = new Weapon(content, Weapon.WeaponType.Turret);
                        texture = content.Load<Texture2D>("TurretTop");
                        textureBottom = content.Load<Texture2D>("TurretBottom");
                        speed = 2.5f;
                        this.Height = 40;
                        this.Width = 40;
                        health = 10;
                        break;
                    }
            }
            origin = this.Center;
            aggro = false;

        }
        public bool LineOfSight()
        {
            return false;
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        private void moveToDestination()
        {
            if (path == null) return;
            if (pathIndex > path.Count)
            {
            }
        }
        public Rectangle GetRectangle
        {
            get
            {
                return new Rectangle((int)position.X - (Width / 2), (int)position.Y - (Height/2), Width, Height);
            }
        }

        public void Update(Player target, List<MapObject> mapObjectList,ContentManager content,List<Enemy> enemyList, List<Bullet> bulletList)
        {

            if (/*!aggro && */Vector2.DistanceSquared(target.position, position) < 1000000)
            {
                Vector2 linePosition = position;
                Vector2 lineSpeed = new Vector2((float)Math.Cos(Math.Atan2(target.position.Y - position.Y, target.position.X - position.X)), (float)Math.Sin(Math.Atan2(target.position.Y - position.Y, target.position.X - position.X))) * 40;
                Rectangle line = new Rectangle(0, 0, 10, 10);
                while (true)
                {
                    linePosition += lineSpeed;

                    line.X = (int)linePosition.X;
                    line.Y = (int)linePosition.Y;

                    if (line.Intersects(target.GetRectangle))
                    {
                        los = true;
                        aggro = true;
                        break;
                    }

                    foreach (MapObject mapObj in mapObjectList)
                        if (mapObj.GetRectangle.Intersects(line))
                        {
                            los = false;
                            goto exitLoop;
                        }

                    if (Vector2.Distance(position, linePosition) > 1000)
                        goto exitLoop;
                }
            }
            //if (!aggro && Vector2.DistanceSquared(target.position, position) < 10000)
            //{
            //    aggro = true;
            //}

        exitLoop:

            //if (Vector2.Distance(target.position, position) < 250)
            //    aggro = true;

            if (reload && !(enemyType == "Heavy"))
                Reload();

        if (weapon1.clipCount <= 0 && !(enemyType == "Heavy"))
            {
                weapon1.reloading = true;
                reload = true;
            }

            weapon1.Update();
            if (enemyType == "Heavy")
                weapon2.Update();

            if (alive)
            {
                switch (enemyType)
                {
                    case "Chaser":
                        {
                            UpdateChaser(target, mapObjectList, content, enemyList);
                            break;
                        }
                    case "Shooter":
                        {
                            UpdateShooter(target, mapObjectList, content, enemyList,bulletList);
                            break;
                        }
                    case "Heavy":
                        {
                            reload = false;
                            weapon1.reloading = false;
                            weapon2.reloading = false;
                            UpdateHeavy(target, mapObjectList, content, enemyList,bulletList);
                            break;
                        }
                    case "Turret":
                        {
                            UpdateTurret(target, mapObjectList, content, enemyList, bulletList);
                            break;
                        }

                }
                //Move(target, mapObjectList, content, enemyList);
            }

            Animate(content,bulletList);
        }

        private void Animate(ContentManager content, List<Bullet> bulletList)
        {
            animateCountTop++;
            string name = null;
            rotationMid = rotationTop;

            if (!alive)
            {
                name = ("explosion_" + animateCountTop.ToString());
                texture = content.Load<Texture2D>(name);
                if (animateCountTop >= 74)
                    remove = true;
            }
            else
            {
                if (velocity != Vector2.Zero)
                {
                        if (moved)
                        {
                            animateCountBottom++;
                            animateCountBottom = animateCountBottom % 52;
                            name = "PlayerWalk" + (animateCountBottom).ToString();
                            textureBottom = content.Load<Texture2D>(name);
                            moved = false;
                        }
                }
                else
                {
                    animateCountTop = 0;
                }
                if (drill)
                {
                    if (animateDrill < 20)
                    {
                        animateDrill++;
                        rotationMid -= (animateDrill / 30f);
                    }
                    else
                    {
                        int temp = 100;
                        weapon1.Drill(rotationTop, position, bulletList, ref temp);
                    }
                }
                else
                    if (animateDrill >= 0)
                    {
                        animateDrill--;
                        rotationMid -= (animateDrill / 30f);
                    }
            }
        }
        public bool Collision(Rectangle collisionArea, Vector2 newPosition, List<MapObject> mapObjectList, Player player, List<Enemy> enemyList)
        {
            collisionArea.X += (int)newPosition.X;
            collisionArea.Y += (int)newPosition.Y;

            foreach (MapObject obj in mapObjectList)
                if (obj.GetRectangle.Intersects(collisionArea))
                    return true;

            if (player.GetRectangle.Intersects(collisionArea))
                return true;

            return false;
        }
        public void Reload()
        {
            int temp = 1000;
            weapon1.Reload(ref temp);
            if (!weapon1.reloading)
                reload = false;


        }
        public void Damage(int damage, List<Drop> dropList)
        {
            if (!alive) return;

            health-= damage;
            if (health <= 0)
            {
                alive = false;
                animateCountTop = 0;
                Drop item = new Drop(position, content);
                dropList.Add(item);
            }
        }
        public void Die()
        {
        }
        public void UpdateChaser(Player player, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList)
        {
            direction = Vector2.Zero;
            if(aggro)
                rotationTop = (float)Math.Atan2(player.position.Y - position.Y, player.position.X - position.X);

            velocity = new Vector2((float)Math.Cos(Math.Atan2(player.position.Y - position.Y, player.position.X - position.X)), (float)Math.Sin(Math.Atan2(player.position.Y - position.Y, player.position.X - position.X))) * speed;

            if (!Collision(GetRectangle, new Vector2(velocity.X, 0), mapObjectList, player, enemyList) && aggro)
            {
                position.X += velocity.X;moved = true;
                direction.X += velocity.X;
            }


            if (!Collision(GetRectangle, new Vector2(0, velocity.Y), mapObjectList, player, enemyList) && aggro)
            {
                position.Y += velocity.Y; moved = true;
                direction.Y += velocity.Y;
            }

            if (Vector2.DistanceSquared(position, player.position) < 5000 && weapon1.weaponType == Weapon.WeaponType.Drill)
                drill = true;
            else
                drill = false;

            rotationBottom = (float)Math.Atan2(direction.Y, direction.X);
            //}
        }
        public void UpdateShooter(Player player, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList, List<Bullet> bulletList)
        {
            direction = Vector2.Zero;
            float tempRotation = 0f;

            if(aggro)

            rotationTop = (float)Math.Atan2(player.position.Y - position.Y, player.position.X - position.X);

            velocity = new Vector2((float)Math.Cos(Math.Atan2(player.position.Y - position.Y, player.position.X - position.X)), (float)Math.Sin(Math.Atan2(player.position.Y - position.Y, player.position.X - position.X))) * speed;

            if (!Collision(GetRectangle, new Vector2(velocity.X, 0), mapObjectList, player, enemyList) && aggro)
            {
                position.X += velocity.X; moved = true;
                direction.X += velocity.X;
            }


            if (!Collision(GetRectangle, new Vector2(0, velocity.Y), mapObjectList, player, enemyList) && aggro)
            {
                position.Y += velocity.Y; moved = true;
                direction.Y += velocity.Y;
            }
            rotationBottom = (float)Math.Atan2(direction.Y, direction.X);
            if (los && !reload)
                weapon1.Shoot(rotationTop, (direction != Vector2.Zero), position, bulletList, player.position);
        }

        public void UpdateHeavy(Player player, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList, List<Bullet> bulletList)
        {
            direction = Vector2.Zero;
            float tempRotation = 0f;

            if (aggro)
                rotationTop = (float)Math.Atan2(player.position.Y - position.Y, player.position.X - position.X);

            velocity = new Vector2((float)Math.Cos(Math.Atan2(player.position.Y - position.Y, player.position.X - position.X)), (float)Math.Sin(Math.Atan2(player.position.Y - position.Y, player.position.X - position.X))) * speed;

            if (!Collision(GetRectangle, new Vector2(velocity.X, 0), mapObjectList, player, enemyList) && aggro)
            {
                position.X += velocity.X; moved = true;
                direction.X += velocity.X;
            }


            if (!Collision(GetRectangle, new Vector2(0, velocity.Y), mapObjectList, player, enemyList) && aggro)
            {
                position.Y += velocity.Y; moved = true;
                direction.Y += velocity.Y;
            }
            rotationBottom = (float)Math.Atan2(direction.Y, direction.X);
            if (los)
            {
                direction.Normalize();
                weapon1.Shoot(rotationTop, (direction != Vector2.Zero), position + (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 20) + (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 20), bulletList, player.position);
                weapon2.Shoot(rotationTop, (direction != Vector2.Zero), position - (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 20) + (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 20), bulletList, player.position);
                weapon1.clipCount = 1;
                weapon2.clipCount = 1;
            }
        }
        public void UpdateTurret(Player player, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList, List<Bullet> bulletList)
        {
            direction = Vector2.Zero;
            float tempRotation = 0f;

            if (aggro)
                rotationTop = (float)Math.Atan2(player.position.Y - position.Y, player.position.X - position.X);

            if (los && !reload)
                weapon1.Shoot(rotationTop, (direction != Vector2.Zero), position, bulletList, player.position);
        }
    }
}
