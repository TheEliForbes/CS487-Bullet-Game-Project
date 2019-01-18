using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ExampleGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
   
    class Bullets
    {
        public Texture2D texture;

        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;

        public bool isVisible;

        public Bullets(Texture2D newTexture)
        {
            texture = newTexture;
            isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 1);
        }
    }

    class Enemy
    {
        private Texture2D enemyTexture;
        private Vector2 enemyPosition;
        private float enemySpeed;

        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            enemySpeed = initSpeed;
            enemyPosition = initPosition;
        }
        public void Load(Texture2D initTexture)
        {
            enemyTexture = initTexture;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                enemyTexture,
                enemyPosition,
                null,
                Color.White,
                0f,
                new Vector2(enemyTexture.Width / 2, enemyTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);
        }
    }

    class Player
    {
        private Texture2D playerTexture;
        private Vector2 playerPosition;
        private float playerSpeed;

        List<Bullets> bullets = new List<Bullets>(); //may depend on design

        //Key mapping
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;
        Keys spacebar = Keys.Space;
        KeyboardState pastKey; //2nd most recent key command

        ContentManager Content;

        public Player(ContentManager gameContent)
        {
            Content = gameContent;
        }
        public Bullets bulletFactory(String bulletName)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName));
        }

        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            playerSpeed = initSpeed;
            playerPosition = initPosition;
        }
        public void Load(Texture2D initTexture)
        {
            playerTexture = initTexture;
        }
        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(upKey))
                playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(downKey))
                playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(leftKey))
                playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(rightKey))
                playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(spacebar) && pastKey.IsKeyUp(Keys.Space))
            {
                shoot(Content);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                playerTexture,
                playerPosition,
                null,
                Color.White,
                0f,
                new Vector2(playerTexture.Width / 2, playerTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

            foreach (Bullets bullet in bullets)
                bullet.Draw(spriteBatch);
        }
        public void shoot(ContentManager Content)
        {
            Bullets newBullet = bulletFactory("bullet");
            newBullet.velocity = new Vector2(0, -10);
            newBullet.position = playerPosition;
            newBullet.isVisible = true;

            if (bullets.Count < 20)
                bullets.Add(newBullet);
        }
        public void UpdateBullets()
        {
            foreach (Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                if (Vector2.Distance(bullet.position, playerPosition) > 600)
                    bullet.isVisible = false;
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }
        public void boundsCheck(GraphicsDeviceManager graphics)
        {
            //----------------v This MathHelper.Min(...) blob is essentially collision detection?
            playerPosition.X = MathHelper.Min(MathHelper.Max(playerTexture.Width / 2, playerPosition.X), graphics.PreferredBackBufferWidth - playerTexture.Width / 2);
            playerPosition.Y = MathHelper.Min(MathHelper.Max(playerTexture.Height / 2, playerPosition.Y), graphics.PreferredBackBufferHeight - playerTexture.Height / 2);
        }      
    }
    public class Game1 : Game
    {
        Player player;
        Enemy grunt;
        //Key mapping
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;
        Keys spacebar = Keys.Space;
        KeyboardState pastKey; //2nd most recent key command

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(Content);
            player.Initialize(100f, new Vector2(graphics.PreferredBackBufferWidth / 2,
                                       graphics.PreferredBackBufferHeight / 2));

            grunt = new Enemy();
            grunt.Initialize(0f, new Vector2(graphics.PreferredBackBufferWidth / 3,
                                         graphics.PreferredBackBufferHeight / 3));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player.Load(Content.Load<Texture2D>("player"));
            grunt.Load(Content.Load<Texture2D>("invader1"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            player.Update(gameTime);
            player.boundsCheck(graphics);

            base.Update(gameTime);
            player.UpdateBullets();
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            player.Draw(spriteBatch);
            grunt.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
