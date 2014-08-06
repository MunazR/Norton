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
using System.Threading;

namespace NortonXBOX360
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
        Button title;

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
        bool startRelease;

        //Cutscene Text
        string[] displayText;
        string[] cutsceneText;
        char[] textArray;
        int stage;
        int stage2;
        bool done;
        int textDelay;
        int AIdelay;
        int updateDelay;
        Texture2D cutsceneBackground;


        string[] highScores;

        GamePadState prevGamePadState;

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
            LoadPauseMenu,
            PauseMenu,
            LoadHelpMenu,
            HelpMenu,
            LoadEnd,
            End,
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Window.Title = "Norton";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            cameraOne = new Camera(GraphicsDevice.Viewport);
            gameState = GameState.LoadStartMenu;
            base.Initialize();
            //IsFixedTimeStep = true;
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1 / 30);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //playerOne.Texture = Content.Load<Texture2D>("Norton");

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

            buttonPress = Content.Load<SoundEffect>("Button-1");

            Player playerOne  = new Player(Content);
            playerOne.Texture = Content.Load<Texture2D>("Norton");
            playerOne.Index = 1;
            Player playerTwo = new Player(Content);
            playerTwo.Index = 2;
            playerTwo.Texture = Content.Load<Texture2D>("Norton");
            playerList.Add(playerOne);
            playerList.Add(playerTwo);
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
            updateDelay++;
            updateDelay = updateDelay % 2;
            //updateDelay = 0;
            if (updateDelay == 0)
            {
                if (!this.IsActive)
                    return;

                GamePadState gamePadStateOne = GamePad.GetState(PlayerIndex.One);
                GamePadState gamePadStateTwo = GamePad.GetState(PlayerIndex.Two);

                matrixOffset = cameraOne.inverse; ;

                if (gameState == GameState.LoadStartMenu)
                {
                    LoadStartMenu();
                }
                else if (gameState == GameState.StartMenu)
                {
                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadCutscene1;
                    }
                    else if (gamePadStateOne.Buttons.Back == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        this.Exit();
                    }
                    else if (gamePadStateOne.Buttons.X == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadArcadeChooseMap;
                    }
                    else if ((gamePadStateOne.Buttons.Y == ButtonState.Pressed))
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadHelpMenu;
                    }
                }
                else if (gameState == GameState.LoadEnd)
                {
                    LoadEnd();
                    gameState = GameState.End;
                }
                else if (gameState == GameState.End)
                {

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
                    {
                        buttonPress.Play();
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
                    if (true)
                    {
                        Rectangle mouseRect = new Rectangle(0, 0, CursorOn.Width, CursorOn.Height);

                        if (mouseRect.Intersects(exitButton.GetRectangle()))
                        {
                            buttonPress.Play();
                            gameState = GameState.LoadStartMenu;
                        }
                    };
                }
                else if (gameState == GameState.LoadCutscene1)
                {
                    LoadCutscene(new StreamReader("Content/1.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene1;
                }
                else if (gameState == GameState.LoadCutscene2)
                {
                    LoadCutscene(new StreamReader("Content/2.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene2;
                }
                else if (gameState == GameState.LoadCutscene3)
                {
                    LoadCutscene(new StreamReader("Content/3.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene3;
                }
                else if (gameState == GameState.LoadCutscene4)
                {
                    LoadCutscene(new StreamReader("Content/4.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene4;
                }
                else if (gameState == GameState.LoadCutscene5)
                {
                    LoadCutscene(new StreamReader("Content/5.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene5;
                }
                else if (gameState == GameState.LoadCutscene6)
                {
                    LoadCutscene(new StreamReader("Content/6.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene6;
                }
                else if (gameState == GameState.LoadCutscene7)
                {
                    LoadCutscene(new StreamReader("Content/7.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene7;
                }
                else if (gameState == GameState.LoadCutscene8)
                {
                    LoadCutscene(new StreamReader("Content/8.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene8;
                }
                else if (gameState == GameState.LoadCutscene9)
                {
                    LoadCutscene(new StreamReader("Content/9.txt"));
                    cutsceneBackground = Content.Load<Texture2D>("Cutscene1");
                    gameState = GameState.Cutscene9;
                }
                else if (gameState == GameState.LoadLevel1)
                {
                    StreamReader map = new StreamReader("Content/level1.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldCPU");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    map.Close();

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

                    music = Content.Load<Song>("Music 1");

                    background = Content.Load<Texture2D>("WhiteTileBackground");
                    MediaPlayer.Volume = 2;
                    MediaPlayer.Play(music);
                    gameState = GameState.Level1;
                }
                else if (gameState == GameState.LoadLevel2)
                {
                    StreamReader map = new StreamReader("Content/level2.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldRAM");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

                    music = Content.Load<Song>("Music 3");

                    background = Content.Load<Texture2D>("WhiteTileBackground");
                    MediaPlayer.Volume = 2;
                    MediaPlayer.Play(music);
                    gameState = GameState.Level2;
                }
                else if (gameState == GameState.LoadLevel3)
                {
                    StreamReader map = new StreamReader("Content/level3.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldGPU");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

                    music = Content.Load<Song>("Music 2");

                    background = Content.Load<Texture2D>("WhiteTileBackground");
                    MediaPlayer.Volume = 2;
                    MediaPlayer.Play(music);
                    gameState = GameState.Level3;
                }
                else if (gameState == GameState.LoadLevel4)
                {
                    StreamReader map = new StreamReader("Content/level4.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldHDD");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

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
                    StreamReader map = new StreamReader("Content/level5.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldSSD");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

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
                    StreamReader map = new StreamReader("Content/level6.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldPSU");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

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
                    StreamReader map = new StreamReader("Content/level7.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldMOBO");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

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
                    StreamReader map = new StreamReader("Content/level8.txt");

                    endAreaTexture = Content.Load<Texture2D>("GoldCASE");

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    LoadLevel(map);

                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                    }

                    enemyTotalCount = 50;
                    enemyAliveAtOneTime = 3;

                    music = Content.Load<Song>("Music 2");

                    background = Content.Load<Texture2D>("WhiteTileBackground");
                    MediaPlayer.Volume = 2;
                    MediaPlayer.Play(music);
                    gameState = GameState.Level8;
                }
                else if (gameState == GameState.HighScore)
                {

                }
                else if (gameState == GameState.LoadArcadeChooseMap)
                {
                    LoadArcadeChooseMap();
                }
                else if (gameState == GameState.ArcadeChooseMap)
                {
                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        mapChosen = 1;
                        gameState = GameState.LoadArcade;
                    }
                    else if (gamePadStateOne.Buttons.X == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        mapChosen = 2;
                        gameState = GameState.LoadArcade;
                    }
                    else if (gamePadStateOne.Buttons.Y == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        mapChosen = 3;
                        gameState = GameState.LoadArcade;
                    }
                    else if (gamePadStateOne.Buttons.B == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                    else if (gamePadStateOne.Buttons.RightShoulder == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadViewScores;
                    }
                }
                else if (gameState == GameState.LoadArcade)
                {
                    StreamReader map;

                    switch (mapChosen)
                    {
                        case 1:
                            map = new StreamReader("Content/arcade1.txt");
                            break;
                        case 2:
                            map = new StreamReader("Content/arcade2.txt");
                            break;
                        case 3:
                            map = new StreamReader("Content/arcade3.txt");
                            break;
                        default:
                            map = new StreamReader("Content/arcade1.txt");
                            break;
                    }

                    enemyList.Clear();
                    mapObjectList.Clear();
                    bulletList.Clear();
                    itemDropList.Clear();

                    spawnLocations = new List<Vector2>();
                    LoadLevel(map);

                    background = Content.Load<Texture2D>("WhiteTileBackground");
                    foreach (Player player in playerList)
                    {
                        player.Health = player.MaxHealth;
                        player.StaminaDepleted = false;
                        player.Stamina = player.MaxStamina;
                        player.ResetAmmoCount();
                    }
                    
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
                    {
                        if(player.Index == 1)
                            player.Update(gameTime, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList, gamePadStateOne);
                        else if(player.Index == 2)
                            player.Update(gameTime, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList, gamePadStateTwo);
                    }

                    //playerOne.Update(gameTime, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList, gamePadStateOne);
                    //playerTwo.Update(gameTime, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList, gamePadStateTwo);

                    for (int x = 0; x < itemDropList.Count; x++)
                    {
                        itemDropList[x].Update(playerList);
                    }

                    for (int x = 0; x < itemDropList.Count; x++)
                        if (itemDropList[x].TimeAlive <= 0)
                            itemDropList.Remove(itemDropList[x]);

                    cameraOne.Update(gameTime, playerList[0], room);
                    
                    //AIdelay++;
                    //if (AIdelay % 10 == 0)
                    //{
                    //    AIdelay = 0;

                    for (int x = 0; x < enemyList.Count; x++)
                    {
                        enemyList[x].Update(playerList, mapObjectList, Content, enemyList, bulletList);
                    }
                    
                    for (int x = 0; x < enemyList.Count; x++)
                    {
                        if (enemyList[x].remove)
                            enemyList.Remove(enemyList[x--]);
                    }

                    for (int x = 0; x < bulletList.Count; x++)
                        bulletList[x].Update(mapObjectList, enemyList, playerList, itemDropList, Content);


                    for (int i = 0; i < bulletList.Count; i++)
                        if (bulletList[i].Remove)
                            bulletList.Remove(bulletList[i]);

                    bool alive = false;
                    foreach(Player player in playerList){
                        if (player.Health > 0)
                        {
                            alive = true;
                        }
                    }

                    if (!alive)
                    {
                        prevGameState = gameState;
                        gameState = GameState.LoadGameOver;
                        GamePad.SetVibration(PlayerIndex.One, 0, 0);
                        GamePad.SetVibration(PlayerIndex.Two, 0, 0);
                    }

                    if (gamePadStateOne.Buttons.Start == ButtonState.Pressed && startRelease)
                    {
                        prevGameState = gameState;
                        gameState = GameState.LoadPauseMenu;
                        GamePad.SetVibration(PlayerIndex.One, 0, 0);
                        GamePad.SetVibration(PlayerIndex.Two, 0, 0);
                        startRelease = false;
                    }
                    else if (gamePadStateOne.Buttons.Start == ButtonState.Released)
                    {
                        startRelease = true;
                    }

                    if (playerList[0].GetRectangle().Intersects(endArea))
                    {
                        if (enemyList.Count != 0)
                        {
                            statusMessage = "Clear all enemies first!";
                        }
                        else
                        {
                            GamePad.SetVibration(PlayerIndex.One, 0, 0);
                            GamePad.SetVibration(PlayerIndex.Two, 0, 0);

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
                    else
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
                    {
                        if(player.Index == 1)
                            player.Update(gameTime, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList, gamePadStateOne);
                        else if(player.Index == 2)
                            player.Update(gameTime, mapObjectList, Content, bulletList, matrixOffset, enemyList, itemDropList, gamePadStateTwo);
                    }
                    foreach (Drop drop in itemDropList)
                        drop.Update(playerList);

                    for (int x = 0; x < itemDropList.Count; x++)
                        if (itemDropList[x].TimeAlive <= 0)
                            itemDropList.Remove(itemDropList[x]);
               
                    cameraOne.Update(gameTime, playerList[0], room);

                    foreach (Enemy enemy in enemyList)
                        enemy.Update(playerList, mapObjectList, Content, enemyList, bulletList);

                    for (int x = 0; x < enemyList.Count; x++)
                        if (enemyList[x].remove)
                            enemyList.Remove(enemyList[x--]);

                    foreach (Bullet bullet in bulletList)
                        bullet.Update(mapObjectList, enemyList, playerList, itemDropList, Content);

                    for (int i = 0; i < bulletList.Count; i++)
                    {
                        if (bulletList[i].Remove)
                            bulletList.Remove(bulletList[i]);
                    }

                    bool alive = false;
                    foreach (Player player in playerList)
                    {
                        if (player.Health > 0)
                            alive = true;
                    }

                    if (!alive)
                    {
                        prevGameState = gameState;
                        gameState = GameState.LoadGameOver;
                        GamePad.SetVibration(PlayerIndex.One, 0, 0);
                        GamePad.SetVibration(PlayerIndex.Two, 0, 0);
                    }

                    if (gamePadStateOne.Buttons.Start == ButtonState.Pressed && startRelease)
                    {
                        prevGameState = gameState;
                        gameState = GameState.LoadPauseMenu;
                        startRelease = false;
                    }
                    else if (gamePadStateOne.Buttons.Start == ButtonState.Released)
                        startRelease = true;

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
                    if (gamePadStateOne.Buttons.Start == ButtonState.Pressed && startRelease)
                    {
                        MediaPlayer.Resume();
                        gameState = prevGameState;
                        startRelease = false;
                    }
                    else if (gamePadStateOne.Buttons.Start == ButtonState.Released)
                        startRelease = true;

                    if (gamePadStateOne.Buttons.B == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = prevGameState;
                        MediaPlayer.Resume();
                    }
                    else if (gamePadStateOne.Buttons.Back == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
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

                    if (gamePadStateOne.Buttons.A == ButtonState.Pressed)
                        gameState = GameState.LoadEnd;

                }
                else if (gameState == GameState.LoadHelpMenu)
                {
                    LoadHelpMenu();
                    gameState = GameState.HelpMenu;
                }
                else if (gameState == GameState.HelpMenu)
                {
                    if (gamePadStateOne.Buttons.B == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                }
                else if (gameState == GameState.LoadGameOver)
                {
                    MediaPlayer.Stop();
                    LoadGameOver();
                }
                else if (gameState == GameState.GameOver)
                {
                    if (gamePadStateOne.Buttons.X == ButtonState.Pressed)
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
                    else if (gamePadStateOne.Buttons.Back == ButtonState.Pressed)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadStartMenu;
                    }
                    else if ((gamePadStateOne.Buttons.Y == ButtonState.Pressed) && prevGameState == GameState.ArcadeMode)
                    {
                        buttonPress.Play();
                        gameState = GameState.LoadHighScoreMenu;
                    }
                }
                prevGamePadState = GamePad.GetState(PlayerIndex.One);

                base.Update(gameTime);
            }
        }

        private void LoadHelpMenu()
        {
            exitButton = new Button();
            exitButton.Texture = Content.Load<Texture2D>("BackButton");
            exitButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), GraphicsDevice.Viewport.Height - 100);
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

            enemy.Position = spawns[rand.Next(0, spawns.Count)]+enemy.Origin;
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
                enemy.Position = position;
            } while ((enemy.Collision(enemy.GetRectangle(), Vector2.Zero, mapObjectList, playerList, enemyList)) && !(Vector2.Distance(enemy.Position, playerList[0].Position) > (960))/*&& !(Vector2.Distance(enemy.Position, playerOne.Position) > 50) && !(Vector2.Distance(enemy.Position, playerOne.Position) < 100)*/);

            enemy.Position = position+enemy.Origin;
            enemy.aggro = true;
            enemyList.Add(enemy);
        }

        private void SpawnEnemy(Vector2 position, string newEnemyType)
        {
            Random rand = new Random();

            Enemy enemy = new Enemy(Content, newEnemyType);

            enemy.Position = position;

            enemy.Position = position;
            enemyList.Add(enemy);
        }

        private void LoadHighScoreMenu()
        {
            saveName = new Button();
            saveName.Texture = Content.Load<Texture2D>("SaveName");
            saveName.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (saveName.Texture.Width / 2), 200);

            exitButton = new Button();
            exitButton.Texture = Content.Load<Texture2D>("ExitButton");
            exitButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), 500);

            saveButton = new Button();
            saveButton.Texture = Content.Load<Texture2D>("SaveButton");
            saveButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), 400);
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
            exitButton.Texture = Content.Load<Texture2D>("ExitButton");
            exitButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), 500);
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
            exitButton.Texture = Content.Load<Texture2D>("exitButton");
            exitButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), 400);
        }

        private void LoadPauseMenu()
        {
            resumeButton = new Button();
            resumeButton.Texture = Content.Load<Texture2D>("BackButton");
            resumeButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Texture.Width / 2), 300);

            title = new Button();
            title.Texture = Content.Load<Texture2D>("Paused");
            title.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (title.Texture.Width / 2), 100);

            exitButton = new Button();
            exitButton.Texture = Content.Load<Texture2D>("ExitButton");
            exitButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), 400);
        }


        private void LoadStartMenu()
        {
            startStoryButton = new Button();
            startArcadeButton = new Button();
            exitButton = new Button();
            howToButton = new Button();
            title = new Button();

            title.Texture = Content.Load<Texture2D>("Title");
            startStoryButton.Texture = Content.Load<Texture2D>("StartStoryButton");
            startArcadeButton.Texture = Content.Load<Texture2D>("StartArcadeButton");
            exitButton.Texture = Content.Load<Texture2D>("ExitButton");
            howToButton.Texture = Content.Load<Texture2D>("HowToButton");

            title.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (title.Texture.Width / 2), 100);
            startStoryButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startStoryButton.Texture.Width / 2), 250);
            startArcadeButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startArcadeButton.Texture.Width / 2), 350);
            howToButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startArcadeButton.Texture.Width / 2), 450);
            exitButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), 550);

            gameState = GameState.StartMenu;
        }

        private void LoadArcadeChooseMap()
        {
            map1 = new Button();
            map2 = new Button();
            map3 = new Button();
            highScoreButton = new Button();
            exitButton = new Button();
            title = new Button();

            map1.Texture = Content.Load<Texture2D>("Map1");
            map2.Texture = Content.Load<Texture2D>("Map2");
            map3.Texture = Content.Load<Texture2D>("Map3");
            title.Texture = Content.Load<Texture2D>("ChooseMap");
            highScoreButton.Texture = Content.Load<Texture2D>("HighScoreButton");
            exitButton.Texture = Content.Load<Texture2D>("ExitButton");

            map1.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (map1.Texture.Width / 2) - 200, 200);
            map2.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (map2.Texture.Width / 2), 200);
            map3.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (map3.Texture.Width / 2) + 200, 200);
            title.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (title.Texture.Width / 2), 100);
            highScoreButton.Position = new Vector2(50, 500);
            exitButton.Position = new Vector2(910 - exitButton.Texture.Width, 500);

            gameState = GameState.ArcadeChooseMap;
        }

        private void LoadGameOver()
        {
            retryButton = new Button();
            exitButton = new Button();
            saveScoreButton = new Button();
            title = new Button();

            retryButton.Texture = Content.Load<Texture2D>("RetryButton");
            exitButton.Texture = Content.Load<Texture2D>("ExitButton");
            title.Texture = Content.Load<Texture2D>("GameOver");

            title.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (title.Texture.Width / 2), 100);
            retryButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startStoryButton.Texture.Width / 2), 300);
            exitButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Texture.Width / 2), 400);


            saveScoreButton.Texture = Content.Load<Texture2D>("SaveScoreButton");
            saveScoreButton.Position = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startStoryButton.Texture.Width / 2), 300);

            MediaPlayer.Stop();

            foreach (Player player in playerList)
            {
                player.Health = player.MaxHealth;
            }

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
                    switch (tile)
                    {
                        case '1':
                            mapObj = new MapObject(BlackWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '2':
                            mapObj = new MapObject(BrownWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '3':
                            mapObj = new MapObject(BlueWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '4':
                            mapObj = new MapObject(CyanWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '5':
                            mapObj = new MapObject(GrayWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '6':
                            mapObj = new MapObject(GreenWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '7':
                            mapObj = new MapObject(LightBlackWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '8':
                            mapObj = new MapObject( LightBlueWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '9':
                            mapObj = new MapObject( LightGreenWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case '0':
                            mapObj = new MapObject( LightPurpleWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'a':
                            mapObj = new MapObject(OrangeWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'b':
                            mapObj = new MapObject(PinkWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'c':
                            mapObj = new MapObject( PurpleWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'C':
                            SpawnEnemy(new Vector2(count * 40, row * 40), "Chaser");
                            break;
                        case 'd':
                           mapObj = new MapObject( RedWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'e':
                           mapObj = new MapObject(Sand);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'f':
                            mapObj = new MapObject( Water);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'g':
                            mapObj = new MapObject( Wood);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'h':
                            mapObj = new MapObject(YellowWall);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'H':
                            SpawnEnemy(new Vector2(count * 40, row * 40), "Heavy");
                            break;
                        case 'i':
                           mapObj = new MapObject( Circle);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'j':
                           mapObj = new MapObject( Bookshelf);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'k':
                            mapObj = new MapObject(Chest);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'l':
                            mapObj = new MapObject( CursorOn);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'm':
                            mapObj = new MapObject( CursorOff);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'n':
                            mapObj = new MapObject( Lava);

                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'o':
                           mapObj = new MapObject( Lever);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'p':
                            mapObj = new MapObject( Light);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'q':
                           mapObj = new MapObject( BrickWall);

                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'r':
                           mapObj = new MapObject(BlueLaser);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 's':
                           mapObj = new MapObject( Crate);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 't':
                            mapObj = new MapObject(Pumpkin);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'u':
                            mapObj = new MapObject( DeadTree);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'v':
                            mapObj = new MapObject( Tree);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'w':
                            mapObj = new MapObject( Bush);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'x':
                           mapObj = new MapObject( Cake);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'z':
                           mapObj = new MapObject( Server);
                            mapObj.Position = new Vector2(count * mapObj.Texture.Width, row * mapObj.Texture.Height);
                            mapObjectList.Add(mapObj);
                            break;
                        case 'I':
                            spawnLocations.Add(new Vector2(count * 40, row * 40));
                            break;
                        case 'P':
                            playerList[0].Position = new Vector2(count * playerList[0].Texture.Width, row * playerList[0].Texture.Height);
                            break;
                        case 'Q':
                            playerList[1].Position = new Vector2(count * playerList[1].Texture.Width, row * playerList[1].Texture.Height);
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


            room.Height = row;

            room.Height *= 40;
            room.Width *= 40;
        }

        private void playerBounds(Player player, int minX, int minY, int maxX, int maxY)
        {
            if (player.Position.X > maxX - 20)
                player.PositionX = maxX - 20;
            else if (player.Position.X < minX + 20)
                player.PositionX = minX + 20;

            //Y Value
            if (player.Position.Y > maxY - 20)
                player.PositionY = maxY - 20;
            else if (player.Position.Y < minY + 20)
                player.PositionY = minY + 20;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Begin();
                GraphicsDevice.Clear(Color.LightGreen);

                spriteBatch.Draw(title.Texture, title.Position, Color.White);
                spriteBatch.Draw(startStoryButton.Texture, startStoryButton.Position, Color.White);
                spriteBatch.Draw(startArcadeButton.Texture, startArcadeButton.Position, Color.White);
                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);
                spriteBatch.Draw(howToButton.Texture, howToButton.Position, Color.White);

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
                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);
                spriteBatch.End();
            }
            else if (gameState == GameState.ArcadeChooseMap)
            {
                spriteBatch.Begin();

                GraphicsDevice.Clear(Color.LightGreen);

                spriteBatch.Draw(title.Texture, title.Position, Color.White);
                spriteBatch.Draw(map1.Texture, map1.Position, Color.White);
                spriteBatch.Draw(map2.Texture, map2.Position, Color.White);
                spriteBatch.Draw(map3.Texture, map3.Position, Color.White);

                spriteBatch.Draw(highScoreButton.Texture, highScoreButton.Position, Color.White);
                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);

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

                spriteBatch.Draw(saveName.Texture, saveName.Position, Color.White);
                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);

                spriteBatch.Draw(saveButton.Texture, saveButton.Position, Color.White);

                FontOrigin = ArialTitle.MeasureString(strSaveName) / 2;
                spriteBatch.DrawString(ArialTitle, strSaveName, saveName.Position + new Vector2(saveName.Texture.Width / 2, saveName.Texture.Height / 2), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                FontOrigin = Arial.MeasureString(statusMessage) / 2;
                spriteBatch.DrawString(Arial, statusMessage, new Vector2(GraphicsDevice.Viewport.Width / 2, saveName.Position.Y + saveName.Texture.Height + 25), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

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

                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);

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

                for(int x = 0; x < mapObjectList.Count; x++)
                    spriteBatch.Draw(mapObjectList[x].Texture, mapObjectList[x].Position, Color.White);

                //for (int x = 0; x < bulletList.Count; x++)
                //{
                //    spriteBatch.Draw(bulletList[x].Texture, bulletList[x].Position, null, Color.White, bulletList[x].Rotation, bulletList[x].Origin, 1f, SpriteEffects.None, 0);
                //    spriteBatch.Draw(null, bulletList[x].targetObject.Position, Color.White);
                //}
                for(int x = 0; x < itemDropList.Count; x++)
                    if (itemDropList[x].Flash)
                        spriteBatch.Draw(itemDropList[x].Texture, itemDropList[x].Position, Color.White);

                for(int x = 0; x < mapObjectList.Count; x++)
                    if (mapObjectList[x].GetRectangle().Intersects(cameraOne.Rectangle()))
                    {
                        mapObjectList[x].Seen = true;
                        spriteBatch.Draw(mapObjectList[x].Texture, mapObjectList[x].Position, Color.White);
                    }
                //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                for (int x = 0; x < bulletList.Count; x++)
                {
                    spriteBatch.Draw(bulletList[x].Texture, bulletList[x].Position, null, Color.White, bulletList[x].Rotation, bulletList[x].Origin, 1f, SpriteEffects.None, 0);
                    if (bulletList[x].targetObject != null)
                    spriteBatch.Draw(Content.Load<Texture2D>("Sand"), bulletList[x].targetObject.Position, Color.White);
                }
                //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                if (!(gameState == GameState.ArcadeMode) && !(gameState == GameState.WaveDelay))
                    spriteBatch.Draw(endAreaTexture, endArea, Color.White);

                for (int x = 0; x < enemyList.Count; x++)
                {
                    spriteBatch.Draw(enemyList[x].TextureBottom, enemyList[x].Position, null, Color.White, enemyList[x].rotationBottom, enemyList[x].origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemyList[x].weapon1.currentWeaponTexture, enemyList[x].Position, null, Color.White, enemyList[x].rotationMid, enemyList[x].origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemyList[x].Texture, enemyList[x].Position, null, Color.White, enemyList[x].rotationTop, enemyList[x].origin, 1f, SpriteEffects.None, 0);
                }

                foreach (Player player in playerList)
                {
                    spriteBatch.Draw(player.TextureBottom, player.Position, null, Color.White, player.RotationBottom, player.Origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(player.currentWeapon().currentWeaponTexture, player.Position, null, Color.White, player.RotationMid, player.Origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(player.Texture, player.Position, null, Color.White, player.RotationTop, player.Origin, 1f, SpriteEffects.None, 0);
                               }


                Vector2 FontOrigin = Arial.MeasureString(playerList[0].Stamina.ToString()) / 2; ;
                
                foreach (Player player in playerList)
                {
                    FontOrigin = Arial.MeasureString(player.Stamina.ToString()) / 2;

                    if (player.Index == 1)
                    {
                        if (player.StaminaDepleted)
                            spriteBatch.DrawString(Arial, "Stamina: " + player.Stamina.ToString(), new Vector2(200 - cameraOne.inverse.X, 100 - cameraOne.inverse.Y), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else
                            spriteBatch.DrawString(Arial, "Stamina: " + player.Stamina.ToString(), new Vector2(200 - cameraOne.inverse.X, 100 - cameraOne.inverse.Y), Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                        if (player.Health < 10)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.DarkRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else if (player.Health < 25)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.OrangeRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else if (player.Health < 50)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Orange, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else if (player.Health < 75)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Green, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    }

                    if (player.Index == 2)
                    {
                        if (player.StaminaDepleted)
                            spriteBatch.DrawString(Arial, "Stamina: " + player.Stamina.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 100 - cameraOne.inverse.Y), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else
                            spriteBatch.DrawString(Arial, "Stamina: " + player.Stamina.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 100 - cameraOne.inverse.Y), Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                        if (player.Health < 10)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.DarkRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else if (player.Health < 25)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.OrangeRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else if (player.Health < 50)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Orange, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else if (player.Health < 75)
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else
                            spriteBatch.DrawString(Arial, "Health: " + player.Health.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 75 - cameraOne.inverse.Y), Color.Green, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    }
                }
                //8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
                //if (playerOne.CurrentAmmo() > 0)
                //    spriteBatch.DrawString(Arial, "Ammo: " + playerOne.currentWeapon().clipCount + "/" + playerOne.CurrentAmmo(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 125 - cameraOne.inverse.Y), Color.Blue, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                //else
                //    spriteBatch.DrawString(Arial, "Ammo: " + playerOne.currentWeapon().clipCount + "/" + playerOne.CurrentAmmo(), new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 125 - cameraOne.inverse.Y), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                if (gameState == GameState.ArcadeMode || gameState == GameState.WaveDelay)
                {
                    spriteBatch.DrawString(Arial, "Score: " + score, new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 200 - cameraOne.inverse.Y), Color.Magenta, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(Arial, "Wave: " + wave, new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 225 - cameraOne.inverse.Y), Color.Brown, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                }

                foreach (Player player in playerList)
                {
                    if (player.currentWeapon().clipCount != 0) { }
                    //spriteBatch.DrawString(Arial, "Clip: " + playerOne.currentWeapon().clipCount, new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 150 - cameraOne.inverse.Y), Color.DarkCyan, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    else
                    {
                        spriteBatch.DrawString(Arial, "Clip: " + player.currentWeapon().clipCount, new Vector2(GraphicsDevice.Viewport.Width - 200 - cameraOne.inverse.X, 150 - cameraOne.inverse.Y), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        if (!player.currentWeapon().reloading && player.CurrentAmmo() > 0)
                        {
                            FontOrigin = Arial.MeasureString("RELOAD") / 2;
                            spriteBatch.DrawString(Arial, "RELOAD", player.Position + new Vector2(0, 50), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        }
                        else if (player.CurrentAmmo() == 0)
                        {
                            FontOrigin = Arial.MeasureString("NO AMMO") / 2;
                            spriteBatch.DrawString(Arial, "NO AMMO", player.Position + new Vector2(0, 50), Color.DarkRed, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        }
                    }
                }
                if (gameState == GameState.WaveDelay)
                {
                    foreach (Player player in playerList)
                    {
                        FontOrigin = Arial.MeasureString("Next wave starting in " + (waveCounter / 60)) / 2;
                        if (waveCounter < 180)
                            spriteBatch.DrawString(Arial, "Next wave starting in " + (waveCounter / 60), player.Position + new Vector2(0, 75), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else if (waveCounter < 360)
                            spriteBatch.DrawString(Arial, "Next wave starting in " + (waveCounter / 60), player.Position + new Vector2(0, 75), Color.Orange, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                        else
                            spriteBatch.DrawString(Arial, "Next wave starting in " + (waveCounter / 60), player.Position + new Vector2(0, 75), Color.LimeGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    }
                 }

                foreach (Player player in playerList)
                {
                    if (player.currentWeapon().reloading)
                    {
                        FontOrigin = Arial.MeasureString("RELOADING") / 2;
                        spriteBatch.DrawString(Arial, "RELOADING", player.Position + new Vector2(0, 50), Color.Green, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    }
                }
                spriteBatch.End();

                spriteBatch.Begin();
                tileRect.Height = 2;
                tileRect.Width = 2;

                spriteBatch.Draw(MinimapBackground, new Vector2(25, 25), Color.White);

                for(int x = 0; x < mapObjectList.Count; x++)
                {
                    if (mapObjectList[x].Seen)
                    {
                        tileRect.X = (int)(mapObjectList[x].Position.X / 20) + 25;
                        tileRect.Y = (int)(mapObjectList[x].Position.Y / 20) + 25;
                        spriteBatch.Draw(MinimapTile, tileRect, Color.White);
                    }
                }

                FontOrigin = Arial.MeasureString(statusMessage) / 2;
                spriteBatch.DrawString(Arial, statusMessage, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 100), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

                tileRect.Width = 4;
                tileRect.Height = 4;

                for(int x = 0; x < enemyList.Count; x++)
                {
                    if (enemyList[x].aggro)
                    {
                        tileRect.X = (int)(enemyList[x].Position.X / 20 - 2) + 25;
                        tileRect.Y = (int)(enemyList[x].Position.Y / 20 - 2) + 25;
                        spriteBatch.Draw(RedTile, tileRect, Color.White);
                    }
                }

                tileRect.X = (int)(playerList[0].Position.X / 20 - 2) + 25;
                tileRect.Y = (int)(playerList[0].Position.Y / 20 - 2) + 25;
                spriteBatch.Draw(GreenTile, tileRect, Color.White);

                spriteBatch.End();
            }
            else if (gameState == GameState.GameOver)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraOne.transform);
                spriteBatch.Draw(background, Vector2.Zero, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);

                foreach (Bullet bullet in bulletList)
                    spriteBatch.Draw(bullet.Texture, bullet.Position, null, Color.White, bullet.Rotation, bullet.Origin, 1f, SpriteEffects.None, 0);

                foreach (Drop drop in itemDropList)
                    if (drop.Flash)
                        spriteBatch.Draw(drop.Texture, drop.Position, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (mapObject.GetRectangle().Intersects(cameraOne.Rectangle()))
                    {
                        mapObject.Seen = true;
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);
                    }

                foreach (Enemy enemy in enemyList)
                {
                    spriteBatch.Draw(enemy.TextureBottom, enemy.Position, null, Color.White, enemy.rotationBottom, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.weapon1.currentWeaponTexture, enemy.Position, null, Color.White, enemy.rotationMid, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.Texture, enemy.Position, null, Color.White, enemy.rotationTop, enemy.origin, 1f, SpriteEffects.None, 0);
                }

                foreach (Player player in playerList)
                {
                    spriteBatch.Draw(player.TextureBottom, player.Position, null, Color.White, player.RotationBottom, player.Origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(player.currentWeapon().currentWeaponTexture, player.Position, null, Color.White, player.RotationMid, player.Origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(player.Texture, player.Position, null, Color.White, player.RotationTop, player.Origin, 1f, SpriteEffects.None, 0);
                }

                spriteBatch.End();
                spriteBatch.Begin();

                spriteBatch.Draw(title.Texture, title.Position, Color.White);
                spriteBatch.Draw(retryButton.Texture, retryButton.Position, Color.White);
                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);

                spriteBatch.End();
            }
            else if (gameState == GameState.PauseMenu)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraOne.transform);
                spriteBatch.Draw(background, Vector2.Zero, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);

                foreach (Bullet bullet in bulletList)
                    spriteBatch.Draw(bullet.Texture, bullet.Position, null, Color.White, bullet.Rotation, bullet.Origin, 1f, SpriteEffects.None, 0);


                foreach (Drop drop in itemDropList)
                    if (drop.Flash)
                        spriteBatch.Draw(drop.Texture, drop.Position, Color.White);

                foreach (MapObject mapObject in mapObjectList)
                    if (mapObject.GetRectangle().Intersects(cameraOne.Rectangle()))
                    {
                        mapObject.Seen = true;
                        spriteBatch.Draw(mapObject.Texture, mapObject.Position, Color.White);
                    }

                foreach (Enemy enemy in enemyList)
                {
                    spriteBatch.Draw(enemy.TextureBottom, enemy.Position, null, Color.White, enemy.rotationBottom, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.weapon1.currentWeaponTexture, enemy.Position, null, Color.White, enemy.rotationMid, enemy.origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(enemy.Texture, enemy.Position, null, Color.White, enemy.rotationTop, enemy.origin, 1f, SpriteEffects.None, 0);
                }

                foreach (Player player in playerList)
                {
                    spriteBatch.Draw(player.TextureBottom, player.Position, null, Color.White, player.RotationBottom, player.Origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(player.currentWeapon().currentWeaponTexture, player.Position, null, Color.White, player.RotationMid, player.Origin, 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(player.Texture, player.Position, null, Color.White, player.RotationTop, player.Origin, 1f, SpriteEffects.None, 0);
                }

                spriteBatch.End();
                spriteBatch.Begin();

                Vector2 fontOrigin = ArialTitle.MeasureString("Game Paused") / 2;

                spriteBatch.Draw(title.Texture, title.Position, Color.White);
                spriteBatch.Draw(resumeButton.Texture, resumeButton.Position, Color.White);
                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);

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

                spriteBatch.Draw(exitButton.Texture, exitButton.Position, Color.White);

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
