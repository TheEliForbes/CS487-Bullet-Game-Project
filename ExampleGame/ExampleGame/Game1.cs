using ExampleGame.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ExampleGame
{
    class BulletFactory
    {
        private ContentManager Content;

        public BulletFactory(ContentManager gameContent)
        {
            Content = gameContent;
        }
        public Bullets bulletFactory(String bulletName, Vector2 position, Vector2 velocity, bool visibility, int bulletType)
        {
            switch (bulletType)
            {
                case 1: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility);
                case 5: return new BulletSpread(position, Content, 1);
                default: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility);
            }
        }
    }
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

            if (Vector2.Distance(position, origin) > 1000)
                isVisible = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 1);
        }
    }

    class BulletSpread : Bullets
    {
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private BulletFactory factory;
        public BulletSpread(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility) 
            : base(newTexture, newPosition, newVelocity, visibility) { }
        public BulletSpread(Vector2 newPosition, ContentManager gameContent, int directionModifier) : base(null, newPosition, Vector2.Zero, true)
        {
            position = newPosition;
            Content = gameContent;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(0, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(5, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(10, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(-5, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(-10, (-10 * directionModifier)), true, 1));
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
        private BulletFactory factory;
        public Enemy(ContentManager gameContent, Vector2 newVelocity)
        {
            Content = gameContent;
            velocity = newVelocity;
            movementTime = 0f; //parameter?
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
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
            Bullets bullet = factory.bulletFactory("bullet", position, new Vector2(0, 10), true, 1);
            
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
        private BulletFactory factory;
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
            factory = new BulletFactory(gameContent);
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
                Bullets newBullet = factory.bulletFactory("bullet", position, new Vector2(0, -10), true, 1);
                newBullet.position = position; //allow setting through constructor?
                
                if (bullets.Count < 20)
                    bullets.Add(newBullet);
            }
            else
            {
                BulletSpread spread = (BulletSpread)factory.bulletFactory("spread", position, Vector2.Zero, true, 5);
                foreach (Bullets bullet in spread.bullets)
                {
                    if (bullets.Count < 80)
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private State _currentState;

        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }
        
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
            IsMouseVisible = true;

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

            _currentState = new MenuState(this, graphics, Content);
            
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
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //     Exit();

            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);
                        
            base.Update(gameTime);
            
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _currentState.Draw(gameTime, spriteBatch);
            
            base.Draw(gameTime);
        }
    }
}
