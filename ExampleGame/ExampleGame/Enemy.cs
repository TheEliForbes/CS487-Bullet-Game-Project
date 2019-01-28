using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame
{
    class Enemy
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        private List<Bullets> bullets;
        private BulletFactory factory;
        public bool isVisible = true;
        
        private float speed;
        private float movementTime;
        private bool rightward;

        Random random = new Random();
        int randX, randY;

        public Enemy(Texture2D newTexture, Vector2 newposition, ContentManager gameContent)
        {
            texture = newTexture;
            position = newposition;

            randY = random.Next(-4, 4);
            randX = random.Next(-4, 1);

            movementTime = 0f; //parameter?
            velocity = new Vector2(randX, randY);

            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);

        }

        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            speed = initSpeed;
            position = initPosition;
        }

        public void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            // logic for shooting
            movementTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            rightward = (movementTime < 2f) ? true : false;
            movementTime = (((int)movementTime) == 4) ? 0 : movementTime;
            if ((int)movementTime % 2 == 0)
            { //This^^ is kinda funky, could probably be improved
                shoot();
            }

            // logic for enemy to move
            position += velocity;

            if(position.Y <= 0 || position.Y >= graphics.PreferredBackBufferHeight - texture.Height)
            {
                velocity.Y = -velocity.Y;
            }
            if (position.X < 0 - texture.Width)
            {
                isVisible = false;
            }

            bulletsUpdateAndCleanup(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            foreach (Bullets bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void shoot()
        {
            Bullets bullet = factory.bulletFactory("bullet", position, new Vector2(0, 10), true, 1);

            if (bullets.Count < 20)
            {
                bullets.Add(bullet);
            }
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
    }
}
