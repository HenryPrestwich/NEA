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
        public int GameClock = 0;
        //text
        SpriteFont font;

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
            _graphics.PreferredBackBufferHeight = 2000;
            _graphics.PreferredBackBufferWidth = 2400;

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
            player = new Player(Content.Load<Texture2D>("player"));
            enemy = new Enemy(Content.Load<Texture2D>("enemy"));
            

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
            if (KB.GetPressedKeyCount() > 0)
            {
                enemy.SetPath(player, graph);
            }
            
            player.Move(KB, GP);

            foreach (Character character in characterList)
            {
                character.Move();
            }   


            camera.Track(player.Position);


            
            //dash




            // TODO: Add your update logic here

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
            

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}