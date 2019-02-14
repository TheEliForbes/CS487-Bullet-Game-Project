using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExampleGame.Bullet
{
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
}
