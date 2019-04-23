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
    class BulletSpread : Bullets
    {
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private BulletFactory factory;
        public BulletSpread(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility)
            : base(newTexture, newPosition, newVelocity, visibility, null)
        { }
        public BulletSpread(Vector2 newPosition, ContentManager gameContent, int directionModifier) : base(null, newPosition, Vector2.Zero, true, null)
        {
            position = newPosition;
            Content = gameContent;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            BulletMovements up = new BulletMovements();
            BulletMovements upLeft = new BulletMovements();
            BulletMovements upRight = new BulletMovements();
            up.addMovement(new MoveUp(7.0));
            upLeft.addMovement(new MoveUpLeft(7.0));
            upRight.addMovement(new MoveUpRight(7.0));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(0, (10 * directionModifier)), true, 1, up));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(5, (10 * directionModifier)), true, 1, upLeft));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(10, (10 * directionModifier)), true, 1,upLeft));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(5, (10 * directionModifier)), true, 1, upRight));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(10, (10 * directionModifier)), true, 1, upRight));
        }
    }
}
