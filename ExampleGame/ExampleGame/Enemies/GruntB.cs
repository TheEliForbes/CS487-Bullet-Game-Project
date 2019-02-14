using ExampleGame.Bullet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExampleGame.Enemies
{
    class GruntB : Enemy
    {
        private List<Bullets> bullets;
        private BulletFactory factory;

        private float speed;
        private float movementTime;
        private bool rightward;

        Random random = new Random();
        int randX, randY;

        public GruntB(Vector2 newPosition, ContentManager gameContent)
        {
            texture = gameContent.Load<Texture2D>("invader2");
            position = newPosition;

            randY = random.Next(-4, 4);
            randX = random.Next(-4, 1);

            movementTime = 0f; //parameter?
            velocity = new Vector2(randX, randY);

            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
        }

        public override void Initialize(float initSpeed, Vector2 initPosition)
        {
            speed = initSpeed;
            position = initPosition;
        }

        public override void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            // logic for shooting
            movementTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rightward = (movementTime < 2f) ? true : false;
            movementTime = (((int)movementTime) == 4) ? 0 : movementTime;
            if ((int)movementTime % 4 == 0)
            { //This^^ is kinda funky, could probably be improved
                shoot();
            }

            // logic for enemy to move
            position += velocity;

            if (position.Y <= 0 || position.Y >= graphics.PreferredBackBufferHeight - texture.Height)
            {
                velocity.Y = -velocity.Y;
            }
            if (position.X < 0 - texture.Width)
            {
                isVisible = false;
            }

            bulletsUpdateAndCleanup(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            foreach (Bullets bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public override void shoot()
        {
            Bullets bullet = factory.bulletFactory("bullet", position, new Vector2(0, 10), true, 1);

            if (bullets.Count < 20)
            {
                bullets.Add(bullet);
            }
        }

        public override void bulletsUpdateAndCleanup(GameTime gameTime)
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

    }
}
