using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.Entities;
using System.Collections.Generic;
using System.Timers;


namespace mono.Main
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public Timer _timer;
        public static int GameClock = 0;

        public GameStates GameState;
        //text
        SpriteFont font;

        



        public Map Map;

        



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
            TM.CreateTextures(Content);




            //SETTINGS
            HitboxesDrawn = true;

            GameState = GameStates.MainMenu;

            






            //MAP
            Map = new Map(12800, 12800);
            Map.BuildMap();


            //player
            CM.CreatePlayer(Map.Rooms[0].CentrePixel.ToVector2());





            for (int i = 1; i < 3; i++)
            {
                CM.CreateEnemy(Map.Rooms[i].CentrePixel.ToVector2());
            }



            //camera
            camera = new Camera2D(GraphicsDevice.Viewport);

            font = Content.Load<SpriteFont>("font");



        }

        protected override void Update(GameTime gameTime)
        {
            IM.UpdateInput();

            PM.UpdateProjectiles(CM.Player,CM.EnemyList, Map);

            //movement
            if (GameClock % 20 == 0)
            {
                foreach (Enemy enemy in CM.EnemyList)
                {
                    enemy.SetPath(CM.Player, Map);
                }
            }



            CM.Player.Move(CM.EnemyList, Map);
            CM.Player.Attack();

            for (int i = 0; i < CM.EnemyList.Count; i++)
            {
                Enemy e = CM.EnemyList[i];
                e.Move(CM.Player, CM.EnemyList);
                e.updateRect();
                e.SetState(CM.Player);
                if (e.state == EnemyState.DEAD)
                {
                    CM.EnemyList.Remove(e);
                }
            }


            camera.Track(CM.Player.Position);

            GameClock = (GameClock + 1) % 3600;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: camera.GetCamMatrix());

            Map.DrawMap(_spriteBatch, CM.Player, SCREEN_HEIGHT, SCREEN_WIDTH);


            CM.Player.Draw(_spriteBatch);
            foreach (Character c in CM.EnemyList)
            {
                c.Draw(_spriteBatch);

            }

            PM.DrawProjectiles(_spriteBatch);


            _spriteBatch.DrawString(font, CM.Player.DashCool.ToString(), new Vector2(30, 30), Color.Black);


            _spriteBatch.DrawString(font, GameClock.ToString(), new Vector2(CM.Player.Position.X + 700, CM.Player.Position.Y + 400), Color.Black);

            if (HitboxesDrawn)
            {
                DrawHitBoxes();
            }


            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private void DrawHitBoxes()
        {
            foreach (Enemy e in CM.EnemyList)
            {
                e.DrawPath(_spriteBatch);
                e.DrawRect(_spriteBatch);
            }
            Map.DrawRoomsHitbox(_spriteBatch);
            CM.Player.DrawRect(_spriteBatch);
            PM.DrawHitboxes(_spriteBatch);
        }
    }
}