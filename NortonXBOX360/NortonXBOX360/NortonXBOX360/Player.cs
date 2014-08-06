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

namespace NortonXBOX360
{
    class Player : Sprite
    {
        Texture2D _TextureBottom;
        float _RotationTop;
        float _RotationBottom ;
        bool _Alive;
        Vector2 _Direction;
        float _Speed;
        int _Health;
        bool _Reload;
        Weapon _Weapon1;
        Weapon _Weapon2;
        Weapon _Weapon3;
        Texture2D _CurrentWeaponTexture;
        bool _Moved;
        int _SpeedMultiplier;
        int _Stamina;
        int _MaxStamina;
        bool _StaminaDepleted;
        int _MaxHealth;
        ContentManager _Content;
        bool _Sprint;
        int _CurrentWeaponIndex;
        int _RocketAmmoCount;
        int _AssaultAmmoCount;
        int _ShotgunAmmoCount;
        int _DrillAmmoCount;
        int _RocketAmmoMax;
        int _AssaultAmmoMax;
        int _ShotgunAmmoMax;
        int _DrillAmmoMax;
        bool _Melee;
        int _AnimateCountBottom;
        int _AnimateMeleeCount;
        int _AnimateSprintCount;
        float _RotationMid;
        int _index;

        public Player(ContentManager ContentManager)
        {
            Content = ContentManager;
            Velocity = Vector2.Zero;
            this.Height = 40;
            this.Width = 40;
            Weapon1 = new Weapon(Content, Weapon.WeaponType.AssaultRifle);
            Weapon2 = new Weapon(Content, Weapon.WeaponType.Shotgun);
            Weapon3 = new Weapon(Content, Weapon.WeaponType.RocketLauncher);
            CurrentWeaponIndex = 1;
            Speed = 1f;
            Alive = true;
            TextureBottom = Content.Load<Texture2D>("PlayerWalk0");
            MaxHealth = 100;
            Health = MaxHealth;
            SpeedMultiplier = 8;
            StaminaDepleted = false;
            MaxStamina = 100;
            Stamina = MaxStamina;
            Origin = this.Center;
            RocketAmmoMax = 10;
            AssaultAmmoMax = 200;
            ShotgunAmmoMax = 50;
            DrillAmmoMax = 500;
            RocketAmmoCount = RocketAmmoMax;
            AssaultAmmoCount = AssaultAmmoMax;
            ShotgunAmmoCount = ShotgunAmmoMax;
            DrillAmmoCount = DrillAmmoMax;
            Index = 1;
        }

        public Weapon currentWeapon()
        {
            switch (CurrentWeaponIndex)
            {
                case 1:
                    return Weapon1;
                case 2:
                    return Weapon2;
                case 3:
                    return Weapon3;
            }
            return Weapon1;
        }

        public void ResetAmmoCount()
        {
            AssaultAmmoCount = AssaultAmmoMax;
            RocketAmmoCount = RocketAmmoMax;
            ShotgunAmmoCount = ShotgunAmmoMax;
        }
        public void Update(GameTime gameTime, List<MapObject> mapObjectList, ContentManager Content, List<Bullet> bulletList, Vector2 matrixOffset, List<Enemy> enemyList, List<Drop> dropList, GamePadState gamePadState)
        {
            Weapon1.Update();
            Weapon2.Update();
            Weapon3.Update();

            Move(gameTime, mapObjectList, matrixOffset, enemyList, bulletList, gamePadState);

            if (Reload)
                ReloadGun();

            if (!Sprint && !Reload)
                Shoot(bulletList, Content, matrixOffset, gamePadState,mapObjectList);

            Animate(Content, bulletList, enemyList, mapObjectList, dropList);
        }

