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

        Map map;
        public Graph graph;

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
            player = new Player(Content.Load<Texture2D>("player"), new Vector2(10, 10));
            enemy = new Enemy(Content.Load<Texture2D>("enemy"), new Vector2(800, 800));
            

            map = new Map(Content.Load<Texture2D>("testmap"));

            graph = new Graph(map.texture.Height, map.texture.Width);


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
            if (GameClock %  150 == 0)
            {
                enemy.SetPath(player, graph);
            }
               
            
           
            player.Move(KB, GP);

            foreach (Character character in characterList)
            {
                character.Move();
            }   


            camera.Track(player.Position);

            GameClock = (GameClock++) %3600; //reset clock every minute
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: camera.GetCamMatrix());

            map.Draw(_spriteBatch);
            // player.Draw();
            foreach (Character character in characterList)
            {
                character.Draw(_spriteBatch);
            }

            //  _spriteBatch.Draw(player.Texture, player.Position, null, Color.White, 0f, player.Centre, 1.5f, SpriteEffects.None, Layers.Entity);

            _spriteBatch.DrawString(font, player.DashCool.ToString(), new Vector2(30, 30), Color.Black);


            _spriteBatch.DrawString(font, GameClock.ToString(), new Vector2(player.Position.X + 700, player.Position.Y + 400), Color.Black);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}