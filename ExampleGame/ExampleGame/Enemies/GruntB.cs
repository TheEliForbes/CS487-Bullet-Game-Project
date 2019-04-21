using ExampleGame.Entities.BulletTypes;
using ExampleGame.Factories;
using ExampleGame.Movements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExampleGame.Enemies
{
    class GruntB : Enemy
    {
        private Movement currentMove;
        private Double movementStartTime;

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
            lives = 1;
        }

        public GruntB(Vector2 newPosition, Vector2 newVelocity, ContentManager gameContent, EnemyMovements newMoves)
        {
            texture = gameContent.Load<Texture2D>("invader2");
            position = newPosition;

            movementTime = 0f;
            this.velocity = newVelocity;
            this.moves = newMoves;

            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            lives = 1;
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
            if (moves == null)
            {
                position += velocity;
            }
            else if (currentMove == null)
            {
                currentMove = moves.GetMovement();
                movementStartTime = gameTime.TotalGameTime.TotalSeconds;
            }
            else if (currentMove.seconds <= (gameTime.TotalGameTime.TotalSeconds - movementStartTime))
            {
                moves.popMovement();
                currentMove = null;
            }
            else
            {
                position = currentMove.getNewPosition(position, velocity);
            }

            //if (position.Y <= 0 || position.Y >= graphics.PreferredBackBufferHeight - texture.Height)
            //{
            //    velocity.Y = -velocity.Y;
            //}
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
            Vector2 gruntBulletPos = new Vector2(position.X + 14, position.Y + 9);
            Bullets bullet = factory.bulletFactory("bullet", gruntBulletPos, new Vector2(0, 10), true, 1);

            if (bullets.Count < 70)
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
        public override void removeBullets()
        {
            bullets.Clear();
        }

    }
}
