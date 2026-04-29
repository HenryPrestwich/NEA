using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.Entities;
using System.Collections.Generic;


namespace mono.Main
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public  SpriteBatch _spriteBatch;
        

        //text
        SpriteFont font;

        //player
        Player player;

        Map map;
        Graph graph;

        //Logs
        List<Character> characterList;
        
        //camera
        Camera2D camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            characterList = new List<Character>();

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

            //player
            player = new Player(Content.Load<Texture2D>("player"));
            characterList = new List<Character>();

            map = new Map(Content.Load<Texture2D>("testmap"));

            graph = new Graph(map.texture.Height, map.texture.Width);
            characterList.Add(player);


            //camera
            camera = new Camera2D(GraphicsDevice.Viewport);

            font = Content.Load<SpriteFont>("font");



        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState KB = Keyboard.GetState();
            GamePadState GP = GamePad.GetState(PlayerIndex.One);

            //movement
            player.Move(KB, GP);
            player.Dash(KB, GP);


            camera.Track(player.Position);


            AStar.ASTAR(graph.grid[0, 0], graph.grid[10, 10]);
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