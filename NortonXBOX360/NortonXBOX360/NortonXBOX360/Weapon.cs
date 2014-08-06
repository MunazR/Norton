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
    class Weapon
    {
        public int clipCount = 0;
        public int clipMax = 0;
        public int fireRate;
        public int accuracy;
        public int reloadDelay;
        public Texture2D idleTexture;
        public Texture2D fireTexture;
        public WeaponType weaponType;
        public Texture2D currentWeaponTexture;
        private ContentManager content;
        private int currentDelay = 0;
        public SoundEffect shootSound;
        public int ammoCount;
        public int maxAmmo;
        public int reloadCount;
        public bool reloading;
        public int turnDrill;

        public Weapon(ContentManager contentManager, WeaponType type)
        {
            weaponType = type;
            content = contentManager;
            reloadCount = 0;
            switch (weaponType)
            {
                case WeaponType.AssaultRifle:
                    {
                        idleTexture = content.Load<Texture2D>("AssaultRifleIdle");
                        fireTexture = content.Load<Texture2D>("AssaultRifleFire");

                        clipMax = 30;

                        fireRate = 5;
                        accuracy = 10;
                        reloadDelay = 50;
                        shootSound = content.Load<SoundEffect>("shoot");
                        maxAmmo = 200;
                        break;
                    }
                case WeaponType.RocketLauncher:
                    {
                        idleTexture = content.Load<Texture2D>("RocketLauncherIdle");
                        fireTexture = content.Load<Texture2D>("RocketLauncherFire");
                        clipMax = 2;
                        fireRate = 50;
                        accuracy = 0;
                        reloadDelay = 100;
                        shootSound = content.Load<SoundEffect>("shoot");
                        maxAmmo = 10;
                        currentDelay = fireRate;
                        break;
                    }
                case WeaponType.Shotgun:
                    {
                        idleTexture = content.Load<Texture2D>("ShotgunIdle");
                        fireTexture = content.Load<Texture2D>("ShotgunFire");
                        clipMax = 6;
                        fireRate = 30;
                        accuracy = 0;
                        reloadDelay = 75;
                        shootSound = content.Load<SoundEffect>("shoot");
                        maxAmmo = 50;
                        currentDelay = fireRate;
                        break;
                    }
                case WeaponType.Turret:
                    {
                        idleTexture = content.Load<Texture2D>("Null");
                        fireTexture = content.Load<Texture2D>("Null");
                        clipMax = 100;
                        fireRate = 5;
                        accuracy = 0;
                        reloadDelay = 10;
                        shootSound = content.Load<SoundEffect>("shoot");
                        maxAmmo = 50;
                        currentDelay = fireRate;
                        break;
                    }
                case WeaponType.Drill:
                    {
                        idleTexture = content.Load<Texture2D>("DrillIdle");
                        clipMax = 6;
                        fireRate = 5;
                        accuracy = 0;
                        reloadDelay = 75;
                        shootSound = content.Load<SoundEffect>("shoot");
                        maxAmmo = 50;
                        currentDelay = fireRate;
                        break;
                    }
            }
            clipCount = clipMax;
            ammoCount = maxAmmo;
            currentWeaponTexture = idleTexture;
        }

        public enum WeaponType
        {
            AssaultRifle,
            RocketLauncher,
            Shotgun,
            Turret,
            Drill,
        }
        public void Reload(ref int newAmmo)
        {
            reloadCount++;
            if (reloadCount >= reloadDelay)
            {
                while (newAmmo > 0)
                {
                    if (clipCount >= clipMax)
                        break;

                    newAmmo--;
                    clipCount++;
                }
                reloading = false;
                reloadCount = 0;
            }
        }
        public void Update()
        {
            if (currentDelay != fireRate)
                currentDelay++;
            if (!reloading)
                reloadCount = 0;
            currentWeaponTexture = idleTexture;
        }
        public void Shoot(float rotation, bool inaccurate, Vector2 position, List<Bullet> bulletList, Vector2 destination,List<MapObject> mapObjectList)
        {
            if (currentDelay >= fireRate && clipCount > 0)
            {
                clipCount--;
                switch (weaponType)
                {
                    case WeaponType.AssaultRifle:
                        {
                            Random rand = new Random();
                            Bullet newBullet = new Bullet(content, Bullet.AmmoTypes.Bullet, Vector2.Zero);

                            if (inaccurate)
                                newBullet.Rotation = rotation + ((float)rand.Next(-accuracy, accuracy) / 100);
                            else
                                newBullet.Rotation = rotation + ((float)rand.Next(-accuracy, accuracy) / 200);

                            shootSound.Play();

                            newBullet.Position = position + new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 30;
                            newBullet.Velocity = new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 10f;
                            bulletList.Add(newBullet);
                            currentWeaponTexture = fireTexture;
                            newBullet.BulletRay = new Ray(new Vector3(newBullet.Position.X, newBullet.Position.Y, 0), new Vector3(newBullet.Velocity.X, newBullet.Velocity.Y, 0));
                            newBullet.DetermineTargetObject(mapObjectList);
                            currentDelay = 0;
                            break;
                        }
                    case WeaponType.RocketLauncher:
                        {
                            Random rand = new Random();
                            Bullet newBullet = new Bullet(content, Bullet.AmmoTypes.Rocket, destination);

                            newBullet.Rotation = rotation;

                            shootSound.Play();

                            newBullet.Position = position + new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 30;
                            newBullet.Velocity = new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 5f;
                            newBullet.Destination = destination;
                            bulletList.Add(newBullet);
                            currentWeaponTexture = fireTexture;
                            currentDelay = 0;
                            break;
                        }
                    case WeaponType.Shotgun:
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                Random rand = new Random();
                                Bullet newBullet = new Bullet(content, Bullet.AmmoTypes.Shotgun, Vector2.Zero);

                                if (i % 2 == 0)
                                    newBullet.Rotation = rotation + (i - 1) * ((float)rand.Next(-10, 10) / 100);
                                else
                                    newBullet.Rotation = rotation - (i) * ((float)rand.Next(-10, 10) / 100);

                                shootSound.Play();

                                newBullet.Position = position + new Vector2((float)(Math.Cos(rotation)), (float)(Math.Sin(rotation))) * 30;
                                newBullet.Velocity = new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 10f;
                                bulletList.Add(newBullet);
                                currentWeaponTexture = fireTexture;
                                currentDelay = 0;
                            }
                            break;
                        }
                    case WeaponType.Turret:
                        {
                            Random rand = new Random();
                            Bullet newBullet = new Bullet(content, Bullet.AmmoTypes.Bullet, Vector2.Zero);


                            newBullet.Rotation = rotation;

                            shootSound.Play();

                            newBullet.Position = position + new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 30;
                            newBullet.Velocity = new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 10f;
                            bulletList.Add(newBullet);
                            currentWeaponTexture = fireTexture;
                            currentDelay = 0;
                            break;

                        }

                }
            }

        }
        public void Drill(float rotation, Vector2 position, List<Bullet> bulletList, ref int newDrillAmmo)
        {
            turnDrill = turnDrill % 4 + 1;
            string name = "DrillFire" + turnDrill.ToString();

            currentWeaponTexture = content.Load<Texture2D>(name);
            if (currentDelay >= fireRate)
            {
                Bullet newBullet = new Bullet(content, Bullet.AmmoTypes.Melee, Vector2.Zero);
                newBullet.Rotation = rotation;
                newBullet.Position = position + new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 30;
             
                newBullet.Velocity = Vector2.Zero;
                bulletList.Add(newBullet);
                currentDelay = 0;
            }
        }
        public void Melee(float rotation, List<Bullet> bulletList, Vector2 position)
        {
            Bullet newBullet = new Bullet(content, Bullet.AmmoTypes.Melee, Vector2.Zero);
            newBullet.Rotation = rotation;
            newBullet.Position = position + new Vector2((float)(Math.Cos(newBullet.Rotation)), (float)(Math.Sin(newBullet.Rotation))) * 30;
            newBullet.Velocity = Vector2.Zero;
            bulletList.Add(newBullet);
            currentDelay = 0;
        }
    }
}
