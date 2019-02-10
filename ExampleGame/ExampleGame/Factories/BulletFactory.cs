using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ExampleGame.Entities.BulletTypes;

namespace ExampleGame.Factories
{
    class BulletFactory
    {
        private ContentManager Content;

        public BulletFactory(ContentManager gameContent)
        {
            Content = gameContent;
        }
        public Bullets bulletFactory(String bulletName, Vector2 position, Vector2 velocity, bool visibility, int bulletType)
        {
            switch (bulletType)
            {
                case 1: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility);
                case 5: return new BulletSpread(position, Content, 1);
                case 6: return new FinalBossBullets(position, Content, 1);
                case 7: return new MidBossSpread(position, Content, 1);
                default: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility);
            }
        }
    }
}
