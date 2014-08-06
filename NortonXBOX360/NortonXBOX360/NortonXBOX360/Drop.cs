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
    class Drop : Sprite
    {
        ItemType _Item;
        int _TimeAlive;
        int _FlashTime;
        bool _Flash;
        Random _Rand;

        public Drop(Vector2 newPosition, ContentManager content)
        {
            Randomizer = new Random();
            TimeAlive = 500;

            if (Item == ItemType.AMMO)
                Texture = content.Load<Texture2D>("AMMO");
            else if (Item == ItemType.CPU)
                Texture = content.Load<Texture2D>("CPU");
            else if (Item == ItemType.HDD)
                Texture = content.Load<Texture2D>("HDD");
            else if (Item == ItemType.RAM)
                Texture = content.Load<Texture2D>("RAM");
            else if (Item == ItemType.SSD)
                Texture = content.Load<Texture2D>("SSD");
            else if (Item == ItemType.CASE)
                Texture = content.Load<Texture2D>("CASE");

            Position = newPosition;
            Flash = true;
        }

        public enum ItemType
        {
            AMMO,
            CPU,
            HDD,
            RAM,
            SSD,
            CASE,
        }
        public override Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }
        public void Update(List<Player> playerList)
        {
            TimeAlive--;
            if (TimeAlive < 100)
            {
                if (Flash)
                {
                    if (FlashTime++ == 10)
                    {
                        Flash = false;
                        FlashTime = 0;
                    }
                }
                else
                {
                    if (FlashTime++ == 10)
                    {
                        Flash = true;
                        FlashTime = 0;
                    }
                }
            }

            foreach(Player player in playerList){
                if (player.GetRectangle().Intersects(GetRectangle()))
                {
                    TimeAlive = 0;

                    if (Item == ItemType.AMMO)
                    {
                        if (player.AssaultAmmoCount + 25 > player.AssaultAmmoMax)
                            player.AssaultAmmoCount = player.AssaultAmmoMax;
                        else
                            player.AssaultAmmoCount += 25;

                        if (player.RocketAmmoCount + 1 > player.RocketAmmoMax)
                            player.RocketAmmoCount = player.RocketAmmoMax;
                        else
                            player.RocketAmmoCount += 1;

                        if (player.ShotgunAmmoCount + 6 > player.ShotgunAmmoMax)
                            player.ShotgunAmmoCount = player.ShotgunAmmoMax;
                        else
                            player.ShotgunAmmoCount += 6;
                    }
                    else if (Item == ItemType.CPU)
                    {
                        player.MaxStamina += 5;
                    }
                    else if (Item == ItemType.HDD)
                    {
                        player.RocketAmmoMax += 1;
                    }
                    else if (Item == ItemType.SSD)
                    {
                        player.AssaultAmmoMax += 25;
                    }
                    else if (Item == ItemType.RAM)
                    {
                        player.Health = player.MaxHealth;
                    }
                    else if (Item == ItemType.CASE)
                    {
                        player.MaxHealth += 10;
                    }
                }
            }
        }

        public void Random()
        {
            switch (Randomizer.Next(1, 51))
            {
                case 1:
                    Item = ItemType.CPU;
                    break;
                case 2:
                    Item = ItemType.HDD;
                    break;
                case 3:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    Item = ItemType.RAM;
                    break;
                case 4:
                    Item = ItemType.SSD;
                    break;
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    Item = ItemType.AMMO;
                    break;
                case 21:
                    Item = ItemType.CASE;
                    break;
            }
        }

        public ItemType Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }

        public int TimeAlive
        {
            get
            {
                return _TimeAlive;
            }
            set
            {
                _TimeAlive = value;
            }
        }

        public int FlashTime
        {
            get
            {
                return _FlashTime;
            }
            set
            {
                _FlashTime = value;
            }
        }

        public bool Flash
        {
            get
            {
                return _Flash;
            }
            set
            {
                _Flash = value;
            }
        }

        public Random Randomizer
        {
            get
            {
                return _Rand;
            }
            set
            {
                _Rand = value;
            }
        }
    }
}
