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
        public Bullets(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity)
        {
            texture = newTexture;
            isVisible = false;
            position = newPosition;
            velocity = newVelocity;
        }
        public Bullets(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility)
        {
            texture = newTexture;
            isVisible = visibility;
            position = newPosition;
            velocity = newVelocity;
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
        private Vector2 velocity;
        private float enemySpeed;
        private float movementTime = 0f;
        private bool rightward;
        
        private List<Bullets> bullets = new List<Bullets>(); //may depend on design

        private ContentManager Content;

        public Enemy(ContentManager gameContent, Vector2 newVelocity)
        {
            Content = gameContent;
            velocity = newVelocity;
        }
        public Bullets bulletFactory(String bulletName, Vector2 velocity, bool visibility)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName), enemyPosition, velocity, visibility);
        }
        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            enemySpeed = initSpeed;
            enemyPosition = initPosition;
        }
        public void Load(Texture2D initTexture)
        {
            enemyTexture = initTexture;
        }
        public void Update(GameTime gameTime)
        {
            movementTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rightward = (movementTime < 2f) ? true : false;
            enemyPosition = (rightward == true) ? enemyPosition + velocity : enemyPosition - velocity;
            movementTime = (((int)movementTime) == 4) ? 0 : movementTime;
            if ((int)movementTime % 2 == 0)
            { //This^^ is kinda funky, could probably be improved
                shoot();
            }
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
            foreach (Bullets bullet in bullets)
                bullet.Draw(spriteBatch);
        }
        public void shoot()
        {
            Bullets bullet = bulletFactory("bullet", new Vector2(0, 10), true);
            
            if (bullets.Count < 20)
            {
                bullets.Add(bullet);                
            }
        }
        public void UpdateBullets()
        {
            foreach (Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                
                if (Vector2.Distance(bullet.position, enemyPosition) > 600)
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
    }

    class Player
    {
        private Texture2D playerTexture;
        private Vector2 playerPosition;
        private float playerSpeed;
        private float originalSpeed;
        private int slowModeModifier;
        private bool isGod;

        private List<Bullets> bullets = new List<Bullets>(); //may depend on design

        //Key mapping
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;
        Keys spacebar = Keys.Space;
        Keys slowMode = Keys.S;
        Keys godMode = Keys.G;
        KeyboardState pastKey; //2nd most recent key command

        ContentManager Content;

        public Player(ContentManager gameContent)
        {
            Content = gameContent;
            isGod = false;
        }
        public Bullets bulletFactory(String bulletName)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName));
        }
        public Bullets bulletFactory(String bulletName, Vector2 velocity, bool visibility)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName), playerPosition, velocity, visibility);
        }

        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            originalSpeed = playerSpeed = initSpeed;
            playerPosition = initPosition;
            slowModeModifier = 4;
        }
        public void Load(Texture2D initTexture)
        {
            playerTexture = initTexture;
        }
        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(slowMode))
                playerSpeed = (playerSpeed == originalSpeed) ? playerSpeed / slowModeModifier : playerSpeed * slowModeModifier;

            if (kstate.IsKeyDown(godMode) && pastKey.IsKeyUp(godMode))
                isGod = !isGod;
                
            if (kstate.IsKeyDown(upKey))
                playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(downKey))
                playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(leftKey))
                playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(rightKey))
                playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(spacebar) && pastKey.IsKeyUp(spacebar))
            {
                shoot();
            }
            pastKey = Keyboard.GetState();
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
        public void shoot()
        {
            if (!isGod)
            {
                Bullets newBullet = bulletFactory("bullet");
                newBullet.velocity = new Vector2(0, -10);
                newBullet.position = playerPosition;
                newBullet.isVisible = true;

                if (bullets.Count < 20)
                    bullets.Add(newBullet);
            }
            else
            {
                Bullets centerBullet = bulletFactory("bullet", new Vector2(0, -10), true);
                Bullets rightBullet = bulletFactory("bullet", new Vector2(5, -10), true);
                Bullets rightBullet2 = bulletFactory("bullet", new Vector2(10, -10), true);
                Bullets leftBullet = bulletFactory("bullet", new Vector2(-5, -10), true);
                Bullets leftBullet2 = bulletFactory("bullet", new Vector2(-10, -10), true);

                if (bullets.Count < 20)
                {
                    bullets.Add(centerBullet);
                    bullets.Add(rightBullet);
                    bullets.Add(rightBullet2);
                    bullets.Add(leftBullet);
                    bullets.Add(leftBullet2);
                }
            }



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
        Texture2D backgroundTexture;

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

            grunt = new Enemy(Content, new Vector2(1, 0));
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
            backgroundTexture = Content.Load<Texture2D>("spaceBackground");
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

            grunt.Update(gameTime);
            player.Update(gameTime);
            player.boundsCheck(graphics);
                        
            base.Update(gameTime);
            player.UpdateBullets();
            grunt.UpdateBullets();
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
            spriteBatch.Draw(
                backgroundTexture,
                new Rectangle(0, 0, 800, 480),
                Color.White);

            player.Draw(spriteBatch);
            grunt.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