        private void Move(GameTime gameTime, List<MapObject> mapObjectList, Vector2 matrixOffset, List<Enemy> enemyList, List<Bullet> bulletList, GamePadState gamePadState)
        {
            Velocity = Vector2.Zero;
            Direction = Vector2.Zero;
            Moved = false;
            Sprint = false;

            if (Stamina == 0)
            {
                StaminaDepleted = true;
                Stamina++;
            }

            else if (gamePadState.Buttons.LeftStick == ButtonState.Pressed && !StaminaDepleted)
            {
                Reload = false;
                currentWeapon().reloading = false;
                SpeedMultiplier = 12;
                Sprint = true;
                Stamina--;
            }
            else if (Stamina == MaxStamina)
            {
                StaminaDepleted = false;
                SpeedMultiplier = 4;
            }
            else
            {
                SpeedMultiplier = 4;
                Stamina++;
            }

            for (int i = 0; i < SpeedMultiplier; i++)
            {
                if (gamePadState.ThumbSticks.Left.X < 0 && !Collision(GetRectangle(), new Vector2(-Speed, 0), mapObjectList, enemyList))
                {
                    PositionX -= Speed * Math.Abs(gamePadState.ThumbSticks.Left.X);
                    DirectionX--;
                    Moved = true;
                }
                if (gamePadState.ThumbSticks.Left.X > 0 && !Collision(GetRectangle(), new Vector2(Speed, 0), mapObjectList, enemyList))
                {
                    PositionX += Speed * Math.Abs(gamePadState.ThumbSticks.Left.X);
                    DirectionX++;
                    Moved = true;
                }
                if (gamePadState.ThumbSticks.Left.Y > 0 && !Collision(GetRectangle(), new Vector2(0, -Speed), mapObjectList, enemyList))
                {
                    PositionY -= Speed * Math.Abs(gamePadState.ThumbSticks.Left.Y);
                    DirectionY--;
                    Moved = true;
                }
                if (gamePadState.ThumbSticks.Left.Y < 0 && !Collision(GetRectangle(), new Vector2(0, Speed), mapObjectList, enemyList))
                {
                    PositionY += Speed * Math.Abs(gamePadState.ThumbSticks.Left.Y);
                    DirectionY++;
                    Moved = true;
                }

                Direction.Normalize();
                Position += Velocity;
                if (Velocity != Vector2.Zero)
                    Velocity.Normalize();
            }

            if (gamePadState.ThumbSticks.Right.Y != 0 || gamePadState.ThumbSticks.Right.X != 0)
                RotationTop = -(float)Math.Atan2(gamePadState.ThumbSticks.Right.Y, gamePadState.ThumbSticks.Right.X);
            RotationMid = RotationTop;
            RotationBottom = (float)Math.Atan2(Direction.Y, Direction.X);

            if (gamePadState.Buttons.RightStick == ButtonState.Pressed && !Sprint)
            {
                Reload = false;
                currentWeapon().reloading = false;
                Melee = true;
            }
            if (Sprint && currentWeapon().clipCount <= 0) { }
            if ((gamePadState.Triggers.Left > 0.5f && !Reload && CurrentAmmo() > 0 || currentWeapon().clipCount <= 0) && currentWeapon().clipCount != currentWeapon().clipMax)
            {
                if (!Sprint && !Melee)
                {
                    Reload = true;
                    currentWeapon().reloading = true;
                }
            }

            if (gamePadState.DPad.Up == ButtonState.Pressed)
                CurrentWeaponIndex = 1;
            else if (gamePadState.DPad.Left == ButtonState.Pressed)
                CurrentWeaponIndex = 2;
            else if (gamePadState.DPad.Right == ButtonState.Pressed)
                CurrentWeaponIndex = 3;

        }
        private void Animate(ContentManager Content, List<Bullet> bulletList, List<Enemy> enemyList, List<MapObject> mapObjectList, List<Drop> dropList)
        {
            string name = null;
            if (Alive)
            {
                if (Moved)
                {
                    if (Sprint)
                    {
                        AnimateCountBottom++;
                        AnimateCountBottom = (AnimateCountBottom) % 52;
                        name = "PlayerWalk" + (AnimateCountBottom).ToString();
                        TextureBottom = Content.Load<Texture2D>(name);
                    }
                    else
                    {
                        AnimateCountBottom++;
                        AnimateCountBottom = AnimateCountBottom % 52;
                        name = "PlayerWalk" + (AnimateCountBottom).ToString();
                        TextureBottom = Content.Load<Texture2D>(name);
                    }
                }
                if (Melee)
                {
                    AnimateMeleeCount++;
                    if (AnimateMeleeCount <= 5)
                        RotationMid -= (AnimateMeleeCount / 5f);
                    else if (AnimateMeleeCount < 10)
                        RotationMid -= (2f - (AnimateMeleeCount / 10f));
                    else if (AnimateMeleeCount >= 20)
                        Melee = false;
                    if (AnimateMeleeCount == 5)
                        MeleeAction(enemyList, mapObjectList, bulletList, dropList);
                }
                else
                    AnimateMeleeCount = 0;

                if (Sprint)
                {
                    AnimateSprintCount++;
                    AnimateSprintCount = AnimateSprintCount % 60;

                    RotationMid -= 1.5f;
                    if (AnimateSprintCount > 45)
                        RotationMid += ((AnimateSprintCount - 45) / 20f);
                    else if (AnimateSprintCount > 30)
                        RotationMid -= ((AnimateSprintCount - 30) / 20f);
                    else if (AnimateSprintCount > 15)
                        RotationMid += ((AnimateSprintCount - 15) / 20f);
                    else
                        RotationMid -= (AnimateSprintCount / 20f);
                }
                else
                    AnimateSprintCount = 0;
            }
        }

