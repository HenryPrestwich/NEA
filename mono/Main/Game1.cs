using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Entities;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;


namespace mono.Main
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public Timer _timer;
        public static int GameClock = 0;

        public int GameState;
        //text
        SpriteFont font;

        //player
        public Player player;



        public Map Map;

        //Logs
        List<Enemy> enemyList;
        
        

        //camera
        Camera2D camera;


        //Settings
        public const int SCREEN_HEIGHT = 1000;
        public const int SCREEN_WIDTH = 1600;
        public bool HitboxesDrawn;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _timer = new Timer();
            _timer.Interval = 100;
            _timer.Start();
            TextureManager.CreateTextures(Content);
            

            

            //SETTINGS
            HitboxesDrawn = true;

            GameState = GameStates.MainMenu;

            enemyList = new List<Enemy>();






            //MAP
            Map = new Map(12800, 12800);
            Map.BuildMap();


            //player
            player = new Player(Map.Rooms[0].CentrePixel.ToVector2());


            


            for (int i = 1; i < 3; i++)
            {
                enemyList.Add(new Enemy(Map.Rooms[i].CentrePixel.ToVector2()));
            }



            //camera
            camera = new Camera2D(GraphicsDevice.Viewport);

            font = Content.Load<SpriteFont>("font");



        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.UpdateInput();

            ProjectileManager.UpdateProjectiles(player, enemyList, Map);

            //movement
            if (GameClock % 20 == 0)
            {
                foreach (Enemy enemy in enemyList)
                {
                    enemy.SetPath(player, Map);
                }
            }

            

            player.Move(enemyList, Map);
            player.Attack();

            for (int i = 0; i < enemyList.Count; i++)
            {
                Enemy e = enemyList[i];
                e.Move(player, enemyList);
                e.updateRect();
                e.SetState(player);
                if (e.state == EnemyState.DEAD)
                {
                  enemyList.Remove(e);
                }
            }


            camera.Track(player.Position);

            GameClock = (GameClock + 1) % 3600; 
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: camera.GetCamMatrix());

            Map.DrawMap(_spriteBatch, player, SCREEN_HEIGHT, SCREEN_WIDTH);


            player.Draw(_spriteBatch);
            foreach (Character c in enemyList)
            {
                c.Draw(_spriteBatch);

            }

            ProjectileManager.DrawProjectiles(_spriteBatch);
            

            _spriteBatch.DrawString(font, player.DashCool.ToString(), new Vector2(30, 30), Color.Black);


            _spriteBatch.DrawString(font, GameClock.ToString(), new Vector2(player.Position.X + 700, player.Position.Y + 400), Color.Black);

            if (HitboxesDrawn)
            {
                DrawHitBoxes();
            }


            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private void DrawHitBoxes()
        {
            foreach (Enemy e in enemyList)
            {
                e.DrawPath(_spriteBatch);
                e.DrawRect(_spriteBatch);
            }
            Map.DrawRoomsHitbox(_spriteBatch);
            player.DrawRect(_spriteBatch);
            ProjectileManager.DrawHitboxes(_spriteBatch);
        }
    }
}