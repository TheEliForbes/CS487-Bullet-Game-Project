using ExampleGame.Entities.BulletTypes;
using ExampleGame.Factories;
using ExampleGame.Movements;
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
        protected EnemyMovements moves;
        public float speed;
        public float movementTime;
        public bool rightward;

        public abstract void Initialize(float initSpeed, Vector2 initPosition);
        public abstract void Update(GraphicsDeviceManager graphics, GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void shoot();
        public abstract void bulletsUpdateAndCleanup(GameTime gameTime);

    }
}