        private void MeleeAction(List<Enemy> enemyList, List<MapObject> mapObjectList, List<Bullet> bulletList, List<Drop> dropList)
        {
            Weapon1.Melee(RotationTop, bulletList, Position );
        }
        private void ReloadGun()
        {
            switch (currentWeapon().weaponType)
            {
                case Weapon.WeaponType.AssaultRifle:
                    currentWeapon().Reload(ref _AssaultAmmoCount);
                    break;
                case Weapon.WeaponType.Shotgun:
                    currentWeapon().Reload(ref _ShotgunAmmoCount);
                    break;
                case Weapon.WeaponType.RocketLauncher:
                    currentWeapon().Reload(ref _RocketAmmoCount);
                    break;
            }
            if (!currentWeapon().reloading)
                Reload = false;
        }

        public int CurrentAmmo()
        {
            switch (currentWeapon().weaponType)
            {
                case Weapon.WeaponType.AssaultRifle:
                    return AssaultAmmoCount;
                case Weapon.WeaponType.Shotgun:
                    return ShotgunAmmoCount;
                case Weapon.WeaponType.RocketLauncher:
                    return RocketAmmoCount;
            }

            return 0;
        }

        private void Shoot(List<Bullet> bulletList, ContentManager Content, Vector2 matrixOffset, GamePadState gamePadState,List<MapObject> mapObjectList)
        {
            if (gamePadState.Triggers.Right > 0.5)
            {
                currentWeapon().Shoot(RotationTop, (Direction != Vector2.Zero), Position, bulletList, new Vector2(gamePadState.ThumbSticks.Right.X - matrixOffset.X, gamePadState.ThumbSticks.Right.Y - matrixOffset.Y),mapObjectList);
                if (currentWeapon().clipCount > 0)
                    GamePad.SetVibration(GetIndex(), 0.25f, 0.25f);
                else
                    GamePad.SetVibration(GetIndex(), 0, 0);
            }
            else
            {
                GamePad.SetVibration(GetIndex(), 0, 0);
            }
        }
        private bool Collision(Rectangle collisionArea, Vector2 newPosition, List<MapObject> mapObjectList, List<Enemy> enemyList)
        {
            collisionArea.X += (int)newPosition.X;
            collisionArea.Y += (int)newPosition.Y;

           for(int x = 0; x < mapObjectList.Count; x++)
                if (mapObjectList[x].GetRectangle().Intersects(collisionArea))
                    return true;

           for (int x = 0; x < enemyList.Count; x++)
                if (Vector2.DistanceSquared((Position + newPosition), enemyList[x].Position) < 1000 && enemyList[x].Alive)
                    return true;

            return false;
        }

        public void Damage(int damage)
        {
            Health -= damage;
        }

        public Texture2D TextureBottom
        {
            get
            {
                return _TextureBottom;
            }
            set
            {
                _TextureBottom = value;
            }
        }

        public float RotationTop
        {
            get
            {
                return _RotationTop;
            }
            set
            {
                _RotationTop = value;
            }
        }

        public float RotationBottom
        {
            get
            {
                return _RotationBottom;
            }
            set
            {
                _RotationBottom = value;
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

        public Vector2 Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;
            }
        }

        public float DirectionX
        {
            get
            {
                return _Direction.X;
            }
            set
            {
                _Direction.X = value;
            }
        }

        public float DirectionY
        {
            get
            {
                return _Direction.Y;
            }
            set
            {
                _Direction.Y = value;
            }
        }

