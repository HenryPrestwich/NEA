using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Entities;
using SharpDX.XAudio2;
using System.Collections.Generic;
using System.Linq;
using System.Timers;


namespace mono.Main
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public  SpriteBatch _spriteBatch;
        public Timer _timer;
        public static int GameClock = 0;

        public int GameState;
        //text
        SpriteFont font;
        
        //player
        public Player player;

        

        public Texture2D pixel;

        
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

            //SETTINGS
            HitboxesDrawn = true;

            GameState = GameStates.MainMenu;

            enemyList = new List<Enemy>();

            //MAP
            Map = new Map(12800, 12800, Content.Load<Texture2D>("grass"), Content.Load<Texture2D>("wall"));
            Map.BuildMap();


            //player
            player = new Player(Content.Load<Texture2D>("player"), Map.Rooms[0].CentrePixel.ToVector2());
            

            pixel = Content.Load<Texture2D>("pixel");

           

            


            
            for (int i = 1; i < 3 ; i++)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemy"), Map.Rooms[i].CentrePixel.ToVector2()));
            }
            


            //camera
            camera = new Camera2D(GraphicsDevice.Viewport);

            font = Content.Load<SpriteFont>("font");



        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KB = Keyboard.GetState();
            GamePadState GP = GamePad.GetState(PlayerIndex.One);

            

            //movement
            if (GameClock %  40 == 0)
            {
                foreach (Enemy enemy in enemyList)
                {
                    enemy.SetPath(player, Map);
                }
            }
               
            
           
            player.Move(KB, GP, enemyList, Map);
            player.updateRect();

            foreach (Enemy e in enemyList)
            {
                e.Move(player);
                e.updateRect();
            }   


            camera.Track(player.Position);

            GameClock = (GameClock + 1) %3600; //reset clock every minute
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: camera.GetCamMatrix());

            Map.DrawMap(_spriteBatch);


            player.Draw(_spriteBatch);
            foreach (Character c in enemyList)
            {
                c.Draw(_spriteBatch);
                
            }
            

            //  _spriteBatch.Draw(player.Texture, player.Position, null, Color.White, 0f, player.Centre, 1.5f, SpriteEffects.None, Layers.Entity);

            _spriteBatch.DrawString(font, player.DashCool.ToString(), new Vector2(30, 30), Color.Black);


            _spriteBatch.DrawString(font, GameClock.ToString(), new Vector2(player.Position.X + 700, player.Position.Y + 400), Color.Black);

            if (HitboxesDrawn)
            {
                //DrawHitBoxes(_spriteBatch);
            }
            

            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private void DrawHitBoxes(SpriteBatch spriteBatch)
        {
            foreach (Enemy e in enemyList)
            {
                e.DrawPath(_spriteBatch, pixel);
            }
            Map.DrawRoomsHitbox(_spriteBatch, pixel);
            player.DrawRect(_spriteBatch, pixel);
            foreach (Enemy e in enemyList)
            {
                e.DrawRect(_spriteBatch, pixel);
            }
        }
    }
}