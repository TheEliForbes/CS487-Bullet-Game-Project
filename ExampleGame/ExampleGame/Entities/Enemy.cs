using ExampleGame.Entities.BulletTypes;
using ExampleGame.Factories;
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
    abstract class Enemy
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        public bool isVisible = true;
        public List<Bullets> bullets;
        public BulletFactory factory;

        public float speed;
        public float movementTime;
        public bool rightward;

        public abstract void Initialize(float initSpeed, Vector2 initPosition);
        public abstract void Update(GraphicsDeviceManager graphics, GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void shoot();
        public abstract void bulletsUpdateAndCleanup(GameTime gameTime);
        public abstract void removeBullets();

    }

    class FinalBoss : Enemy
    {


        public FinalBoss( Vector2 newPosition, ContentManager gameContent)
        {
            texture = gameContent.Load<Texture2D>("finalBoss");
            position = newPosition;

            // = random.Next(-4, 4);
            //randX = random.Next(-4, 1);

            movementTime = 0f; //parameter?
            velocity = new Vector2(1,0);

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
            FinalBossBullets spread = (FinalBossBullets)factory.bulletFactory("finalBossBullets", position, Vector2.Zero, true, 6);
            foreach (Bullets bullet in spread.bullets)
            {
                if (bullets.Count < 400)
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

    class MidBoss : Enemy
    {
        private List<Bullets> bullets;
        private BulletFactory factory;

        private float speed;
        private float movementTime;
        private bool rightward;

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

    class GruntA : Enemy
    {
        private List<Bullets> bullets;
        private BulletFactory factory;

        private float speed;
        private float movementTime;
        private bool rightward;

        Random random = new Random();
        int randX, randY;

        public GruntA(Vector2 newPosition, ContentManager gameContent)
        {
            texture = gameContent.Load<Texture2D>("invader1");
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
        public override void removeBullets()
        {
            bullets.Clear();
        }

    }

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
        public override void removeBullets()
        {
            bullets.Clear();
        }

    }

}
