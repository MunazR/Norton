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
    class Drop
    {
        Item item;
        public int timeAlive;
        public int flashTime;
        public bool flash;
        public Texture2D texture;
        public Vector2 position;
        private Random rand = new Random();

        public Drop(Vector2 newPosition, ContentManager content)
        {
            Random();
            timeAlive = 500;

            if (item == Item.AMMO)
                texture = content.Load<Texture2D>("AMMO");
            else if (item == Item.CPU)
                texture = content.Load<Texture2D>("CPU");
            else if (item == Item.HDD)
                texture = content.Load<Texture2D>("HDD");
            else if (item == Item.RAM)
                texture = content.Load<Texture2D>("RAM");
            else if (item == Item.SSD)
                texture = content.Load<Texture2D>("SSD");
            else if (item == Item.CASE)
                texture = content.Load<Texture2D>("CASE");

            position = newPosition;
            flash = true;
        }

        public enum Item 
        {
            AMMO,
            CPU,
            HDD,
            RAM,
            SSD,
            CASE,
        }

        public void Update(Player player)
        {
            timeAlive--;
            if (timeAlive < 100)
            {
                if (flash)
                {
                    if(flashTime++ == 10)
                    {
                        flash = false;
                        flashTime = 0;
                    }
                }
                else
                {
                    if(flashTime++ == 10)
                    {
                        flash = true;
                        flashTime = 0;
                    }
                }
            }

            if (player.GetRectangle.Intersects(GetRectangle))
           {
               timeAlive = 0;

               if (item == Item.AMMO)
               {
                       if (player.assaultAmmoCount + 25 > player.assaultAmmoMax)
                           player.assaultAmmoCount = player.assaultAmmoMax;
                       else
                           player.assaultAmmoCount += 25;

                       if (player.rocketAmmoCount + 1 > player.rocketAmmoMax)
                           player.rocketAmmoCount = player.rocketAmmoMax;
                       else
                           player.rocketAmmoCount += 1;

                       if (player.shotgunAmmoCount + 6 > player.shotgunAmmoMax)
                           player.shotgunAmmoCount = player.shotgunAmmoMax;
                       else
                           player.shotgunAmmoCount += 6;
               }
               else if (item == Item.CPU)
               {
                   player.maxStamina += 5;
               }
               else if (item == Item.HDD)
               {
                   player.rocketAmmoMax += 1;
               }
               else if (item == Item.SSD)
               {
                   player.assaultAmmoMax += 25;
               }
               else if (item == Item.RAM)
               {
                   player.health = player.maxHealth;
               }
               else if (item == Item.CASE)
               {
                   player.maxHealth += 10;
               }
           }
        }

        public Rectangle GetRectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        public void Random()
        {
            switch (rand.Next(1,51))
            {
                case 1:
                    item = Item.CPU;
                    break;
                case 2:
                    item = Item.HDD;
                    break;
                case 3:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    item = Item.RAM;
                    break;
                case 4:
                    item = Item.SSD;
                    break;
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    item = Item.AMMO;
                    break;
                case 21:
                    item = Item.CASE;
                    break;
            }
        }
    }
}
