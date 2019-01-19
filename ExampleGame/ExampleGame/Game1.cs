using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ExampleGame
{
    abstract class Entity
    {
        public Texture2D texture;
        public Vector2 position;

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }

    class Bullets : Entity
    {        
        public Vector2 velocity;
        public Vector2 origin = Vector2.Zero;
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
        public override void Update(GameTime gameTime)
        {
            position += velocity;

            if (Vector2.Distance(position, origin) > 600)
                isVisible = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 1);
        }
    }

    class BulletSpread
    {
        private Vector2 position;
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private int directionModifier; //change up|down
        public BulletSpread(Vector2 newPosition, ContentManager gameContent, int directionModifier)
        {
            position = newPosition;
            Content = gameContent;
            bullets = new List<Bullets>();
            bullets.Add(bulletFactory("bullet", new Vector2(0, (-10 * directionModifier)), true));
            bullets.Add(bulletFactory("bullet", new Vector2(5, (-10 * directionModifier)), true));
            bullets.Add(bulletFactory("bullet", new Vector2(10, (-10 * directionModifier)), true));
            bullets.Add(bulletFactory("bullet", new Vector2(-5, (-10 * directionModifier)), true));
            bullets.Add(bulletFactory("bullet", new Vector2(-10, (-10 * directionModifier)), true));
        }
        public Bullets bulletFactory(String bulletName, Vector2 velocity, bool visibility)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility);
        }
    }

    class Enemy : Entity
    {
        private Vector2 velocity;
        private float speed;
        private float movementTime;
        private bool rightward;
        private List<Bullets> bullets; //may depend on design
        private ContentManager Content;

        public Enemy(ContentManager gameContent, Vector2 newVelocity)
        {
            Content = gameContent;
            velocity = newVelocity;
            movementTime = 0f; //parameter?
            bullets = new List<Bullets>();
        }
        public Bullets bulletFactory(String bulletName, Vector2 velocity, bool visibility)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility);
        }
        public void bulletsUpdateAndCleanup(GameTime gameTime)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);

                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            speed = initSpeed;
            position = initPosition;
        }
        public void Load(Texture2D initTexture)
        {
            texture = initTexture;
        }
        public override void Update(GameTime gameTime)
        {
            movementTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rightward = (movementTime < 2f) ? true : false;
            position = (rightward == true) ? position + velocity : position - velocity;
            movementTime = (((int)movementTime) == 4) ? 0 : movementTime;
            if ((int)movementTime % 2 == 0)
            { //This^^ is kinda funky, could probably be improved
                shoot();
            }

            bulletsUpdateAndCleanup(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                position,
                null,
                Color.White,
                0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
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
        
    }

    class Player : Entity
    {
        private float speed;
        private float originalSpeed;
        private int slowModeModifier;
        private bool isGod;
        private List<Bullets> bullets; //may depend on design
        ContentManager Content;

        //Key mapping
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;
        Keys shootKey = Keys.Space;
        Keys slowMode = Keys.S;
        Keys godMode = Keys.G;
        KeyboardState pastKey; //2nd most recent key command

        public Player(ContentManager gameContent)
        {
            Content = gameContent;
            isGod = false;
            bullets = new List<Bullets>();
        }
        public Bullets bulletFactory(String bulletName)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName));
        }
        public Bullets bulletFactory(String bulletName, Vector2 velocity, bool visibility)
        {
            return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility);
        }
        public void bulletsUpdateAndCleanup(GameTime gameTime)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);

                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            originalSpeed = speed = initSpeed;
            position = initPosition;
            slowModeModifier = 4;
        }
        public void Load(Texture2D initTexture)
        {
            texture = initTexture;
        }
        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(slowMode))
                speed = (speed == originalSpeed) ? speed / slowModeModifier : speed * slowModeModifier;

            if (kstate.IsKeyDown(godMode) && pastKey.IsKeyUp(godMode))
                isGod = !isGod;
                
            if (kstate.IsKeyDown(upKey))
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(downKey))
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(leftKey))
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(rightKey))
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(shootKey) && pastKey.IsKeyUp(shootKey))
            {
                shoot();
            }
            pastKey = Keyboard.GetState();

            bulletsUpdateAndCleanup(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                position,
                null,
                Color.White,
                0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
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
                Bullets newBullet = bulletFactory("bullet", new Vector2(0, -10), true);
                newBullet.position = position; //allow setting through constructor?
                
                if (bullets.Count < 20)
                    bullets.Add(newBullet);
            }
            else
            {
                BulletSpread spread = new BulletSpread(position, Content, 1);
                foreach (Bullets bullet in spread.bullets)
                {
                    if (bullets.Count < 20)
                    {
                        bullets.Add(bullet);
                    }
                }
            }
        }
        public void boundsCheck(GraphicsDeviceManager graphics)
        {
            //----------------v This MathHelper.Min(...) blob is essentially collision detection?
            position.X = MathHelper.Min(MathHelper.Max(texture.Width / 2, position.X), graphics.PreferredBackBufferWidth - texture.Width / 2);
            position.Y = MathHelper.Min(MathHelper.Max(texture.Height / 2, position.Y), graphics.PreferredBackBufferHeight - texture.Height / 2);
        }      
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
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
        Keys shootKey = Keys.Space;
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
