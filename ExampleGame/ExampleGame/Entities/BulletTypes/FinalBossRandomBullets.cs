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
    class FinalBossRandomBullets : Bullets
    {
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private BulletFactory factory;
        public FinalBossRandomBullets(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility)
            : base(newTexture, newPosition, newVelocity, visibility, null)
        { }
        public FinalBossRandomBullets(Vector2 newPosition, ContentManager gameContent, int directionModifier) : base(null, newPosition, Vector2.Zero, true, null)
        {
            Random random = new Random();
            Content = gameContent;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);

            Vector2 pos = new Vector2(random.Next(0,1000), position.Y +random.Next(0,40));
            int randDir = random.Next(0, 2);
            Movement mov = new MoveDownLeft(7.0);
                if(randDir == 0)
                    mov = new MoveDown(7.0);
                else if (randDir == 1)
                    mov = new MoveDownLeft(7.0);
                else if(randDir == 2)
                    mov = new MoveDownRight(7.0);

            bullets.Add(factory.bulletFactory("bullet", pos, new Vector2(1, (2 * directionModifier)), true, 1, mov));
        }
    }
}
