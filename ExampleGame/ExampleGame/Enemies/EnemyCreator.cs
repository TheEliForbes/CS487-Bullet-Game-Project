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
    }

    class ConcreteGruntBCreator : EnemyCreator
    {
        public override Enemy CreateEnemy(ContentManager content, int randY)
        {
            return new GruntB(new Vector2(1100, randY), content);
        }
    }

    class ConcreteMidBossCreator : EnemyCreator
    {
        public override Enemy CreateEnemy(ContentManager content, int randY)
        {
            return new MidBoss(new Vector2(300, 50), content);
        }
    }

    class ConcreteFinalBossCreator : EnemyCreator
    {
        public override Enemy CreateEnemy(ContentManager content, int randY)
        {
            return new FinalBoss(new Vector2(100, 50), content);
        }
    }
}
