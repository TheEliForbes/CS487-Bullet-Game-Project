using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ExampleGame.Factories;
using ExampleGame.Movements;

namespace ExampleGame.Entities.BulletTypes
{
    class FinalBossTopBottom : Bullets
    {
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private BulletFactory factory;
        Random random = new Random();
        public FinalBossTopBottom(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility)
            : base(newTexture, newPosition, newVelocity, visibility, null)
        { }
        public FinalBossTopBottom(Vector2 newPosition, ContentManager gameContent, int directionModifier) : base(null, newPosition, Vector2.Zero, true, null)
        {
            Vector2 randomPos = new Vector2(position.X + 94, position.Y + 63);

            Content = gameContent;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            int randDir = 0;
            for (int i = 0; i < 30; i++)
            {
                BulletMovements moves = new BulletMovements();
                for (int j = 0; j < 15; j++)
                {
                    randDir = random.Next(0, 3);
                    if (randDir == 0)
                        moves.addMovement(new MoveDown(0.5));
                    else if (randDir == 1)
                        moves.addMovement(new MoveDownLeft(0.5));
                    else if (randDir == 2)
                        moves.addMovement(new MoveDownRight(0.5));
                }
                bullets.Add(factory.bulletFactory("bullet", new Vector2(-50 + i*35, -i*10), new Vector2(1, (3 * directionModifier)), true, 1, moves));
            }
            for (int i = 0; i < 30; i++)
            {
                BulletMovements moves = new BulletMovements();
                for (int j = 0; j < 20; j++)
                {
                    randDir = random.Next(0, 3);
                    if (randDir == 0)
                        moves.addMovement(new MoveUp(0.5));
                    else if (randDir == 1)
                        moves.addMovement(new MoveUpLeft(0.5));
                    else if (randDir == 2)
                        moves.addMovement(new MoveUpRight(0.5));
                }
                bullets.Add(factory.bulletFactory("bullet", new Vector2(-50 + i * 35, 450 + i*10), new Vector2(1, (3 * directionModifier)), true, 1, moves));
            }
        }
    }
}
