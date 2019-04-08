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
    class MidBossSpread : Bullets
    {
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private BulletFactory factory;
        public MidBossSpread(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility)
            : base(newTexture, newPosition, newVelocity, visibility, null)
        { }
        public MidBossSpread(Vector2 newPosition, ContentManager gameContent, int directionModifier) : base(null, newPosition, Vector2.Zero, true, null)
        {
            Vector2 newPos = new Vector2(position.X + 46, position.Y + 20);
            Content = gameContent;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            bullets.Add(factory.bulletFactory("bullet", newPos, new Vector2(0, (-10 * directionModifier)), true, 1, new MoveUp(7.0)));
            bullets.Add(factory.bulletFactory("bullet", newPos, new Vector2(-5, (-10 * directionModifier)), true, 1, new MoveUpLeft(7.0)));
            bullets.Add(factory.bulletFactory("bullet", newPos, new Vector2(-10, (-10 * directionModifier)), true, 1, new MoveUpLeft(7.0)));
            bullets.Add(factory.bulletFactory("bullet", newPos, new Vector2(-5, (-10 * directionModifier)), true, 1, new MoveUpRight(7.0)));
            bullets.Add(factory.bulletFactory("bullet", newPos, new Vector2(-10, (-10 * directionModifier)), true, 1, new MoveUpRight(7.0)));
        }
    }
}
