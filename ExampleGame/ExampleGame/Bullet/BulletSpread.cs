using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExampleGame.Bullet
{
    class BulletSpread : Bullets
    {
        public List<Bullets> bullets; //may depend on design
        private ContentManager Content;
        private BulletFactory factory;
        public BulletSpread(Texture2D newTexture, Vector2 newPosition, Vector2 newVelocity, bool visibility)
            : base(newTexture, newPosition, newVelocity, visibility) { }
        public BulletSpread(Vector2 newPosition, ContentManager gameContent, int directionModifier) : base(null, newPosition, Vector2.Zero, true)
        {
            position = newPosition;
            Content = gameContent;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(0, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(5, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(10, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(-5, (-10 * directionModifier)), true, 1));
            bullets.Add(factory.bulletFactory("bullet", position, new Vector2(-10, (-10 * directionModifier)), true, 1));
        }
    }
}
