﻿using ExampleGame.Entities.BulletTypes;
using ExampleGame.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ExampleGame.Enemies
{
    class MidBoss : Enemy
    {
        public MidBoss(Vector2 newPosition, ContentManager gameContent)
        {
            texture = gameContent.Load<Texture2D>("midBoss");
            position = newPosition;

            // = random.Next(-4, 4);
            //randX = random.Next(-4, 1);

            movementTime = 0f; //parameter?
            velocity = new Vector2(1, 0);

            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            lives = 2;
            width = 96;
            height = 30;
        }

        public override void Initialize(float initSpeed, Vector2 initPosition)
        {
            speed = initSpeed;
            position = initPosition;
        }

        public override void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            movementTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rightward = (movementTime < 2f) ? true : false;
            position = (rightward == true) ? position + velocity : position - velocity;
            movementTime = (((int)movementTime) == 4) ? 0 : movementTime;
            if ((int)movementTime % 4 == 0)
            { //This^^ is kinda funky, could probably be improved
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

            MidBossSpread spread = (MidBossSpread)factory.bulletFactory("midBossSpread", position, Vector2.Zero, true, 7);
            foreach (Bullets bullet in spread.bullets)
            {
                if (bullets.Count < 300)
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
