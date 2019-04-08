using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ExampleGame.Movements;

namespace ExampleGame.Entities.BulletTypes
{
    class Bullets : Entity
    {
        public Vector2 velocity;
        public Vector2 origin = Vector2.Zero;
        public bool isVisible;
        private Movement move;

        public Bullets(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility, Movement newMove)
        {
            texture = newTexture;
            isVisible = visibility;
            position = newPosition;
            velocity = newVelocity;
            move = newMove;
        }
        public override void Update(GameTime gameTime)
        {
            position = move.getNewPosition(position, velocity);

            if (Vector2.Distance(position, origin) > 1000)
                isVisible = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 1);
        }
    }
}
