using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ExampleGame.Entities.BulletTypes;
using ExampleGame.Movements;

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
            BulletMovements newMove = new BulletMovements();
            newMove.addMovement(new MoveDown(7.0));
            switch (bulletType)
            {
                case 1: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility, newMove);
                case 5: return new BulletSpread(position, Content, 1);
                case 6: return new FinalBossBullets(position, Content, 1);
                case 7: return new MidBossSpread(position, Content, 1);
                case 8: return new FinalBossRandomBullets(position, Content, 1);
                case 9: return new FinalBossTopBottom(position, Content, 1);
                case 10: return new FinalBossSpiral(position, Content, 1);
                default: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility, newMove);
            }
        }
        public Bullets bulletFactory(String bulletName, Vector2 position, Vector2 velocity, bool visibility, int bulletType, int direction)
        {
            BulletMovements newMove = new BulletMovements();
            newMove.addMovement(new MoveDown(7.0));
            switch (bulletType)
            {
                case 1: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility, newMove);
                case 5: return new BulletSpread(position, Content, direction);
                case 6: return new FinalBossBullets(position, Content, direction);
                case 7: return new MidBossSpread(position, Content, direction);
                case 8: return new FinalBossRandomBullets(position, Content, 1);
                case 9: return new FinalBossTopBottom(position, Content, 1);
                case 10: return new FinalBossSpiral(position, Content, 1);
                default: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility, newMove);
            }
        }
        public Bullets bulletFactory(String bulletName, Vector2 position, Vector2 velocity, bool visibility, int bulletType, BulletMovements move)
        {
            switch (bulletType)
            {
                case 1: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility, move);
                case 5: return new BulletSpread(position, Content, 1);
                case 6: return new FinalBossBullets(position, Content, 1);
                case 7: return new MidBossSpread(position, Content, 1);
                case 8: return new FinalBossRandomBullets(position, Content, 1);
                case 9: return new FinalBossTopBottom(position, Content, 1);
                case 10: return new FinalBossSpiral(position, Content, 1);
                default: return new Bullets(Content.Load<Texture2D>(bulletName), position, velocity, visibility, move);
            }
        }
    }
}
