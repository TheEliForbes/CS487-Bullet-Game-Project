using ExampleGame.Entities.BulletTypes;
using ExampleGame.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ExampleGame.Enemies
{
    class FinalBoss : Enemy
    {
        public FinalBoss(Vector2 newPosition, ContentManager gameContent)
        {
            texture = gameContent.Load<Texture2D>("finalBoss");
            position = newPosition;

            movementTime = 0f; //parameter?
            velocity = new Vector2(1, 0);

            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            lives = 3;
            width = 192;
            height = 84;
        }

        public override void Initialize(float initSpeed, Vector2 initPosition)
        {
            speed = initSpeed;
            position = initPosition;
        }

        public override void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            if (lives > 1)
            {
                rightward = (movementTime < 2f) ? true : false;
                position = (rightward == true) ? position + velocity : position - velocity;
            }
            movementTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            movementTime = (((int)movementTime) == 4) ? 0 : movementTime;
            if (lives == 3)
            {
                shoot(); //shoot forever if 3 lives (random bullets)
            }
            else if (bullets.Count <= 0 && lives == 2)
            {
                shoot();
            }
            else if((int)movementTime % 4 == 0 && lives == 1)
            {
                shoot();
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
            if(lives == 3)
            {
                FinalBossRandomBullets spread = (FinalBossRandomBullets)factory.bulletFactory("finalBossRandomBullets", position, Vector2.Zero, true, 8);
                foreach (Bullets bullet in spread.bullets)
                {
                    bullets.Add(bullet);
                }
            }
            else if (lives == 2)
            {
                FinalBossTopBottom spread = (FinalBossTopBottom)factory.bulletFactory("finalBossTopBottom", position, Vector2.Zero, true, 9);
                foreach (Bullets bullet in spread.bullets)
                {
                    bullets.Add(bullet);
                }
            }
            else if (lives == 1)
            {
                FinalBossSpiral spread = (FinalBossSpiral)factory.bulletFactory("finalBossSpiral", position, Vector2.Zero, true, 10);
                foreach (Bullets bullet in spread.bullets)
                {
                    bullets.Add(bullet);
                }
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
