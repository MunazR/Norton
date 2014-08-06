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
    class Player: Sprite
    {
        public Texture2D textureBottom;
        public Vector2 position = new Vector2(0,0);
        public Color color = Color.White;
        public float rotationTop = 0.0f;
        public float rotationBottom = 0.0f;
        public Vector2 origin = Vector2.Zero;
        public float scale = 1.0f;
        public SpriteEffects effects = SpriteEffects.None;
        public float layerDepth = 1.0f;
        private bool alive = true;
        public Vector2 velocity;
        public Vector2 direction;
        private string currentAnimation;
        public int shootDelay;
        public int delay;
        public bool EnableUp=true;
        public bool EnableDown = true;
        public bool EnableLeft = true;
        public bool EnableRight = true;
        public float speed;
        public int health=0;
        public bool reload = false;
        public int bulletCount=0;
        public Weapon weapon1;
        public Weapon weapon2;
        public Weapon weapon3;
        public Texture2D currentWeaponTexture;
        private int animateCountTop = 0;
        private int animateCountBottom = 0;
        private bool moved;
        public int speedMultiplier;
        public int stamina;
        public int maxStamina;
        public bool staminaDepleted;
        public int maxHealth;
        private ContentManager content;
        public bool sprint;
        public int currentWeaponIndex;
        public int rocketAmmoCount;
        public int assaultAmmoCount;
        public int shotgunAmmoCount;
        public int drillAmmoCount;
        public int rocketAmmoMax;
        public int assaultAmmoMax;
        public int shotgunAmmoMax;
        public int drillAmmoMax;
        public List<Weapon> weaponList;
        private bool melee;
        private int animateMeleeCount=0;
        private int animateSprintCount = 0;
        public float rotationMid = 0.0f;
        public Vector2 positionMid;
        public int meleeDamage;


        //ContentManager content= new ContentManager(,"Content");

        public Player(ContentManager contentManager) 
        {
            weaponList = new List<Weapon>();
            content = contentManager;
            //content.RootDirectory = "Content";
            velocity = Vector2.Zero;
            origin = Center;
            this.Height = 40;
            this.Width = 40;
            meleeDamage =1;
            weapon1 = new Weapon(content, Weapon.WeaponType.AssaultRifle);
            weapon2 = new Weapon(content, Weapon.WeaponType.Shotgun);
            weapon3 = new Weapon(content, Weapon.WeaponType.RocketLauncher);
            weaponList.Add(weapon1);
            weaponList.Add(weapon2);
            weaponList.Add(weapon3);
            currentWeaponIndex  = 1;
            origin = this.Center;
            shootDelay = 0;
            delay = 3;
            speed = 1f;
            alive = true;
            textureBottom = content.Load<Texture2D>("PlayerWalk0");
            maxHealth = 100;
            health = maxHealth;
            currentWeaponTexture = weapon1.idleTexture;
            speedMultiplier = 5;
            staminaDepleted = false;
            maxStamina = 100;
            stamina = maxStamina;

            rocketAmmoMax = 10;
            assaultAmmoMax = 200;
            shotgunAmmoMax = 50;
            drillAmmoMax = 500;
            rocketAmmoCount = rocketAmmoMax;
            assaultAmmoCount = assaultAmmoMax;
            shotgunAmmoCount = shotgunAmmoMax;
            drillAmmoCount = drillAmmoMax;
        }

        public Weapon currentWeapon()
        {
            switch (currentWeaponIndex)
            {
                case 1:
                    return weapon1;
                case 2:
                    return weapon2;
                case 3:
                    return weapon3;
            }
            return weapon1;
        }

        public void ResetAmmoCount()
        {
            assaultAmmoCount = assaultAmmoMax;
            rocketAmmoCount = rocketAmmoMax;
            shotgunAmmoCount = shotgunAmmoMax;
        }

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        public Rectangle GetRectangle
        {
            get
            {
                return new Rectangle((int)position.X-20, (int)position.Y-20, 40, 40);
            }
        }
        public void Update(GameTime gameTime, KeyboardState keyState, MouseState mouseState, List<MapObject> mapObjectList, ContentManager content, List<Bullet> bulletList,Vector2 matrixOffset,List<Enemy> enemyList,List<Drop> dropList)
        {
            weapon1.Update();
            weapon2.Update();
            weapon3.Update();

            Move(gameTime, keyState, mapObjectList, matrixOffset,enemyList,bulletList);
            
            if (reload)
                Reload();

            if (!sprint &&!reload)
                Shoot(mouseState, bulletList, content, matrixOffset);

            Animate(content, bulletList, enemyList, mapObjectList, dropList);
        }

        private void Move(GameTime gameTime, KeyboardState keyState, List<MapObject> mapObjectList, Vector2 matrixOffset, List<Enemy> enemyList,List<Bullet> bulletList)
        {
            velocity = Vector2.Zero;
            direction = Vector2.Zero;
            moved = false;
            sprint = false;
            if (stamina == 0)
            {
                staminaDepleted = true;
                stamina++;
            }

            else if (keyState.IsKeyDown(Keys.LeftShift) && !staminaDepleted)
            {
                reload = false;
                currentWeapon().reloading = false;
                speedMultiplier = 8;
                sprint = true;
                stamina--;
            }
            else if (stamina == maxStamina)
            {
                staminaDepleted = false;
                speedMultiplier = 4;
            }
            else
            {
                speedMultiplier = 4;
                stamina++;
            }

            for (int i = 0; i < speedMultiplier; i++)
            {

                if (keyState.IsKeyDown(Keys.A) && !Collision(GetRectangle, new Vector2(-speed, 0), mapObjectList, enemyList))
                {
                    position.X -= speed;
                    direction.X--;
                    moved = true;
                }
                if (keyState.IsKeyDown(Keys.D) && !Collision(GetRectangle, new Vector2(speed, 0), mapObjectList, enemyList))
                {
                    position.X += speed;
                    direction.X++;
                    moved = true;
                }
                if (keyState.IsKeyDown(Keys.W) && !Collision(GetRectangle, new Vector2(0, -speed), mapObjectList, enemyList))
                {
                    position.Y -= speed;
                    direction.Y--;
                    moved = true;
                }
                if (keyState.IsKeyDown(Keys.S) && !Collision(GetRectangle, new Vector2(0, speed), mapObjectList, enemyList))
                {
                    position.Y += speed;
                    direction.Y++;
                    moved = true;
                }
                direction.Normalize();
                position += velocity;
                if (velocity != Vector2.Zero)
                    velocity.Normalize();
            }

          
            rotationTop = (float)Math.Atan2(Mouse.GetState().Y - position.Y - matrixOffset.Y, Mouse.GetState().X - position.X - matrixOffset.X);
            rotationMid = rotationTop;
            rotationBottom = (float)Math.Atan2(direction.Y, direction.X);

            if (keyState.IsKeyDown(Keys.V)&&!sprint)
            {
                reload = false;
                currentWeapon().reloading = false;
                melee = true;
            }
            if (sprint && currentWeapon().clipCount <= 0) { }
            if ((keyState.IsKeyDown(Keys.R) && !reload && CurrentAmmo() > 0 || currentWeapon().clipCount <= 0) && currentWeapon().clipCount != currentWeapon().clipMax)
            {
                if (!sprint && !melee)
                {
                    reload = true;
                    currentWeapon().reloading = true;
                }
            }

            if (keyState.IsKeyDown(Keys.D1))
                currentWeaponIndex = 1;
            else if (keyState.IsKeyDown(Keys.D2))
                currentWeaponIndex = 2;
            else if(keyState.IsKeyDown(Keys.D3))
                currentWeaponIndex = 3;

        }
        private void Animate(ContentManager content,List<Bullet> bulletList,List<Enemy> enemyList,List<MapObject> mapObjectList,List<Drop> dropList)
        {
            string name = null;
            positionMid = position;
            if (alive)
            {
                if (moved)
                {
                    if (sprint)
                    {
                        animateCountBottom++;
                        animateCountBottom = (animateCountBottom) % 52;
                        name = "PlayerWalk" + (animateCountBottom).ToString();
                        textureBottom = content.Load<Texture2D>(name);
                    }
                    else
                    {
                        animateCountBottom++;
                        animateCountBottom = animateCountBottom % 52;
                        name = "PlayerWalk" + (animateCountBottom).ToString();
                        textureBottom = content.Load<Texture2D>(name);
                    }
                }
                if (melee)
                {
                    animateMeleeCount++;
                    if (animateMeleeCount <= 10)
                        rotationMid -= (animateMeleeCount / 5f);
                    else if (animateMeleeCount < 40)
                        rotationMid -= (2f - (animateMeleeCount / 20f));
                    else if (animateMeleeCount >= 40)
                        melee = false;
                    if (animateMeleeCount == 20)
                        Melee(enemyList, mapObjectList, bulletList, dropList);
                }
                else
                    animateMeleeCount = 0;
                
                if (sprint)
                {
                    animateSprintCount++;
                    animateSprintCount = animateSprintCount % 60;

                    rotationMid -= 1.5f;
                    if (animateSprintCount >45)
                        rotationMid += ((animateSprintCount-45) / 20f);
                    else if (animateSprintCount >30)
                        rotationMid -= ((animateSprintCount-30) / 20f);
                    else if (animateSprintCount >15)
                        rotationMid += ((animateSprintCount-15) / 20f);
                    else 
                        rotationMid -= (animateSprintCount / 20f);
                }
                else
                    animateSprintCount = 0;
            }
        }
        private void Melee(List<Enemy> enemyList,List<MapObject> mapObjectList,List<Bullet> bulletList,List<Drop> dropList)
        {

                        weapon1.Melee(rotationTop,bulletList,position);

        }
        private void Reload()
        {
            switch (currentWeapon().weaponType)
            {
                case Weapon.WeaponType.AssaultRifle:
                    currentWeapon().Reload(ref assaultAmmoCount);
                        break;
                case Weapon.WeaponType.Shotgun:
                        currentWeapon().Reload(ref shotgunAmmoCount);
                         break;
                case Weapon.WeaponType.RocketLauncher:
                         currentWeapon().Reload(ref rocketAmmoCount);
                         break;
            }
            if (!currentWeapon().reloading)
                reload = false;
        }

        public int CurrentAmmo()
        {
            switch (currentWeapon().weaponType)
            {
                case Weapon.WeaponType.AssaultRifle:
                    return assaultAmmoCount;
                case Weapon.WeaponType.Shotgun:
                    return shotgunAmmoCount;
                case Weapon.WeaponType.RocketLauncher:
                    return rocketAmmoCount;
            }

            return 0;
        }

        private void Shoot(MouseState mouseState, List<Bullet> bulletList, ContentManager content, Vector2 matrixOffset)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
                currentWeapon().Shoot(rotationTop, (direction != Vector2.Zero), position, bulletList,new Vector2(mouseState.X- matrixOffset.X,mouseState.Y-matrixOffset.Y));
        }
        private bool Collision(Rectangle collisionArea, Vector2 newPosition, List<MapObject> mapObjectList,List<Enemy> enemyList)
        {
            collisionArea.X += (int)newPosition.X;
            collisionArea.Y += (int)newPosition.Y;

            foreach (MapObject obj in mapObjectList)
                if (obj.GetRectangle.Intersects(collisionArea) && obj.Collision)
                    return true;

            foreach (Enemy enemy in enemyList)
                if (Vector2.DistanceSquared((position+newPosition),enemy.position) < 1000 && enemy.Alive)
                    return true;

            return false;
        }

        public void Damage(int damage)
        {
          health -= damage;
        }

    }
}
