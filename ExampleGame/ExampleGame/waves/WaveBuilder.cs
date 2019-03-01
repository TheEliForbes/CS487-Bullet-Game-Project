using ExampleGame.Enemies;
using ExampleGame.Movements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.waves
{
    class WaveBuilder
    {
        public EnemyWave BuildWave(int wavenum,ContentManager _content)
        {
            EnemyWave newWave = new EnemyWave(wavenum);
            if(wavenum == 1)
            {
                buildGruntAWave(newWave, _content);
                buildGruntBWave(newWave, _content);
            }
            return newWave;
        }

        private void buildGruntAWave(EnemyWave newWave,ContentManager _content)
        {
            for (int j = 0; j < 4; j++)
            {
                EnemyMovements moves = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves.addMovement(new MoveLeft(6.0));
                    moves.addMovement(new MoveRight(6.0));
                }
                Vector2 pos = new Vector2(j * 100, j * 100);
                Vector2 vel = new Vector2(1, 0);
                newWave.addEnemy(new GruntA(pos, vel, _content, moves));
            }
        }
        private void buildGruntBWave(EnemyWave newWave, ContentManager _content)
        {
            for (int j = 0; j < 4; j++)
            {
                EnemyMovements moves = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves.addMovement(new MoveLeft(6.0));
                    moves.addMovement(new MoveRight(6.0));
                }
                Vector2 pos = new Vector2(j * 75, j * 75);
                Vector2 vel = new Vector2(1, 0);
                newWave.addEnemy(new GruntB(pos, vel, _content, moves));
            }
        }
    }
}
