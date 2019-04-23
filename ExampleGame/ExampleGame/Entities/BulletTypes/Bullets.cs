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
        private Movement currentMove;
        private Double movementStartTime;
        public Vector2 velocity;
        public Vector2 origin = Vector2.Zero;
        public bool isVisible;
        private Movement move;
        private BulletMovements moves;

        public Bullets(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility, BulletMovements newMoves)
        {
            texture = newTexture;
            isVisible = visibility;
            position = newPosition;
            velocity = newVelocity;
            moves = newMoves;
        }
        public override void Update(GameTime gameTime)
        {
            //position = moves.GetMovement().getNewPosition(position, velocity);
            //position = move.getNewPosition(position, velocity);

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

            if (Vector2.Distance(position, origin) > 1000)
                isVisible = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 1);
        }
    }
}
