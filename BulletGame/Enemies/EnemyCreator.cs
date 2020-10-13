using ExampleGame.Movements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Enemies
{
    abstract class EnemyCreator
    {
        // factory method
        public abstract Enemy CreateEnemy(ContentManager content, int randY);

        public abstract Enemy CreateEnemy(Vector2 pos, Vector2 vel, ContentManager _content, EnemyMovements move);

        public Enemy AddEnemy(ContentManager content, int randY)
        {
            return CreateEnemy(content, randY);
        }
    }

    class ConcreteGruntACreator : EnemyCreator
    {
        public override Enemy CreateEnemy(ContentManager content, int randY)
        {
           return new GruntA(new Vector2(1100, randY), content);
        }

        public override Enemy CreateEnemy(Vector2 pos, Vector2 vel, ContentManager _content, EnemyMovements moves)
        {
            return new GruntA(pos, vel, _content, moves);
        }
    }
    class ConcreteGruntBCreator : EnemyCreator
    {
        public override Enemy CreateEnemy(ContentManager content, int randY)
        {
            return new GruntB(new Vector2(1100, randY), content);
        }

        public override Enemy CreateEnemy(Vector2 pos, Vector2 vel, ContentManager _content, EnemyMovements moves)
        {
            return new GruntB(pos, vel, _content, moves);
        }
    }

    class ConcreteMidBossCreator : EnemyCreator
    {
        public override Enemy CreateEnemy(ContentManager content, int randY)
        {
            return new MidBoss(new Vector2(300, 50), content);
        }
        public Enemy CreateEnemy(Vector2 pos, ContentManager _content)
        {
            return new MidBoss(pos, _content);
        }
        public override Enemy CreateEnemy(Vector2 pos, Vector2 vel, ContentManager _content, EnemyMovements moves)
        {
            return new MidBoss(pos, _content);
        }
    }

    class ConcreteFinalBossCreator : EnemyCreator
    {
        public override Enemy CreateEnemy(ContentManager content, int randY)
        {
            return new FinalBoss(new Vector2(100, 50), content);
        }
        public Enemy CreateEnemy(Vector2 pos, ContentManager _content)
        {
            return new FinalBoss(pos, _content);
        }
        public override Enemy CreateEnemy(Vector2 pos, Vector2 vel, ContentManager _content, EnemyMovements moves)
        {
            return new FinalBoss(pos, _content);
        }
    }
}
