using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame
{
    class Enemies
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;

        public bool isVisible = true;

        Random random = new Random();
        int randX, randY;

        public Enemies(Texture2D newTexture, Vector2 newposition)
        {
            texture = newTexture;
            position = newposition;

            randY = random.Next(-4, 4);
            randX = random.Next(-4, 1);

            velocity = new Vector2(randX, randY);

        }
        public void Update(GraphicsDeviceManager graphics)
        {
            position += velocity;

            if(position.Y <= 0 || position.Y >= graphics.PreferredBackBufferHeight - texture.Height)
            {
                velocity.Y = -velocity.Y;
            }
            if (position.X < 0 - texture.Width)
            {
                isVisible = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
