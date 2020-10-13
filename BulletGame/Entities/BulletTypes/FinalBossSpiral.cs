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
    class FinalBossSpiral : Bullets
    {
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private BulletFactory factory;
        public FinalBossSpiral(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility)
            : base(newTexture, newPosition, newVelocity, visibility, null)
        { }
        public FinalBossSpiral(Vector2 newPosition, ContentManager gameContent, int directionModifier) : base(null, newPosition, Vector2.Zero, true, null)
        {
            Content = gameContent;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            BulletMovements down = new BulletMovements();
            down.addMovement(new MoveDown(.65));
            down.addMovement(new MoveDownRight(.65));
            down.addMovement(new MoveRight(10.5));

            BulletMovements downLeft = new BulletMovements();
            downLeft.addMovement(new MoveDownRight(.65));
            downLeft.addMovement(new MoveRight(.65));
            downLeft.addMovement(new MoveUpRight(10.5));

            BulletMovements downRight = new BulletMovements();
            downRight.addMovement(new MoveDownLeft(.65));
            downRight.addMovement(new MoveDown(.65));
            downRight.addMovement(new MoveDownRight(10.5));

            BulletMovements right = new BulletMovements();
            right.addMovement(new MoveLeft(.65));
            right.addMovement(new MoveDownLeft(.65));
            right.addMovement(new MoveDown(10.5));

            BulletMovements left = new BulletMovements();
            left.addMovement(new MoveRight(.65));
            left.addMovement(new MoveUpRight(.65));
            left.addMovement(new MoveUp(10.5));

            BulletMovements upLeft = new BulletMovements();
            upLeft.addMovement(new MoveUpRight(.65));
            upLeft.addMovement(new MoveUp(.65));
            upLeft.addMovement(new MoveUpLeft(10.5));

            BulletMovements up = new BulletMovements();
            up.addMovement(new MoveUp(.65));
            up.addMovement(new MoveUpLeft(.65));
            up.addMovement(new MoveLeft(10.5));

            BulletMovements upRight = new BulletMovements();
            upRight.addMovement(new MoveUpLeft(.65));
            upRight.addMovement(new MoveLeft(.65));
            upRight.addMovement(new MoveDownLeft(10.5));

            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, down));
            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, downLeft));
            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, downRight));
            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, right));
            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, left));
            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, upLeft));
            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, up));
            bullets.Add(factory.bulletFactory("bullet", new Vector2(position.X + 94, position.Y + 40), new Vector2(4, (3 * directionModifier)), true, 1, upRight));
        }
    }
}
