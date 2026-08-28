using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Entities;
using System.Collections.Generic;
using System.Timers;


namespace mono.Main
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public  SpriteBatch _spriteBatch;
        public Timer _timer;
        public static int GameClock = 0;
        //text
        SpriteFont font;
        public const int SCREEN_HEIGHT = 1000;
        public const int SCREEN_WIDTH = 1600; 
        
        //player
        public Player player;

        public Enemy enemy;

        public Texture2D pixel;

        
        public Map Map;

        //Logs
        List<Character> characterList;
        
        //camera
        Camera2D camera;

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

            characterList = new List<Character>();

            //player
            player = new Player(Content.Load<Texture2D>("player"), new Vector2(1400,1400));
            enemy = new Enemy(Content.Load<Texture2D>("enemy"), new Vector2(1200, 1200));

            pixel = Content.Load<Texture2D>("pixel");

           

            Map = new Map(3200, 3200, Content.Load<Texture2D>("grass"), Content.Load<Texture2D>("wall"));
            Map.BuildMap();


            characterList.Add(player);
            characterList.Add(enemy);


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
                enemy.SetPath(player, Map);
            }
               
            
           
            player.Move(KB, GP, characterList, Map);

            foreach (Character character in characterList)
            {
                character.Move(player);
                character.updateRect();
            }   


            camera.Track(player.Position);

            GameClock = (GameClock + 1) %3600; //reset clock every minute
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: camera.GetCamMatrix());

            
            // player.Draw();
            foreach (Character character in characterList)
            {
                character.Draw(_spriteBatch);
                character.DrawRect(_spriteBatch, pixel);
            }
            enemy.DrawPath(_spriteBatch, pixel);
            Map.DrawMap(_spriteBatch);
            Map.DrawRooms(_spriteBatch, pixel);

            //  _spriteBatch.Draw(player.Texture, player.Position, null, Color.White, 0f, player.Centre, 1.5f, SpriteEffects.None, Layers.Entity);

            _spriteBatch.DrawString(font, player.DashCool.ToString(), new Vector2(30, 30), Color.Black);


            _spriteBatch.DrawString(font, GameClock.ToString(), new Vector2(player.Position.X + 700, player.Position.Y + 400), Color.Black);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}