﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ExampleGame.Entities
{
    class Reward : Entity
    {
        ContentManager Content;

        public Reward(ContentManager gameContent)
        {
            Content = gameContent;
            position.Y = 200;
            position.X = 100;
            texture = gameContent.Load<Texture2D>("reward");
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Vector2(100, 200),
                null, Color.White,
                0,
                new Vector2(texture.Width / 2, texture.Height / 2),
                1,
                SpriteEffects.None,
                0
            );
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
