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
using System.Threading;

namespace NortonXBOX360
{
    class Enemy : Sprite
    {
        public Texture2D TextureBottom;
        public int health;
        public float rotationTop = 0.0f;
        public float rotationBottom = 0.0f;
        public float rotationMid = 0.0f;
        public Vector2 origin = Vector2.Zero;
        public float scale = 1.0f;
        public SpriteEffects effects = SpriteEffects.None;
        public float layerDepth = 1.0f;
        private bool alive = true;
        public int shootDelay = 0;
        public List<Bullet> BulletList = new List<Bullet>();
        private Vector2 destination;
        private List<Point> path = new List<Point> { };
        private int pathIndex = 0;
        public float speed;
        public bool remove = false;
        private int animateCountTop = 0;
        private int animateCountBottom = 0;
        private int animateDrill = 0;
        private string enemyType;
        public Weapon weapon1;
        public Weapon weapon2;
        private Vector2 direction = Vector2.Zero;
        public bool aggro;
        public bool moved;
        public bool reload;
        private ContentManager content;
        public bool drill;
        private int AIdelay;
        private int Sightdelay;
        private List<Player> _targetList;
        private List<MapObject> _mapObjectList;

        public Enemy(ContentManager contentManager, string newEnemyType)
        {
            content = contentManager;
            Velocity = Vector2.Zero;
            destination = Position;
            alive = true;
            enemyType = newEnemyType;
            switch (newEnemyType)
            {
                case "Chaser":
                    {
                        weapon1 = new Weapon(content, Weapon.WeaponType.Drill);
                        Texture = content.Load<Texture2D>("ChaserTop");
                        TextureBottom = content.Load<Texture2D>("PlayerWalk0");
                        //speed = 2.5f;
                        speed = 0f;
                        this.Height = 40;
                        this.Width = 40;
                        health = 5;
                        break;
                    }
                case "Shooter":
                    {
                        weapon1 = new Weapon(content, Weapon.WeaponType.AssaultRifle);
                        Texture = content.Load<Texture2D>("ShooterTop");
                        TextureBottom = content.Load<Texture2D>("PlayerWalk0");
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
                        Texture = content.Load<Texture2D>("HeavyTop");
                        TextureBottom = content.Load<Texture2D>("PlayerWalk0");
                        speed = 2.5f;
                        this.Height = 60;
                        this.Width = 60;
                        health = 20;
                        break;
                    }
                case "Turret":
                    {
                        weapon1 = new Weapon(content, Weapon.WeaponType.Turret);
                        Texture = content.Load<Texture2D>("TurretTop");
                        TextureBottom = content.Load<Texture2D>("TurretBottom");
                        speed = 2.5f;
                        this.Height = 40;
                        this.Width = 40;
                        health = 10;
                        break;
                    }
            }
            aggro = false;
            origin = this.Center;
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

        public void UpdateAI()
        {
            foreach (Player target in _targetList)
            {
                if (!aggro && Vector2.DistanceSquared(target.Position, Position) < 1000000 && Sightdelay == 0)
                {
                    Vector2 linePosition = Position;
                    Vector2 lineSpeed = new Vector2((float)Math.Cos(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X)), (float)Math.Sin(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X))) * 40;
                    Rectangle line = new Rectangle(0, 0, 10, 10);
                    while (true)
                    {
                        linePosition += lineSpeed;

                        line.X = (int)linePosition.X;
                        line.Y = (int)linePosition.Y;

                        if (line.Intersects(target.GetRectangle()))
                        {
                            aggro = true;
                            break;
                        }

                        foreach (MapObject mapObj in _mapObjectList)
                        {
                            if (mapObj.GetRectangle().Intersects(line))
                            {
                                return;
                            }
                        }

                    }
                }
            }
        }

