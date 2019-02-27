using ExampleGame.Entities;
using ExampleGame.Entities.BulletTypes;
using ExampleGame.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ExampleGame.PlayerFolder
{
    class Player : Entity
    {
        private float speed;
        private float originalSpeed;
        private int slowModeModifier;
        private bool isGod;
        private List<Bullets> bullets; //may depend on design
        private BulletFactory factory;
        ContentManager Content;
        private int health = 10;
        private int winner = 0;

        //Key mapping
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;
        Keys shootKey = Keys.Space;
        Keys slowMode = Keys.S;
        Keys godMode = Keys.G;

        Keys win = Keys.W;
        Keys die = Keys.D;
        
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

            // for testing the game states
            if (kstate.IsKeyDown(die) && pastKey.IsKeyUp(die))
                health = 0; // die -> see lose screen

            // for testing the win states
            if (kstate.IsKeyDown(win) && pastKey.IsKeyUp(win))
                winner = 1; // win -> see win screen

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

        public int GetHealth()
        {
            return health;
        }

        public int IsWinner()
        {
            return winner;
        }
    }
}