        public float Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
            }
        }

        public int Health
        {
            get
            {
                return _Health;
            }
            set
            {
                _Health = value;
            }
        }

        public bool Reload
        {
            get
            {
                return _Reload;
            }
            set
            {
                _Reload = value;
            }
        }

        public Weapon Weapon1
        {
            get
            {
                return _Weapon1;
            }
            set
            {
                _Weapon1 = value;
            }
        }

        public Weapon Weapon2
        {
            get
            {
                return _Weapon2;
            }
            set
            {
                _Weapon2 = value;
            }
        }

        public Weapon Weapon3
        {
            get
            {
                return _Weapon3;
            }
            set
            {
                _Weapon3 = value;
            }
        }

        public Texture2D CurrentWeaponTexture
        {
            get
            {
                return _CurrentWeaponTexture;
            }
            set
            {
                _CurrentWeaponTexture = value;
            }
        }

        public bool Moved
        {
            get
            {
                return _Moved;
            }
            set
            {
                _Moved = value;
            }
        }

        public int SpeedMultiplier
        {
            get
            {
                return _SpeedMultiplier;
            }
            set
            {
                _SpeedMultiplier = value;
            }
        }

        public int Stamina
        {
            get
            {
                return _Stamina;
            }
            set
            {
                _Stamina = value;
            }
        }

        public int MaxStamina
        {
            get
            {
                return _MaxStamina;
            }
            set
            {
                _MaxStamina = value;
            }
        }

        public bool StaminaDepleted
        {
            get
            {
                return _StaminaDepleted;
            }
            set
            {
                _StaminaDepleted = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return _MaxHealth;
            }
            set
            {
                _MaxHealth = value;
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

        public bool Sprint
        {
            get
            {
                return _Sprint;
            }
            set
            {
                _Sprint = value;
            }
        }

        public int CurrentWeaponIndex
        {
            get
            {
                return _CurrentWeaponIndex;
            }
            set
            {
                _CurrentWeaponIndex = value;
            }
        }

        public int RocketAmmoCount
        {
            get
            {
                return _RocketAmmoCount;
            }
            set
            {
                _RocketAmmoCount = value;
            }
        }

        public int AssaultAmmoCount
        {
            get
            {
                return _AssaultAmmoCount;
            }
            set
            {
                _AssaultAmmoCount = value;
            }
        }

        public int ShotgunAmmoCount
        {
            get
            {
                return _ShotgunAmmoCount;
            }
            set
            {
                _ShotgunAmmoCount = value;
            }
        }

        public int DrillAmmoCount
        {
            get
            {
                return _DrillAmmoCount;
            }
            set
            {
                _DrillAmmoCount = value;
            }
        }

        public int RocketAmmoMax
        {
            get
            {
                return _RocketAmmoMax;
            }
            set
            {
                _RocketAmmoMax = value;
            }
        }

        public int AssaultAmmoMax
        {
            get
            {
                return _AssaultAmmoMax;
            }
            set
            {
                _AssaultAmmoMax = value;
            }
        }

        public int ShotgunAmmoMax
        {
            get
            {
                return _ShotgunAmmoMax;
            }
            set
            {
                _ShotgunAmmoMax = value;
            }
        }

        public int DrillAmmoMax
        {
            get
            {
                return _DrillAmmoMax;
            }
            set
            {
                _DrillAmmoMax = value;
            }
        }

        public bool Melee
        {
            get
            {
                return _Melee;
            }
            set
            {
                _Melee = value;
            }
        }

        public int AnimateMeleeCount
        {
            get
            {
                return _AnimateMeleeCount;
            }
            set
            {
                _AnimateMeleeCount = value;
            }
        }

        public int AnimateSprintCount
        {
            get
            {
                return _AnimateSprintCount;
            }
            set
            {
                _AnimateSprintCount = value;
            }
        }

        public int AnimateCountBottom
        {
            get
            {
                return _AnimateCountBottom;
            }
            set
            {
                _AnimateCountBottom = value;
            }
        }

        public float RotationMid
        {
            get
            {
                return _RotationMid;
            }
            set
            {
                _RotationMid = value;
            }
        }

        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        public PlayerIndex GetIndex()
        {
            if (_index == 1)
                return PlayerIndex.One;
            else
                return PlayerIndex.Two;   
        }
    }
}