        public void Update(List<Player> targetList, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList, List<Bullet> bulletList)
        {

            if (health <= 0)
                return;

            Sightdelay++;
            Sightdelay = Sightdelay % 50;

            _targetList = targetList;
            _mapObjectList = mapObjectList;

            //if (!aggro)
            //{
            //    Thread thread = new Thread(UpdateAI);
            //    thread.Start();
            //}

            foreach (Player target in targetList)
            {
                if (!aggro && Vector2.DistanceSquared(target.Position, Position) < 1000000 && Sightdelay == 0)
                {
                    Vector2 linePosition = Position;
                    Vector2 lineSpeed = new Vector2((float)Math.Cos(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X)), (float)Math.Sin(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X))) * 40;
                    Rectangle line = new Rectangle(0, 0, 10, 10);
                    while (true)
                    {
                        linePosition += lineSpeed;

                        line.X = (int)linePosition.X;
                        line.Y = (int)linePosition.Y;

                        if (line.Intersects(target.GetRectangle()))
                        {
                            aggro = true;
                            break;
                        }

                        foreach (MapObject mapObj in mapObjectList)
                        {
                            if (mapObj.GetRectangle().Intersects(line))
                            {
                                goto exitLoop;
                            }
                        }

                    }
                }
            }

        exitLoop:

            if (reload)
                Reload();

            if (weapon1.clipCount <= 0)
            {
                weapon1.reloading = true;
                reload = true;
            }
            if (weapon2 != null && weapon2.clipCount <= 0)
            {
                weapon1.reloading = true;
                reload = true;
            }

            weapon1.Update();
      
                if (alive)
                {
                    switch (enemyType)
                    {
                        case "Chaser":
                            {
                                UpdateChaser(targetList, mapObjectList, content, enemyList);
                                break;
                            }
                        case "Shooter":
                            {
                                UpdateShooter(targetList, mapObjectList, content, enemyList, bulletList);
                                break;
                            }
                        case "Heavy":
                            {
                                weapon2.Update();
                                UpdateHeavy(targetList, mapObjectList, content, enemyList, bulletList);
                                break;
                            }
                        case "Turret":
                            {
                                UpdateTurret(targetList, mapObjectList, content, enemyList, bulletList);
                                break;
                            }

                    }
                    //Move(target, mapObjectList, content, enemyList);
                }
            
            Animate(content, bulletList);

        }

        private void Animate(ContentManager content, List<Bullet> bulletList)
        {
            animateCountTop++;
            string name = null;
            rotationMid = rotationTop;

            if (!alive)
            {
                name = ("explosion_" + animateCountTop.ToString());
                if (animateCountTop >= 74)
                    remove = true;
                else
                    Texture = content.Load<Texture2D>(name);
            }
            else
            {
                if (Velocity != Vector2.Zero)
                {
                    if (moved)
                    {
                        animateCountBottom++;
                        animateCountBottom = animateCountBottom % 52;
                        name = "PlayerWalk" + (animateCountBottom).ToString();
                        TextureBottom = content.Load<Texture2D>(name);
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
                        weapon1.Drill(rotationTop, Position, bulletList, ref temp);
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
        public bool Collision(Rectangle collisionArea, Vector2 newPosition, List<MapObject> mapObjectList, List<Player> playerList, List<Enemy> enemyList)
        {
            collisionArea.X += (int)newPosition.X;
            collisionArea.Y += (int)newPosition.Y;

            for (int x = 0; x < mapObjectList.Count; x++)
                if (mapObjectList[x].GetRectangle().Intersects(collisionArea))
                    return true;

            foreach (Player player in playerList)
            {
                if (player.GetRectangle().Intersects(collisionArea))
                    return true;
            }

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

            health -= damage;
            if (health <= 0)
            {
                alive = false;
                animateCountTop = 0;
                Drop item = new Drop(Position, content);
                dropList.Add(item);
            }
        }

        public void UpdateChaser(List<Player> playerList, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList)
        {
            if (!aggro) return;
            
            direction = Vector2.Zero;
            Player target = playerList[0];
            foreach (Player player in playerList)
            {
                if (Vector2.DistanceSquared(player.Position, Position) < Vector2.DistanceSquared(target.Position, Position))
                    target = player;
            }
            rotationTop = (float)Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X);

            Velocity = new Vector2((float)Math.Cos(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X)), (float)Math.Sin(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X))) * speed;

            if (!Collision(GetRectangle(), new Vector2(Velocity.X, 0), mapObjectList, playerList, enemyList) && aggro)
            {
                PositionX += Velocity.X; moved = true;
                direction.X += Velocity.X;
            }


            if (!Collision(GetRectangle(), new Vector2(0, Velocity.Y), mapObjectList, playerList, enemyList) && aggro)
            {
                PositionY += Velocity.Y; moved = true;
                direction.Y += Velocity.Y;
            }

            if (Vector2.DistanceSquared(Position, target.Position) < 5000 && weapon1.weaponType == Weapon.WeaponType.Drill)
                drill = true;
            else
                drill = false;

            rotationBottom = (float)Math.Atan2(direction.Y, direction.X);
            //}
        }

        public void UpdateShooter(List<Player> playerList, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList, List<Bullet> bulletList)
        {


            if (!aggro) return;
            direction = Vector2.Zero;
            float tempRotation = 0f;
            Player target = playerList[0];

            foreach (Player player in playerList)
            {
                if (Vector2.DistanceSquared(player.Position, Position) < Vector2.DistanceSquared(target.Position, Position))
                    target = player;
            }

            rotationTop = (float)Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X);

            Velocity = new Vector2((float)Math.Cos(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X)), (float)Math.Sin(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X))) * speed;

            if (!Collision(GetRectangle(), new Vector2(Velocity.X, 0), mapObjectList, playerList, enemyList) && aggro)
            {
                PositionX += Velocity.X; moved = true;
                direction.X += Velocity.X;
            }


            if (!Collision(GetRectangle(), new Vector2(0, Velocity.Y), mapObjectList, playerList, enemyList) && aggro)
            {
                PositionY += Velocity.Y; moved = true;
                direction.Y += Velocity.Y;
            }
            rotationBottom = (float)Math.Atan2(direction.Y, direction.X);
            if (aggro && !reload)
                weapon1.Shoot(rotationTop, true, Position, bulletList, target.Position,mapObjectList);
        }

        public void UpdateHeavy(List<Player> playerList, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList, List<Bullet> bulletList)
        {
            if (!aggro) return;

            direction = Vector2.Zero;
            float tempRotation = 0f;

            Player target = playerList[0];

            foreach (Player player in playerList)
            {
                if (Vector2.DistanceSquared(player.Position, Position) < Vector2.DistanceSquared(target.Position, Position))
                    target = player;
            }

            rotationTop = (float)Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X);

            Velocity = new Vector2((float)Math.Cos(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X)), (float)Math.Sin(Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X))) * speed;

            if (!Collision(GetRectangle(), new Vector2(Velocity.X, 0), mapObjectList, playerList, enemyList) && aggro)
            {
                PositionX += Velocity.X; moved = true;
                direction.X += Velocity.X;
            }


            if (!Collision(GetRectangle(), new Vector2(0, Velocity.Y), mapObjectList, playerList, enemyList) && aggro)
            {
                PositionY += Velocity.Y; moved = true;
                direction.Y += Velocity.Y;
            }

            rotationBottom = (float)Math.Atan2(direction.Y, direction.X);
            if (!reload)
            {
                
                direction.Normalize();
                weapon1.Shoot(rotationTop, (direction != Vector2.Zero), Position + (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 0) + (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 20), bulletList, target.Position,mapObjectList);
                weapon2.Shoot(rotationTop, (direction != Vector2.Zero), Position - (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 30) + (new Vector2((float)(Math.Sin(rotationTop)), (float)(Math.Cos(rotationTop))) * 20), bulletList, target.Position,mapObjectList);
                weapon1.clipCount = 1;
                weapon2.clipCount = 1;
            }
        }
        public void UpdateTurret(List<Player> playerList, List<MapObject> mapObjectList, ContentManager content, List<Enemy> enemyList, List<Bullet> bulletList)
        {
            if (!aggro) return;

            direction = Vector2.Zero;

            Player target = playerList[0];

            foreach (Player player in playerList)
            {
                if (Vector2.DistanceSquared(player.Position, Position) < Vector2.DistanceSquared(target.Position, Position))
                    target = player;
            }
            rotationTop = (float)Math.Atan2(target.Position.Y - Position.Y, target.Position.X - Position.X);

            if (!reload)
                weapon1.Shoot(rotationTop, (direction != Vector2.Zero), Position, bulletList, target.Position,mapObjectList);
        }
    }
}
