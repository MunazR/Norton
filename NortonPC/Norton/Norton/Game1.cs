/***************************************************
    PROGRAMME	:	Norton
          
    OUTLINE		:	Norton is a 2D top-down shooter
                    game that drives the player
                    through a story, has an available
                    arcade mode and also teaches
                    the user about computer components.
          
    PROGRAMMER	    :	Munaz Rahman, Haocheng Shen
          
     DATE			:	Janurary 13, 2014
***************************************************/

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
using System.IO;

namespace Norton
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState gameState = new GameState();

        //Level Variables
        Camera cameraOne;
        Vector2 matrixOffset = Vector2.Zero;
        Rectangle room = new Rectangle();
        string statusMessage;

        //Buttons
        Button startStoryButton;
        Button exitButton;
        Button startArcadeButton;
        Button retryButton;
        Button resumeButton;
        Button loadGamebutton;
        Button saveGameButton;
        Button highScoreButton;
        Button saveName;
        Button saveButton;
        Button loadButton;
        Button saveScoreButton;
        Button howToButton;

        string strSaveName;
        //Level 1 Variables
        //MapObject[,] lvl1Map = new MapObject[48, 72];

        List<Enemy> enemyList = new List<Enemy>();
        List<Player> playerList = new List<Player>();
        List<MapObject> mapObjectList = new List<MapObject>();
        List<Bullet> bulletList = new List<Bullet>();
        List<Texture2D> textureList = new List<Texture2D>();
        List<Drop> itemDropList = new List<Drop>();
        Rectangle endArea;

        Player playerOne;
        Texture2D background;

        //Arcade Variables
        int score;
        int wave;
        int enemyTotalCount;
        int enemyAliveAtOneTime;
        int waveCounter;
        List<Vector2> spawnLocations;

        int mapChosen;

        Button map1;
        Button map2;
        Button map3;

        //Textures
        Texture2D BlackWall; //1
        Texture2D BrownWall; //2
        Texture2D BlueWall; //3
        Texture2D CyanWall; //4
        Texture2D GrayWall; //5
        Texture2D GreenWall; //6
        Texture2D LightBlackWall; //7
        Texture2D LightBlueWall;//8
        Texture2D LightGreenWall;//9
        Texture2D LightPurpleWall;//0
        Texture2D OrangeWall;//a
        Texture2D PinkWall;//b
        Texture2D PurpleWall;//c
        Texture2D RedWall;//d
        Texture2D Sand;//e
        Texture2D Water;//f
        Texture2D Wood;//g
        Texture2D YellowWall;//h
        Texture2D Circle;//i
        Texture2D Bookshelf;//j
        Texture2D Chest;//k
        Texture2D CursorOn;//l
        Texture2D CursorOff;//m
        Texture2D Lava;//n
        Texture2D Lever;//o
        Texture2D Light;//p
        Texture2D BrickWall;//q
        Texture2D BlueLaser;//r
        Texture2D Crate; //s
        Texture2D Pumpkin; //t
        Texture2D DeadTree; //u
        Texture2D Tree; //v
        Texture2D Bush; //w
        Texture2D Cake; //x
        Texture2D Server; //z

        Texture2D MinimapTile;
        Texture2D RedTile;
        Texture2D GreenTile;
        Texture2D MinimapBackground;
        Texture2D endAreaTexture;
        Texture2D loading;
        Texture2D helpMenu;

        //Music
        Song music;

        //Sound Effects
        SoundEffect buttonPress;

        //Fonts
        SpriteFont Arial;
        SpriteFont ArialTitle;

        //PauseMenu Variables
        GameState prevGameState;

        //Cutscene Text
        string[] displayText;
        string[] cutsceneText;
        char[] textArray;
        int stage;
        int stage2;
        bool done;
        int textDelay;
        Texture2D cutsceneBackground;

        string[] highScores;

        Video intro;
        VideoPlayer player;

        KeyboardState prevKeyState;
        MouseState prevMouseState;

        enum GameState
        {
            LoadStartMenu,
            StartMenu,
            LoadCutscene1,
            Cutscene1,
            LoadCutscene2,
            Cutscene2,
            LoadCutscene3,
            Cutscene3,
            LoadCutscene4,
            Cutscene4,
            LoadCutscene5,
            Cutscene5,
            LoadCutscene6,
            Cutscene6,
            LoadCutscene7,
            Cutscene7,
            LoadCutscene8,
            Cutscene8,
            LoadCutscene9,
            Cutscene9,
            LoadLevel1,
            Level1,
            LoadLevel2,
            Level2,
            LoadLevel3,
            Level3,
            LoadLevel4,
            Level4,
            LoadLevel5,
            Level5,
            LoadLevel6,
            Level6,
            LoadLevel7,
            Level7,
            LoadLevel8,
            Level8,
            LoadArcade,
            LoadArcadeChooseMap,
            ArcadeChooseMap,
            ArcadeMode,
            NextWave,
            LoadHighScoreMenu,
            HighScore,
            LoadViewScores,
            ViewScores,
            WaveDelay,
            LoadGameOver,
            GameOver,
            LoadSaveMenu,
            SaveMenu,
            LoadLoadMenu,
            LoadMenu,
            LoadPauseMenu,
            PauseMenu,
            LoadHelpMenu,
            HelpMenu,
            LoadBuildMenu,
            BuildMenu,
            LoadEnd,
            End,
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 640;
            Window.Title = "Norton";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            cameraOne = new Camera(GraphicsDevice.Viewport);
            gameState = GameState.LoadStartMenu;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //playerOne.texture = Content.Load<Texture2D>("Norton");

            BlackWall = Content.Load<Texture2D>("BlackWall");
            BlueLaser = Content.Load<Texture2D>("BlueLaser");
            BlueWall = Content.Load<Texture2D>("BlueWall");
            Bookshelf = Content.Load<Texture2D>("Bookshelf");
            BrickWall = Content.Load<Texture2D>("BrickWall");
            BrownWall = Content.Load<Texture2D>("BrownWall");
            Chest = Content.Load<Texture2D>("Chest");
            Circle = Content.Load<Texture2D>("Circle");
            CursorOff = Content.Load<Texture2D>("CursorOff");
            CursorOn = Content.Load<Texture2D>("CursorOn");
            CyanWall = Content.Load<Texture2D>("CyanWall");
            GrayWall = Content.Load<Texture2D>("GrayWall");
            GreenWall = Content.Load<Texture2D>("GreenWall");
            Lava = Content.Load<Texture2D>("Lava");
            Lever = Content.Load<Texture2D>("Lever");
            Light = Content.Load<Texture2D>("Light");
            LightBlackWall = Content.Load<Texture2D>("LightBlackWall");
            LightBlueWall = Content.Load<Texture2D>("LightBlueWall");
            LightGreenWall = Content.Load<Texture2D>("LightGreenWall");
            LightPurpleWall = Content.Load<Texture2D>("LightPurpleWall");
            OrangeWall = Content.Load<Texture2D>("OrangeWall");
            PinkWall = Content.Load<Texture2D>("PinkWall");
            PurpleWall = Content.Load<Texture2D>("PurpleWall");
            RedWall = Content.Load<Texture2D>("RedWall");
            Sand = Content.Load<Texture2D>("Sand");
            Water = Content.Load<Texture2D>("Water");
            Wood = Content.Load<Texture2D>("wood");
            YellowWall = Content.Load<Texture2D>("YellowWall");
            Crate = Content.Load<Texture2D>("Crate");
            MinimapTile = Content.Load<Texture2D>("MinimapTile");
            RedTile = Content.Load<Texture2D>("redTile");
            GreenTile = Content.Load<Texture2D>("greenTile");
            Pumpkin = Content.Load<Texture2D>("Pumpkin");
            DeadTree = Content.Load<Texture2D>("DeadTree");
            Tree = Content.Load<Texture2D>("Tree");
            Bush = Content.Load<Texture2D>("Bush");
            Cake = Content.Load<Texture2D>("Cake");
            Server = Content.Load<Texture2D>("Server");
            loading = Content.Load<Texture2D>("LoadingScreen");
            helpMenu = Content.Load<Texture2D>("Controls");

            MinimapBackground = Content.Load<Texture2D>("MinimapBackground");

            intro = Content.Load<Video>("IntroVideo");
            player = new VideoPlayer();

            buttonPress = Content.Load<SoundEffect>("Button-1");

            playerOne = new Player(Content);
            playerOne.texture = Content.Load<Texture2D>("Norton");
            playerList.Add(playerOne);
            background = Content.Load<Texture2D>("WhiteTileBackground");

            Arial = Content.Load<SpriteFont>("Arial");
            ArialTitle = Content.Load<SpriteFont>("ArialTitle");
            endArea = new Rectangle();
            endArea.Height = 80;
            endArea.Width = 80;
            mapChosen = 0;
            statusMessage = "";
            strSaveName = "";
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (!this.IsActive)
                return;

            KeyboardState currentKeyState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();
            matrixOffset = cameraOne.inverse; ;


            if (currentKeyState.IsKeyDown(Keys.NumPad1))
                gameState = GameState.LoadLevel1;
            else if (currentKeyState.IsKeyDown(Keys.NumPad2))
                gameState = GameState.LoadLevel2;
            else if (currentKeyState.IsKeyDown(Keys.NumPad3))
                gameState = GameState.LoadLevel3;
            else if (currentKeyState.IsKeyDown(Keys.NumPad4))
                gameState = GameState.LoadLevel4;
            else if (currentKeyState.IsKeyDown(Keys.NumPad5))
                gameState = GameState.LoadLevel5;
            else if (currentKeyState.IsKeyDown(Keys.NumPad6))
                gameState = GameState.LoadLevel6;
            else if (currentKeyState.IsKeyDown(Keys.NumPad7))
                gameState = GameState.LoadLevel7;
            else if (currentKeyState.IsKeyDown(Keys.NumPad8))
                gameState = GameState.LoadLevel8;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (gameState == GameState.LoadStartMenu)
            {
                LoadStartMenu();
            }
            else if (gameState == GameState.StartMenu)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(startStoryButton.GetRectangle))
                    {
                        player.Stop();
                        buttonPress.Play();
                        gameState = GameState.LoadCutscene1;
                    }
                    else if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        player.Stop();
                        buttonPress.Play();
                        this.Exit();
                    }
                    else if (mouseRect.Intersects(startArcadeButton.GetRectangle))
                    {
                        player.Stop();
                        buttonPress.Play();
                        gameState = GameState.LoadArcadeChooseMap;
                    }
                    else if (mouseRect.Intersects(loadGamebutton.GetRectangle))
                    {
                        player.Stop();
                        buttonPress.Play();
                        gameState = GameState.LoadLoadMenu;
                    }
                    else if (mouseRect.Intersects(howToButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadHelpMenu;
                    }
                }
            }
            else if (gameState == GameState.LoadEnd)
            {
                LoadEnd();
                gameState = GameState.End;
            }
            else if (gameState == GameState.End)
            {

                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        player.Stop();
                        buttonPress.Play();
                        gameState = GameState.LoadLoadMenu;
                    }
                }
            }
            else if (gameState == GameState.LoadHighScoreMenu)
            {
                LoadHighScoreMenu();
                gameState = GameState.HighScore;
            }
            else if (gameState == GameState.LoadViewScores)
            {
                LoadViewScores();
                gameState = GameState.ViewScores;
            }
            else if (gameState == GameState.ViewScores)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        player.Stop();
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                };
            }
            else if (gameState == GameState.LoadCutscene1)
            {
                LoadCutscene(new StreamReader(@"cutscenes\1.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene1;
            }
            else if (gameState == GameState.LoadCutscene2)
            {
                LoadCutscene(new StreamReader(@"cutscenes\2.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene2;
            }
            else if (gameState == GameState.LoadCutscene3)
            {
                LoadCutscene(new StreamReader(@"cutscenes\3.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene3;
            }
            else if (gameState == GameState.LoadCutscene4)
            {
                LoadCutscene(new StreamReader(@"cutscenes\4.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene4;
            }
            else if (gameState == GameState.LoadCutscene5)
            {
                LoadCutscene(new StreamReader(@"cutscenes\5.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene5;
            }
            else if (gameState == GameState.LoadCutscene6)
            {
                LoadCutscene(new StreamReader(@"cutscenes\6.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene6;
            }
            else if (gameState == GameState.LoadCutscene7)
            {
                LoadCutscene(new StreamReader(@"cutscenes\7.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene7;
            }
            else if (gameState == GameState.LoadCutscene8)
            {
                LoadCutscene(new StreamReader(@"cutscenes\8.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene8;
            }
            else if (gameState == GameState.LoadCutscene9)
            {
                LoadCutscene(new StreamReader(@"cutscenes\9.txt"));
                cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                gameState = GameState.Cutscene9;
            }
            else if (gameState == GameState.LoadLevel1)
            {
                StreamReader map = new StreamReader(@"maps\level1.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldCPU");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                map.Close();

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                music = Content.Load<Song>("Music 1");

                background = Content.Load<Texture2D>("WhiteTileBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level1;
            }
            else if (gameState == GameState.LoadLevel2)
            {
                StreamReader map = new StreamReader(@"maps\level2.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldRAM");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                music = Content.Load<Song>("Music 3");

                background = Content.Load<Texture2D>("WhiteTileBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level2;
            }
            else if (gameState == GameState.LoadLevel3)
            {
                StreamReader map = new StreamReader(@"maps\level3.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldGPU");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                music = Content.Load<Song>("Music 2");

                background = Content.Load<Texture2D>("WhiteTileBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level3;
            }
            else if (gameState == GameState.LoadLevel4)
            {
                StreamReader map = new StreamReader(@"maps\level4.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldHDD");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                enemyTotalCount = 50;
                enemyAliveAtOneTime = 3;

                music = Content.Load<Song>("Music 1");

                background = Content.Load<Texture2D>("WhiteTileBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level4;
            }
            else if (gameState == GameState.LoadLevel5)
            {
                StreamReader map = new StreamReader(@"maps\level5.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldSSD");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                enemyTotalCount = 50;
                enemyAliveAtOneTime = 3;

                music = Content.Load<Song>("Music 2");

                background = Content.Load<Texture2D>("WhiteTileBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level5;
            }
            else if (gameState == GameState.LoadLevel6)
            {
                StreamReader map = new StreamReader(@"maps\level6.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldPSU");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                enemyTotalCount = 50;
                enemyAliveAtOneTime = 3;

                music = Content.Load<Song>("Music 1");

                background = Content.Load<Texture2D>("GrassBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level6;
            }
            else if (gameState == GameState.LoadLevel7)
            {
                StreamReader map = new StreamReader(@"maps\level7.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldMOBO");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                enemyTotalCount = 50;
                enemyAliveAtOneTime = 3;

                music = Content.Load<Song>("Music 3");

                background = Content.Load<Texture2D>("WhiteTileBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level7;
            }
            else if (gameState == GameState.LoadLevel8)
            {
                StreamReader map = new StreamReader(@"maps\level8.txt");

                endAreaTexture = Content.Load<Texture2D>("GoldCASE");

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                LoadLevel(map);

                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;

                enemyTotalCount = 50;
                enemyAliveAtOneTime = 3;

                music = Content.Load<Song>("Music 2");

                background = Content.Load<Texture2D>("WhiteTileBackground");
                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);
                gameState = GameState.Level8;
            }
            else if (gameState == GameState.LoadSaveMenu)
            {
                LoadSaveMenu();
            }
            else if (gameState == GameState.LoadLoadMenu)
            {
                LoadLoadMenu();
            }
            else if (gameState == GameState.SaveMenu)
            {
                if (currentKeyState.IsKeyDown(Keys.Back) && !prevKeyState.IsKeyDown(Keys.Back) && strSaveName.Length > 0)
                {
                    strSaveName = strSaveName.Substring(0, strSaveName.Length - 1);
                }
                else
                {
                    foreach (Keys key in currentKeyState.GetPressedKeys())
                        if (key.ToString().ToCharArray().Length == 1)
                            if (!(prevKeyState.IsKeyDown(key)) && Char.IsLetterOrDigit(key.ToString().ToCharArray()[0]))
                                strSaveName += key.ToString();
                }

                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(saveButton.GetRectangle))
                    {
                        buttonPress.Play();
                        statusMessage = SaveGame(strSaveName);
                    }
                    else if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = prevGameState;
                    }
                }
            }
            else if (gameState == GameState.LoadMenu)
            {
                if (currentKeyState.IsKeyDown(Keys.Back) && !prevKeyState.IsKeyDown(Keys.Back) && strSaveName.Length > 0)
                {
                    strSaveName = strSaveName.Substring(0, strSaveName.Length - 1);
                }
                else
                {
                    foreach (Keys key in currentKeyState.GetPressedKeys())
                        if (key.ToString().ToCharArray().Length == 1)
                            if (!(prevKeyState.IsKeyDown(key)) && Char.IsLetterOrDigit(key.ToString().ToCharArray()[0]))
                                strSaveName += key.ToString();
                }

                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(loadButton.GetRectangle))
                    {
                        buttonPress.Play();
                        statusMessage = LoadGame(strSaveName);
                    }
                    else if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                }
            }
            else if (gameState == GameState.HighScore)
            {
                if (currentKeyState.IsKeyDown(Keys.Back) && !prevKeyState.IsKeyDown(Keys.Back) && strSaveName.Length > 0)
                {
                    strSaveName = strSaveName.Substring(0, strSaveName.Length - 1);
                }
                else
                {
                    foreach (Keys key in currentKeyState.GetPressedKeys())
                        if (key.ToString().ToCharArray().Length == 1)
                            if (!(prevKeyState.IsKeyDown(key)) && Char.IsLetterOrDigit(key.ToString().ToCharArray()[0]))
                                strSaveName += key.ToString();
                }

                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(saveButton.GetRectangle))
                    {
                        buttonPress.Play();
                        statusMessage = SaveScore(strSaveName, score);
                    }
                    else if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                }
            }
            else if (gameState == GameState.LoadArcadeChooseMap)
            {
                LoadArcadeChooseMap();
            }
            else if (gameState == GameState.ArcadeChooseMap)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(map1.GetRectangle))
                    {
                        buttonPress.Play();
                        mapChosen = 1;
                        gameState = GameState.LoadArcade;
                    }
                    else if (mouseRect.Intersects(map2.GetRectangle))
                    {
                        buttonPress.Play();
                        mapChosen = 2;
                        gameState = GameState.LoadArcade;
                    }
                    else if (mouseRect.Intersects(map3.GetRectangle))
                    {
                        buttonPress.Play();
                        mapChosen = 3;
                        gameState = GameState.LoadArcade;
                    }
                    else if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                    else if (mouseRect.Intersects(highScoreButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadViewScores;
                    }
                }
            }
            else if (gameState == GameState.LoadArcade)
            {
                StreamReader map;

                switch (mapChosen)
                {
                    case 1:
                        map = new StreamReader(@"maps\arcade1.txt");
                        break;
                    case 2:
                        map = new StreamReader(@"maps\arcade2.txt");
                        break;
                    case 3:
                        map = new StreamReader(@"maps\arcade3.txt");
                        break;
                    default:
                        map = new StreamReader(@"maps\arcade1.txt");
                        break;
                }

                enemyList.Clear();
                mapObjectList.Clear();
                bulletList.Clear();
                itemDropList.Clear();

                spawnLocations = new List<Vector2>();
                LoadLevel(map);

                background = Content.Load<Texture2D>("WhiteTileBackground");
                playerOne.health = playerOne.maxHealth;
                playerOne.staminaDepleted = false;
                playerOne.stamina = playerOne.maxStamina;
                playerOne.ResetAmmoCount();

                score = 0;

                music = Content.Load<Song>("Music 1");

                MediaPlayer.Volume = 2;
                MediaPlayer.Play(music);

                wave = 0;
                gameState = GameState.NextWave;
            }
            else if (gameState == GameState.Level1 || gameState == GameState.Level2 || gameState == GameState.Level3 || gameState == GameState.Level4 || gameState == GameState.Level5 || gameState == GameState.Level6 || gameState == GameState.Level7 || gameState == GameState.Level8)
            {
                //******************************************************************************************
                foreach (Player player in playerList)
                    player.Update(gameTime, currentKeyState, currentMouseState, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList);

                foreach (Drop drop in itemDropList)
                    drop.Update(playerOne);

                for (int x = 0; x < itemDropList.Count; x++)
                    if (itemDropList[x].timeAlive <= 0)
                        itemDropList.Remove(itemDropList[x]);

                cameraOne.Update(gameTime, playerOne, room);
                foreach (Enemy enemy in enemyList)
                {
                    enemy.Update(playerOne, mapObjectList, Content, enemyList, bulletList);
                }

                for (int x = 0; x < enemyList.Count; x++)
                    if (enemyList[x].remove)
                        enemyList.Remove(enemyList[x--]);

                foreach (Bullet bullet in bulletList)
                {
                    bullet.Update(mapObjectList, enemyList, playerList, itemDropList, Content);
                }

                for (int i = 0; i < bulletList.Count; i++)
                {
                    if (bulletList[i].remove)
                        bulletList.Remove(bulletList[i]);
                }

                if (playerOne.health <= 0)
                {
                    prevGameState = gameState;
                    gameState = GameState.LoadGameOver;
                }

                if (currentKeyState.IsKeyDown(Keys.P))
                {
                    playerOne.health = playerOne.maxHealth;
                    playerOne.stamina = playerOne.maxStamina;
                }
                else if (currentKeyState.IsKeyDown(Keys.O))
                {
                    enemyList.Clear();
                }

                if (currentKeyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
                {
                    prevGameState = gameState;
                    gameState = GameState.LoadPauseMenu;
                }

                if (playerOne.GetRectangle.Intersects(endArea))
                {
                    if (enemyList.Count != 0)
                    {
                        statusMessage = "Clear all enemies first!";
                    }
                    else
                    {
                        if (gameState == GameState.Level1)
                            gameState = GameState.LoadCutscene2;
                        else if (gameState == GameState.Level2)
                            gameState = GameState.LoadCutscene3;
                        else if (gameState == GameState.Level3)
                            gameState = GameState.LoadCutscene4;
                        else if (gameState == GameState.Level4)
                            gameState = GameState.LoadCutscene5;
                        else if (gameState == GameState.Level5)
                            gameState = GameState.LoadCutscene6;
                        else if (gameState == GameState.Level6)
                            gameState = GameState.LoadCutscene7;
                        else if (gameState == GameState.Level7)
                            gameState = GameState.LoadCutscene8;
                        else if (gameState == GameState.Level8)
                            gameState = GameState.LoadCutscene9;
                    }
                }
                else if (!playerOne.GetRectangle.Intersects(endArea))
                {
                    statusMessage = "";
                }
                //******************************************************************************************
            }
            else if (gameState == GameState.ArcadeMode || gameState == GameState.WaveDelay)
            {
                //******************************************************************************************
                int enemyCount;
                Random rand = new Random();

                enemyCount = enemyList.Count;

                foreach (Player player in playerList)
                    player.Update(gameTime, currentKeyState, currentMouseState, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList);

                foreach (Drop drop in itemDropList)
                    drop.Update(playerOne);

                for (int x = 0; x < itemDropList.Count; x++)
                    if (itemDropList[x].timeAlive <= 0)
                        itemDropList.Remove(itemDropList[x]);

                cameraOne.Update(gameTime, playerOne, room);

                foreach (Enemy enemy in enemyList)
                    enemy.Update(playerOne, mapObjectList, Content, enemyList, bulletList);

                for (int x = 0; x < enemyList.Count; x++)
                    if (enemyList[x].remove)
                        enemyList.Remove(enemyList[x--]);

                foreach (Bullet bullet in bulletList)
                    bullet.Update(mapObjectList, enemyList, playerList, itemDropList, Content);

                for (int i = 0; i < bulletList.Count; i++)
                {
                    if (bulletList[i].remove)
                        bulletList.Remove(bulletList[i]);
                }

                if (playerOne.health <= 0)
                {
                    prevGameState = gameState;
                    gameState = GameState.LoadGameOver;
                }

                if (currentKeyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
                {
                    prevGameState = gameState;
                    gameState = GameState.LoadPauseMenu;
                }

                if (gameState == GameState.WaveDelay)
                {
                    waveCounter -= 1;
                    if (waveCounter <= 0)
                        gameState = GameState.ArcadeMode;
                }

                if (gameState == GameState.ArcadeMode)
                {
                    score += (enemyCount - enemyList.Count);

                    enemyTotalCount -= (enemyCount - enemyList.Count);

                    if (enemyList.Count < enemyAliveAtOneTime && enemyTotalCount != enemyList.Count)
                    {
                        if (wave < 5)
                        {
                            SpawnEnemy("Shooter", spawnLocations);
                        }
                        else if (wave < 10)
                        {
                            switch (rand.Next(1, 3))
                            {
                                case 1:
                                    SpawnEnemy("Shooter", spawnLocations);
                                    break;
                                case 2:
                                    SpawnEnemy("Chaser", spawnLocations);
                                    break;
                                case 3:
                                    SpawnEnemy("Turret", spawnLocations);
                                    break;
                            }
                        }
                        else
                        {
                            switch (rand.Next(1, 4))
                            {
                                case 1:
                                    SpawnEnemy("Shooter", spawnLocations);
                                    break;
                                case 2:
                                    SpawnEnemy("Chaser", spawnLocations);
                                    break;
                                case 3:
                                    SpawnEnemy("Turret", spawnLocations);
                                    break;
                                case 4:
                                    SpawnEnemy("Heavy", spawnLocations);
                                    break;
                            }
                        }
                    }

                    if (enemyTotalCount <= 0)
                        gameState = GameState.NextWave;
                }
                //******************************************************************************************
            }
            else if (gameState == GameState.NextWave)
            {
                wave += 1;

                enemyTotalCount = (int)Math.Pow(wave, 2);
                enemyAliveAtOneTime = wave;

                waveCounter = 600;
                gameState = GameState.WaveDelay;
            }
            else if (gameState == GameState.LoadPauseMenu)
            {
                MediaPlayer.Pause();
                LoadPauseMenu();
                gameState = GameState.PauseMenu;
            }
            else if (gameState == GameState.PauseMenu)
            {
                if (currentKeyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
                {
                    MediaPlayer.Resume();
                    gameState = prevGameState;
                }

                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(resumeButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = prevGameState;
                        MediaPlayer.Resume();
                    }
                    else if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                    else if ((prevGameState != GameState.WaveDelay) && (prevGameState != GameState.ArcadeMode) && mouseRect.Intersects(saveGameButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadSaveMenu;
                    }
                }
            }
            else if (gameState == GameState.Cutscene1)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space) && !prevKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel1;

            }

            else if (gameState == GameState.Cutscene2)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel2;

            }

            else if (gameState == GameState.Cutscene3)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel3;

            }
            else if (gameState == GameState.Cutscene4)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel4;

            }
            else if (gameState == GameState.Cutscene5)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel5;

            }
            else if (gameState == GameState.Cutscene6)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel6;

            }
            else if (gameState == GameState.Cutscene7)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel7;

            }
            else if (gameState == GameState.Cutscene8)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadLevel8;

            }
            else if (gameState == GameState.Cutscene9)
            {
                if (!done)
                {
                    if (textDelay++ >= 2)
                    {
                        displayText[stage] = "";
                        textArray = cutsceneText[stage].ToCharArray();

                        for (int x = 0; x < stage2; x++)
                        {
                            displayText[stage] += textArray[x];
                        }

                        stage2++;

                        if (stage2 == textArray.Length)
                        {
                            stage2 = 0;
                            stage++;
                        }

                        if (stage == cutsceneText.Length)
                            done = true;

                        textDelay = 0;
                    }
                }

                if (currentKeyState.IsKeyDown(Keys.Space))
                    gameState = GameState.LoadEnd;

            }
            else if (gameState == GameState.LoadHelpMenu)
            {
                LoadHelpMenu();
                gameState = GameState.HelpMenu;
            }
            else if (gameState == GameState.HelpMenu)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                }
            }
            else if (gameState == GameState.LoadGameOver)
            {
                MediaPlayer.Stop();
                LoadGameOver();
            }
            else if (gameState == GameState.GameOver)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Rectangle mouseRect = new Rectangle(currentMouseState.X, currentMouseState.Y, CursorOn.Width, CursorOn.Height);

                    if (mouseRect.Intersects(retryButton.GetRectangle))
                    {
                        buttonPress.Play();

                        switch (prevGameState)
                        {
                            case GameState.Level1:
                                gameState = GameState.LoadLevel1;
                                break;
                            case GameState.Level2:
                                gameState = GameState.LoadLevel2;
                                break;
                            case GameState.Level3:
                                gameState = GameState.LoadLevel3;
                                break;
                            case GameState.Level4:
                                gameState = GameState.LoadLevel4;
                                break;
                            case GameState.Level5:
                                gameState = GameState.LoadLevel5;
                                break;
                            case GameState.Level6:
                                gameState = GameState.LoadLevel6;
                                break;
                            case GameState.Level7:
                                gameState = GameState.LoadLevel7;
                                break;
                            case GameState.Level8:
                                gameState = GameState.LoadLevel8;
                                break;
                            case GameState.ArcadeMode:
                                gameState = GameState.LoadArcade;
                                break;
                        }
                    }
                    else if (mouseRect.Intersects(exitButton.GetRectangle))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                    else if (mouseRect.Intersects(saveScoreButton.GetRectangle) && prevGameState == GameState.ArcadeMode)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadHighScoreMenu;
                    }
                }
            }
            // Put your code here Hoacheng
            else if (gameState == GameState.LoadBuildMenu)
            {

                gameState = GameState.BuildMenu;
            }
            else if (gameState == GameState.BuildMenu)
            {
                //**********************************************************************************************************************************************

            }

            prevKeyState = currentKeyState;
            prevMouseState = currentMouseState;

            base.Update(gameTime);
        }

        private void LoadHelpMenu()
        {
            exitButton = new Button();
            exitButton.texture = Content.Load<Texture2D>("ExitButton");
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 500);
        }

        private string SaveScore(string playerName, int playerScore)
        {
            try
            {
                StreamReader oldFile = new StreamReader("highscore.txt");
                StreamWriter newFile;
                string[] scores;
                string[] names;
                string[] data;
                string input;
                string oldNames = "";
                string oldScores = "";


                while ((input = oldFile.ReadLine()) != null)
                {
                    data = input.Split();
                    oldNames += data[0] + "~";
                    oldScores += data[1] + "~";
                }

                oldFile.Close();

                oldScores += playerScore.ToString();
                oldNames += playerName;

                scores = oldScores.Split('~');
                names = oldNames.Split('~');

                for (int x = 0; x < scores.Length - 1; x++)
                {
                    if ((int.Parse(scores[x]) < int.Parse(scores[x + 1])))
                    {
                        oldNames = names[x];
                        oldScores = scores[x];

                        names[x] = names[x + 1];
                        scores[x] = scores[x + 1];

                        names[x + 1] = oldNames;
                        scores[x + 1] = oldScores;
                    }
                }

                newFile = new StreamWriter("highscore.txt");

                for (int x = 0; x < scores.Length; x++)
                {
                    newFile.WriteLine(names[x] + " " + scores[x]);
                }

                newFile.Close();
            }
            catch (IOException exc)
            {
                return ("Save Failed: " + exc);
            }
            return "Score Saved!";
        }

        private void SpawnEnemy(string newEnemyType, List<Vector2> spawns)
        {
            Random rand = new Random();

            Enemy enemy = new Enemy(Content, newEnemyType);

            enemy.position = spawns[rand.Next(0, spawns.Count)];
            enemy.aggro = true;
            enemyList.Add(enemy);
        }

        private void SpawnEnemy(string newEnemyType)
        {
            Random rand = new Random();
            Vector2 position;

            Enemy enemy = new Enemy(Content, newEnemyType);

            do
            {
                position = new Vector2(rand.Next(0, GraphicsDevice.Viewport.Width), rand.Next(0, GraphicsDevice.Viewport.Height));
                enemy.position = position;
            } while ((enemy.Collision(enemy.GetRectangle, Vector2.Zero, mapObjectList, playerOne, enemyList)) && !(Vector2.Distance(enemy.position, playerOne.position) > (960))/*&& !(Vector2.Distance(enemy.position, playerOne.position) > 50) && !(Vector2.Distance(enemy.position, playerOne.position) < 100)*/);

            enemy.position = position;
            enemy.aggro = true;
            enemyList.Add(enemy);
        }

        private void SpawnEnemy(Vector2 position, string newEnemyType)
        {
            Random rand = new Random();

            Enemy enemy = new Enemy(Content, newEnemyType);

            enemy.position = position;

            enemy.position = position;
            enemyList.Add(enemy);
        }

        private void LoadHighScoreMenu()
        {
            saveName = new Button();
            saveName.texture = Content.Load<Texture2D>("SaveName");
            saveName.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (saveName.texture.Width / 2), 200);

            exitButton = new Button();
            exitButton.texture = Content.Load<Texture2D>("ExitButton");
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 500);

            saveButton = new Button();
            saveButton.texture = Content.Load<Texture2D>("SaveButton");
            saveButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 400);
        }

        private void LoadViewScores()
        {
            StreamReader inFile = new StreamReader("highscore.txt");
            string input;
            string scores = "";

            while ((input = inFile.ReadLine()) != null)
            {
                scores += input + "~";
            }

            inFile.Close();
            highScores = scores.Split('~');

            exitButton = new Button();
            exitButton.texture = Content.Load<Texture2D>("ExitButton");
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 500);
        }

        private string SaveGame(string name)
        {
            if (name == "")
                return "Enter a save name!";

            try
            {
                if (File.Exists(name + ".sav"))
                    File.Delete(name + ".sav");

                StreamWriter saveFile = new StreamWriter(name + ".sav");

                if (prevGameState == GameState.Level1)
                    saveFile.WriteLine(1);
                else if (prevGameState == GameState.Level2)
                    saveFile.WriteLine(2);
                else if (prevGameState == GameState.Level3)
                    saveFile.WriteLine(3);
                else if (prevGameState == GameState.Level4)
                    saveFile.WriteLine(4);
                else if (prevGameState == GameState.Level5)
                    saveFile.WriteLine(5);
                else if (prevGameState == GameState.Level6)
                    saveFile.WriteLine(6);
                else if (prevGameState == GameState.Level7)
                    saveFile.WriteLine(7);
                else if (prevGameState == GameState.Level8)
                    saveFile.WriteLine(8);

                saveFile.WriteLine(playerOne.maxHealth);
                saveFile.WriteLine(playerOne.maxStamina);
                saveFile.WriteLine(playerOne.assaultAmmoCount);
                saveFile.WriteLine(playerOne.assaultAmmoMax);
                saveFile.WriteLine(playerOne.shotgunAmmoCount);
                saveFile.WriteLine(playerOne.shotgunAmmoMax);
                saveFile.WriteLine(playerOne.rocketAmmoCount);
                saveFile.WriteLine(playerOne.rocketAmmoMax);
                saveFile.Close();
            }
            catch (IOException exc)
            {
                return "Save Failed: " + exc;
            }
            return "Save Successfull";
        }

        private string LoadGame(string name)
        {
            string input;
            int level = 0;
            int count = 1;

            if (!File.Exists(name + ".sav"))
                return ("Save does not exist!");

            try
            {
                StreamReader saveFile = new StreamReader(name + ".sav");

                while ((input = saveFile.ReadLine()) != null)
                {
                    if (count == 1)
                        level = int.Parse(input);
                    else if (count == 2)
                        playerOne.maxHealth = int.Parse(input);
                    else if (count == 3)
                        playerOne.maxStamina = int.Parse(input);
                    else if (count == 4)
                        playerOne.assaultAmmoCount = int.Parse(input);
                    else if (count == 5)
                        playerOne.assaultAmmoMax = int.Parse(input);
                    else if (count == 6)
                        playerOne.shotgunAmmoCount = int.Parse(input);
                    else if (count == 7)
                        playerOne.shotgunAmmoMax = int.Parse(input);
                    else if (count == 8)
                        playerOne.rocketAmmoCount = int.Parse(input);
                    else if (count == 9)
                        playerOne.rocketAmmoMax = int.Parse(input);

                    count++;
                }

                if (level == 1)
                    gameState = GameState.LoadLevel1;
                else if (level == 2)
                    gameState = GameState.LoadLevel2;
                else if (level == 3)
                    gameState = GameState.LoadLevel3;
                else if (level == 4)
                    gameState = GameState.LoadLevel4;
                else if (level == 5)
                    gameState = GameState.LoadLevel5;
                else if (level == 6)
                    gameState = GameState.LoadLevel6;
                else if (level == 7)
                    gameState = GameState.LoadLevel7;
                else if (level == 8)
                    gameState = GameState.LoadLevel8;

                saveFile.Close();
            }
            catch (IOException exc)
            {
                return "Load failed: " + exc;
            }
            return "Load Successfull";
        }



        private void LoadCutscene(StreamReader inFile)
        {
            string input;
            string allText = "";
            stage = 0;
            stage2 = 0;
            done = false;

            while ((input = inFile.ReadLine()) != null)
            {
                allText += input + "|";
            }

            allText += "Press space to start!";

            cutsceneText = allText.Split('|');

            displayText = new string[cutsceneText.Length];

            for (int x = 0; x < displayText.Length; x++)
            {
                displayText[x] = "";
            }
        }

        private void LoadEnd()
        {
            exitButton = new Button();
            exitButton.texture = Content.Load<Texture2D>("exitButton");
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 400);
        }

        private void LoadPauseMenu()
        {
            resumeButton = new Button();
            resumeButton.texture = Content.Load<Texture2D>("ResumeButton");
            resumeButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.texture.Width / 2), 200);

            saveGameButton = new Button();
            saveGameButton.texture = Content.Load<Texture2D>("SaveGameButton");
            saveGameButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.texture.Width / 2), 300);

            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 400);
        }

        private void LoadSaveMenu()
        {
            saveName = new Button();
            saveName.texture = Content.Load<Texture2D>("SaveName");
            saveName.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (saveName.texture.Width / 2), 200);

            exitButton = new Button();
            exitButton.texture = Content.Load<Texture2D>("ExitButton");
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 500);

            saveButton = new Button();
            saveButton.texture = Content.Load<Texture2D>("SaveButton");
            saveButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 400);

            gameState = GameState.SaveMenu;
        }

        private void LoadLoadMenu()
        {
            saveName = new Button();
            saveName.texture = Content.Load<Texture2D>("SaveName");
            saveName.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (saveName.texture.Width / 2), 200);

            exitButton = new Button();
            exitButton.texture = Content.Load<Texture2D>("ExitButton");
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 500);

            loadButton = new Button();
            loadButton.texture = Content.Load<Texture2D>("LoadButton");
            loadButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 400);

            gameState = GameState.LoadMenu;
        }

        private void LoadStartMenu()
        {
            startStoryButton = new Button();
            loadGamebutton = new Button();
            startArcadeButton = new Button();
            exitButton = new Button();
            howToButton = new Button();

            startStoryButton.texture = Content.Load<Texture2D>("StartStoryButton");
            loadGamebutton.texture = Content.Load<Texture2D>("LoadGameButton");
            startArcadeButton.texture = Content.Load<Texture2D>("StartArcadeButton");
            exitButton.texture = Content.Load<Texture2D>("ExitButton");
            howToButton.texture = Content.Load<Texture2D>("HowToButton");

            loadGamebutton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startStoryButton.texture.Width / 2), 150);
            startStoryButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startStoryButton.texture.Width / 2), 250);
            startArcadeButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startArcadeButton.texture.Width / 2), 350);
            howToButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startArcadeButton.texture.Width / 2), 450);
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 550);

            player.IsLooped = true;
            player.Play(intro);

            gameState = GameState.StartMenu;
        }

        private void LoadArcadeChooseMap()
        {
            map1 = new Button();
            map2 = new Button();
            map3 = new Button();
            highScoreButton = new Button();
            exitButton = new Button();

            map1.texture = Content.Load<Texture2D>("Map1");
            map2.texture = Content.Load<Texture2D>("Map2");
            map3.texture = Content.Load<Texture2D>("Map3");

            map1.position = new Vector2(40, 200);
            map2.position = new Vector2(360, 200);
            map3.position = new Vector2(680, 200);

            highScoreButton.texture = Content.Load<Texture2D>("HighScoreButton");
            exitButton.texture = Content.Load<Texture2D>("ExitButton");
            highScoreButton.position = new Vector2(50, 500);
            exitButton.position = new Vector2(910 - exitButton.texture.Width, 500);

            gameState = GameState.ArcadeChooseMap;
        }

        private void LoadGameOver()
        {
            retryButton = new Button();
            exitButton = new Button();
            saveScoreButton = new Button();

            retryButton.texture = Content.Load<Texture2D>("RetryButton");
            exitButton.texture = Content.Load<Texture2D>("ExitButton");

            retryButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startStoryButton.texture.Width / 2), 200);
            exitButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.texture.Width / 2), 400);

            saveScoreButton.texture = Content.Load<Texture2D>("SaveScoreButton");
            saveScoreButton.position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startStoryButton.texture.Width / 2), 300);

            MediaPlayer.Stop();

            playerOne.health = playerOne.maxHealth;
            gameState = GameState.GameOver;
        }

        private void LoadLevel(StreamReader map)
        {
            int count = 0;
            int row = 0;
            string input;
            MapObject mapObj;

            endArea = new Rectangle(0, 0, 0, 0);

            while (!((input = map.ReadLine()) == null))
            {
                count = 0;
                foreach (Char tile in input)
                {
                    mapObj = new MapObject();
                    switch (tile)
                    {
                        case '1':
                            mapObj.Texture = BlackWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '2':
                            mapObj.Texture = BrownWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '3':
                            mapObj.Texture = BlueWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '4':
                            mapObj.Texture = CyanWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '5':
                            mapObj.Texture = GrayWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '6':
                            mapObj.Texture = GreenWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '7':
                            mapObj.Texture = LightBlackWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '8':
                            mapObj.Texture = LightBlueWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '9':
                            mapObj.Texture = LightGreenWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '0':
                            mapObj.Texture = LightPurpleWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'a':
                            mapObj.Texture = OrangeWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'b':
                            mapObj.Texture = PinkWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'c':
                            mapObj.Texture = PurpleWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'C':
                            SpawnEnemy(new Vector2(count * 40, row * 40), "Chaser");
                            break;
                        case 'd':
                            mapObj.Texture = RedWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'e':
                            mapObj.Texture = Sand;
                            mapObj.Collision = false;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'f':
                            mapObj.Texture = Water;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'g':
                            mapObj.Texture = Wood;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'h':
                            mapObj.Texture = YellowWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'H':
                            SpawnEnemy(new Vector2(count * 40, row * 40), "Heavy");
                            break;
                        case 'i':
                            mapObj.Texture = Circle;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'j':
                            mapObj.Texture = Bookshelf;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObj.Collision = true;
                            mapObjectList.Add(mapObj);
                            break;
                        case 'k':
                            mapObj.Texture = Chest;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'l':
                            mapObj.Texture = CursorOn;
                            mapObj.Collision = false;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'm':
                            mapObj.Texture = CursorOff;
                            mapObj.Collision = false;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'n':
                            mapObj.Texture = Lava;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'o':
                            mapObj.Texture = Lever;
                            mapObj.Collision = false;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'p':
                            mapObj.Texture = Light;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'q':
                            mapObj.Texture = BrickWall;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'r':
                            mapObj.Texture = BlueLaser;
                            mapObj.Collision = false;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 's':
                            mapObj.Texture = Crate;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 't':
                            mapObj.Texture = Pumpkin;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'u':
                            mapObj.Texture = DeadTree;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'v':
                            mapObj.Texture = Tree;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'w':
                            mapObj.Texture = Bush;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'x':
                            mapObj.Texture = Cake;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'z':
                            mapObj.Texture = Server;
                            mapObj.Collision = true;
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'I':
                            spawnLocations.Add(new Vector2(count * 40, row * 40));
                            break;
                        case 'P':
                            playerOne.position = new Vector2(count * playerOne.texture.Width, row * playerOne.texture.Height);
                            break;
                        case 'O':
                            SpawnEnemy(new Vector2(count * 40, row * 40), "Shooter");
                            break;
                        case 'T':
                            SpawnEnemy(new Vector2(count * 40, row * 40), "Turret");
                            break;
                        case 'Z':
                            endArea.Width = 80;
                            endArea.Height = 80;
                            endArea.X = count * 40;
                            endArea.Y = row * 40;
                            break;
                    }
                    count++;
                }

                if (room.Width < count)
                    room.Width = count;
                row++;
            }


            room.Height = 2160;


            room.Width = 2160;
        }

        private void playerBounds(Player player, int minX, int minY, int maxX, int maxY)
        {
            if (player.position.X > maxX - 20)
                player.position.X = maxX - 20;
            else if (player.position.X < minX + 20)
                player.position.X = minX + 20;

            //Y Value
            if (player.position.Y > maxY - 20)
                player.position.Y = maxY - 20;
            else if (player.position.Y < minY + 20)
                player.position.Y = minY + 20;
        }
        protected override void Draw(GameTime gameTime)
        {
            if (gameState == GameState.StartMenu)
            {
                Texture2D videoTexture;

                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.LightGreen);

                if ((videoTexture = player.GetTexture()) != null)
                {
                    spriteBatch.Draw(videoTexture, Vector2.Zero, Color.White);
                }


                Vector2 FontOrigin = ArialTitle.MeasureString("Norton") / 2;
                spriteBatch.DrawString(ArialTitle, "Norton", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.Draw(startStoryButton.texture, startStoryButton.position, Color.White);
                spriteBatch.Draw(loadGamebutton.texture, loadGamebutton.position, Color.White);
                spriteBatch.Draw(startArcadeButton.texture, startArcadeButton.position, Color.White);
                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);
                spriteBatch.Draw(howToButton.texture, howToButton.position, Color.White);

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);

                spriteBatch.End();
            }
            else if (gameState == GameState.HelpMenu)
            {
                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.LightGreen);
                spriteBatch.Draw(helpMenu, Vector2.Zero, Color.White);
                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                spriteBatch.End();
            }
            else if (gameState == GameState.ArcadeChooseMap)
            {
                spriteBatch.Begin();

                GraphicsDevice.Clear(Color.LightGreen);
                Vector2 FontOrigin = ArialTitle.MeasureString("Arcade Mode") / 2;
                spriteBatch.DrawString(ArialTitle, "Arcade Mode", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.Draw(map1.texture, map1.position, Color.White);
                spriteBatch.Draw(map2.texture, map2.position, Color.White);
                spriteBatch.Draw(map3.texture, map3.position, Color.White);

                spriteBatch.Draw(highScoreButton.texture, highScoreButton.position, Color.White);
                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                spriteBatch.End();
            }
            else if (gameState == GameState.SaveMenu || gameState == GameState.LoadMenu)
            {
                spriteBatch.Begin();
                Vector2 FontOrigin;

                GraphicsDevice.Clear(Color.LightGreen);
                FontOrigin = ArialTitle.MeasureString("Enter Save Name") / 2;
                spriteBatch.DrawString(ArialTitle, "Enter Save Name", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.Draw(saveName.texture, saveName.position, Color.White);
                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);

                if (gameState == GameState.SaveMenu)
                    spriteBatch.Draw(saveButton.texture, saveButton.position, Color.White);
                else if (gameState == GameState.LoadMenu)
                    spriteBatch.Draw(loadButton.texture, loadButton.position, Color.White);

                FontOrigin = ArialTitle.MeasureString(strSaveName) / 2;
                spriteBatch.DrawString(ArialTitle, strSaveName, saveName.position + new Vector2(saveName.texture.Width / 2, saveName.texture.Height / 2), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                FontOrigin = Arial.MeasureString(statusMessage) / 2;
                spriteBatch.DrawString(Arial, statusMessage, new Vector2(GraphicsDevice.Viewport.Width / 2, saveName.position.Y + saveName.texture.Height + 25), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                spriteBatch.End();
            }
            else if (gameState == GameState.HighScore)
            {
                spriteBatch.Begin();
                Vector2 FontOrigin;

                GraphicsDevice.Clear(Color.LightGreen);
                FontOrigin = ArialTitle.MeasureString("Enter Your Name") / 2;
                spriteBatch.DrawString(ArialTitle, "Enter Your Name", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.Draw(saveName.texture, saveName.position, Color.White);
                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);

                spriteBatch.Draw(saveButton.texture, saveButton.position, Color.White);

                FontOrigin = ArialTitle.MeasureString(strSaveName) / 2;
                spriteBatch.DrawString(ArialTitle, strSaveName, saveName.position + new Vector2(saveName.texture.Width / 2, saveName.texture.Height / 2), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                FontOrigin = Arial.MeasureString(statusMessage) / 2;
                spriteBatch.DrawString(Arial, statusMessage, new Vector2(GraphicsDevice.Viewport.Width / 2, saveName.position.Y + saveName.texture.Height + 25), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                spriteBatch.End();
            }
            else if (gameState == GameState.ViewScores)
            {
                spriteBatch.Begin();
                Vector2 FontOrigin;
                string[] data;

                GraphicsDevice.Clear(Color.LightGreen);
                FontOrigin = ArialTitle.MeasureString("Highscores") / 2;
                spriteBatch.DrawString(ArialTitle, "Highscores", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);

                FontOrigin = Arial.MeasureString("Name : String") / 2;
                spriteBatch.DrawString(Arial, "Name : Score", new Vector2(GraphicsDevice.Viewport.Width / 2, 200 + (0 * 25)), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                for (int x = 0; x < 10 && x < highScores.Length; x++)
                {
                    try
                    {
                        data = highScores[x].Split();
                        FontOrigin = Arial.MeasureString(data[0] + " : " + data[1]) / 2;
                        spriteBatch.DrawString(Arial, data[0] + " : " + data[1], new Vector2(GraphicsDevice.Viewport.Width / 2, 225 + (x * 25)), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    }
                    catch (IndexOutOfRangeException exc) { }
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                spriteBatch.End();
            }
            else if (gameState == GameState.Level1 || gameState == GameState.Level2 || gameState == GameState.Level3 || gameState == GameState.Level4 || gameState == GameState.Level5 || gameState == GameState.Level6 || gameState == GameState.Level7 || gameState == GameState.Level8 || gameState == GameState.ArcadeMode || gameState == GameState.WaveDelay)
            {
                Rectangle tileRect = new Rectangle(1, 1, 1, 1);

                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraOne.transform);

                spriteBatch.Draw(background, Vector2.Zero, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (!mapObject.Collision)
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);

                foreach (Bullet bullet in bulletList)
                    spriteBatch.Draw(bullet.texture, bullet.position, null, Color.White, bullet.rotation, bullet.origin, 1f, SpriteEffects.None, 0);


                foreach (Drop drop in itemDropList)
                    if (drop.flash)
                        spriteBatch.Draw(drop.texture, drop.position, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (mapObject.GetRectangle.Intersects(cameraOne.Rectangle()))
                    {
                        mapObject.seen = true;
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);
                    }

                if (!(gameState == GameState.ArcadeMode) && !(gameState == GameState.WaveDelay))
                    spriteBatch.Draw(endAreaTexture, endArea, Color.White);

                foreach (Enemy enemy in enemyList)
                {
                    spriteBatch.Draw(enemy.textureBottom, enemy.position, null, Color.White, enemy.rotationBottom, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.weapon1.currentWeaponTexture, enemy.position, null, Color.White, enemy.rotationMid, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.texture, enemy.position, null, Color.White, enemy.rotationTop, enemy.origin, 1f, SpriteEffects.None, 0);                  
                }

                foreach (Player player in playerList)
                {
                    spriteBatch.Draw(playerOne.textureBottom, playerOne.position, null, Color.White, playerOne.rotationBottom, playerOne.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(playerOne.currentWeapon().currentWeaponTexture, playerOne.positionMid, null, Color.White, playerOne.rotationMid, playerOne.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(playerOne.texture, playerOne.position, null, Color.White, playerOne.rotationTop, playerOne.origin, 1f, SpriteEffects.None, 0);
                }

                Vector2 FontOrigin = Arial.MeasureString(playerOne.stamina.ToString()) / 2;

                if (playerOne.staminaDepleted)
                    spriteBatch.DrawString(Arial, "Stamina: " + playerOne.stamina.ToString(), new Vector2(825 - cameraOne.inverse.X, 50 - cameraOne.inverse.Y), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                else
                    spriteBatch.DrawString(Arial, "Stamina: " + playerOne.stamina.ToString(), new Vector2(825 - cameraOne.inverse.X, 50 - cameraOne.inverse.Y), Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                if (playerOne.health < 10)
                    spriteBatch.DrawString(Arial, "Health: " + playerOne.health.ToString(), new Vector2(825 - cameraOne.inverse.X, 25 - cameraOne.inverse.Y), Color.DarkRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                else if (playerOne.health < 25)
                    spriteBatch.DrawString(Arial, "Health: " + playerOne.health.ToString(), new Vector2(825 - cameraOne.inverse.X, 25 - cameraOne.inverse.Y), Color.OrangeRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                else if (playerOne.health < 50)
                    spriteBatch.DrawString(Arial, "Health: " + playerOne.health.ToString(), new Vector2(825 - cameraOne.inverse.X, 25 - cameraOne.inverse.Y), Color.Orange, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                else if (playerOne.health < 75)
                    spriteBatch.DrawString(Arial, "Health: " + playerOne.health.ToString(), new Vector2(825 - cameraOne.inverse.X, 25 - cameraOne.inverse.Y), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                else
                    spriteBatch.DrawString(Arial, "Health: " + playerOne.health.ToString(), new Vector2(825 - cameraOne.inverse.X, 25 - cameraOne.inverse.Y), Color.Green, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                if (playerOne.CurrentAmmo() > 0)
                    spriteBatch.DrawString(Arial, "Ammo: " + playerOne.CurrentAmmo(), new Vector2(825 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Blue, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                else
                    spriteBatch.DrawString(Arial, "Ammo: " + playerOne.CurrentAmmo(), new Vector2(825 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                if (gameState == GameState.ArcadeMode || gameState == GameState.WaveDelay)
                {
                    spriteBatch.DrawString(Arial, "Score: " + score, new Vector2(825 - cameraOne.inverse.X, 125 - cameraOne.inverse.Y), Color.Magenta, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(Arial, "Wave: " + wave, new Vector2(825 - cameraOne.inverse.X, 150 - cameraOne.inverse.Y), Color.Brown, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                }
                if (playerOne.currentWeapon().clipCount != 0)
                    spriteBatch.DrawString(Arial, "Clip: " + playerOne.currentWeapon().clipCount, new Vector2(825 - cameraOne.inverse.X, 100 - cameraOne.inverse.Y), Color.DarkCyan, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                else
                {
                    spriteBatch.DrawString(Arial, "Clip: " + playerOne.currentWeapon().clipCount, new Vector2(825 - cameraOne.inverse.X, 100 - cameraOne.inverse.Y), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    if (!playerOne.currentWeapon().reloading && playerOne.CurrentAmmo() > 0)
                    {
                        FontOrigin = Arial.MeasureString("RELOAD") / 2;
                        spriteBatch.DrawString(Arial, "RELOAD", playerOne.position + new Vector2(0, 50), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    }
                    else if (playerOne.CurrentAmmo() == 0)
                    {
                        FontOrigin = Arial.MeasureString("NO AMMO") / 2;
                        spriteBatch.DrawString(Arial, "NO AMMO", playerOne.position + new Vector2(0, 50), Color.DarkRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    }
                }

                if (gameState == GameState.WaveDelay)
                {
                    FontOrigin = Arial.MeasureString("Next wave starting in " + (waveCounter / 60)) / 2;
                    if (waveCounter < 180)
                        spriteBatch.DrawString(Arial, "Next wave starting in " + (waveCounter / 60), playerOne.position + new Vector2(0, 75), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    else if (waveCounter < 360)
                        spriteBatch.DrawString(Arial, "Next wave starting in " + (waveCounter / 60), playerOne.position + new Vector2(0, 75), Color.Orange, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    else
                        spriteBatch.DrawString(Arial, "Next wave starting in " + (waveCounter / 60), playerOne.position + new Vector2(0, 75), Color.LimeGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                }

                if (playerOne.currentWeapon().reloading)
                {
                    FontOrigin = Arial.MeasureString("RELOADING") / 2;
                    spriteBatch.DrawString(Arial, "RELOADING", playerOne.position + new Vector2(0, 50), Color.Green, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2) - cameraOne.inverse.X, Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2) - cameraOne.inverse.Y), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2) - cameraOne.inverse.X, Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2) - cameraOne.inverse.Y), Color.White);

                spriteBatch.End();

                spriteBatch.Begin();
                tileRect.Height = 2;
                tileRect.Width = 2;

                spriteBatch.Draw(MinimapBackground, new Vector2(25, 25), Color.White);

                foreach (MapObject mapObj in mapObjectList)
                {
                    if (mapObj.seen)
                    {
                        tileRect.X = (int)(mapObj.Position.X / 20) + 25;
                        tileRect.Y = (int)(mapObj.Position.Y / 20) + 25;
                        spriteBatch.Draw(MinimapTile, tileRect, Color.White);
                    }
                }

                FontOrigin = Arial.MeasureString(statusMessage) / 2;
                spriteBatch.DrawString(Arial, statusMessage, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 100), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                tileRect.Width = 4;
                tileRect.Height = 4;

                foreach (Enemy enemy in enemyList)
                {
                    if (enemy.aggro)
                    {
                        tileRect.X = (int)(enemy.position.X / 20 - 2) + 25;
                        tileRect.Y = (int)(enemy.position.Y / 20 - 2) + 25;
                        spriteBatch.Draw(RedTile, tileRect, Color.White);
                    }
                }

                tileRect.X = (int)(playerOne.position.X / 20 - 2) + 25;
                tileRect.Y = (int)(playerOne.position.Y / 20 - 2) + 25;
                spriteBatch.Draw(GreenTile, tileRect, Color.White);

                spriteBatch.End();
            }
            else if (gameState == GameState.GameOver)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraOne.transform);
                spriteBatch.Draw(background, Vector2.Zero, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (!mapObject.Collision)
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);

                foreach (Bullet bullet in bulletList)
                    spriteBatch.Draw(bullet.texture, bullet.position, null, Color.White, bullet.rotation, bullet.origin, 1f, SpriteEffects.None, 0);

                foreach (Drop drop in itemDropList)
                    if (drop.flash)
                        spriteBatch.Draw(drop.texture, drop.position, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (mapObject.GetRectangle.Intersects(cameraOne.Rectangle()))
                    {
                        mapObject.seen = true;
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);
                    }

                foreach (Enemy enemy in enemyList)
                {
                    spriteBatch.Draw(enemy.textureBottom, enemy.position, null, Color.White, enemy.rotationBottom, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.weapon1.currentWeaponTexture, enemy.position, null, Color.White, enemy.rotationMid, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.texture, enemy.position, null, Color.White, enemy.rotationTop, enemy.origin, 1f, SpriteEffects.None, 0);
                }

                foreach (Player player in playerList)
                {
                    spriteBatch.Draw(playerOne.textureBottom, playerOne.position, null, Color.White, playerOne.rotationBottom, playerOne.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(playerOne.currentWeapon().currentWeaponTexture, playerOne.positionMid, null, Color.White, playerOne.rotationMid, playerOne.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(playerOne.texture, playerOne.position, null, Color.White, playerOne.rotationTop, playerOne.origin, 1f, SpriteEffects.None, 0);
                }

                spriteBatch.End();
                spriteBatch.Begin();
                spriteBatch.Draw(retryButton.texture, retryButton.position, Color.White);
                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);

                if (prevGameState == GameState.ArcadeMode)
                    spriteBatch.Draw(saveScoreButton.texture, saveScoreButton.position, Color.White);

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);

                spriteBatch.End();
            }
            else if (gameState == GameState.PauseMenu)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraOne.transform);
                spriteBatch.Draw(background, Vector2.Zero, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (!mapObject.Collision)
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);

                foreach (Bullet bullet in bulletList)
                    spriteBatch.Draw(bullet.texture, bullet.position, null, Color.White, bullet.rotation, bullet.origin, 1f, SpriteEffects.None, 0);


                foreach (Drop drop in itemDropList)
                    if (drop.flash)
                        spriteBatch.Draw(drop.texture, drop.position, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (mapObject.GetRectangle.Intersects(cameraOne.Rectangle()))
                    {
                        mapObject.seen = true;
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);
                    }

                foreach (Enemy enemy in enemyList)
                {
                    spriteBatch.Draw(enemy.textureBottom, enemy.position, null, Color.White, enemy.rotationBottom, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.weapon1.currentWeaponTexture, enemy.position, null, Color.White, enemy.rotationMid, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.texture, enemy.position, null, Color.White, enemy.rotationTop, enemy.origin, 1f, SpriteEffects.None, 0);
                }

                foreach (Player player in playerList)
                {
                    spriteBatch.Draw(playerOne.textureBottom, playerOne.position, null, Color.White, playerOne.rotationBottom, playerOne.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(playerOne.currentWeapon().currentWeaponTexture, playerOne.positionMid, null, Color.White, playerOne.rotationMid, playerOne.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(playerOne.texture, playerOne.position, null, Color.White, playerOne.rotationTop, playerOne.origin, 1f, SpriteEffects.None, 0);
                }

                spriteBatch.End();
                spriteBatch.Begin();

                Vector2 fontOrigin = ArialTitle.MeasureString("Game Paused") / 2;
                spriteBatch.DrawString(ArialTitle, "Game Paused", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Green, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.Draw(resumeButton.texture, resumeButton.position, Color.White);

                if (prevGameState != GameState.WaveDelay && prevGameState != GameState.ArcadeMode)
                    spriteBatch.Draw(saveGameButton.texture, saveGameButton.position, Color.White);
                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);

                spriteBatch.End();
            }
            else if (gameState == GameState.Cutscene1 || gameState == GameState.Cutscene2 || gameState == GameState.Cutscene3 || gameState == GameState.Cutscene4 || gameState == GameState.Cutscene5 || gameState == GameState.Cutscene6 || gameState == GameState.Cutscene7 || gameState == GameState.Cutscene8 || gameState == GameState.Cutscene9)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(cutsceneBackground, Vector2.Zero, Color.White);

                for (int x = 0; x < displayText.Length; x++)
                {
                    Vector2 fontOrigin = Arial.MeasureString(displayText[x]) / 2;
                    spriteBatch.DrawString(Arial, displayText[x], new Vector2(GraphicsDevice.Viewport.Width / 2, 100 + (25 * x)), Color.White, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                }
                spriteBatch.End();
            }
            else if (gameState == GameState.End)
            {
                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.Yellow);
                Vector2 fontOrigin = ArialTitle.MeasureString("Game Over") / 2;
                spriteBatch.DrawString(ArialTitle, "Game Over", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.Green, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.Draw(exitButton.texture, exitButton.position, Color.White);

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOn"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);
                else
                    spriteBatch.Draw(Content.Load<Texture2D>("CursorOff"), new Vector2(Mouse.GetState().X - (Content.Load<Texture2D>("CursorOn").Width / 2), Mouse.GetState().Y - (Content.Load<Texture2D>("CursorOn").Height / 2)), Color.White);


                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.Draw(loading, Vector2.Zero, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
