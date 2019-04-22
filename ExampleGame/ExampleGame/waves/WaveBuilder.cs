using ExampleGame.Enemies;
using ExampleGame.Movements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
namespace ExampleGame.waves
{
    class WaveBuilder
    {
        private EnemyCreator _creator;
        private String dirpath = "../../../../content/";
        public EnemyWave BuildWave(int wavenum, ContentManager _content)
        {
            EnemyWave newWave = new EnemyWave(wavenum);
            if(wavenum == 1)
            {
                buildGruntAWave1(newWave, _content);
                buildGruntBWave1(newWave, _content);
                buildWaveFromFile(newWave, "wave1.json", _content);
            } else if (wavenum == 2)
            {
                buildGruntAWave2(newWave, _content);
                buildGruntBWave2(newWave, _content);
            }
            else if (wavenum == 3)
            {
                buildMidBossWave(newWave, _content);
                buildGruntAWave1(newWave, _content);
                buildGruntBWave1(newWave, _content);
            }
            else if (wavenum == 4)
            {
                buildGruntAWave3(newWave, _content);
                buildGruntBWave2(newWave, _content);
            }
            else if (wavenum == 5)
            {
                buildFinalBossWave(newWave, _content);
                buildGruntAWave2(newWave, _content);
                buildGruntBWave1(newWave, _content);
            }
            return newWave;
        }

        private void buildGruntAWave1(EnemyWave newWave,ContentManager _content)
        {
            _creator = new ConcreteGruntACreator();
            for (int j = 0; j < 4; j++)
            {
                EnemyMovements moves = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves.addMovement(new MoveRight(6.0));
                    moves.addMovement(new MoveLeft(6.0));
                }
                Vector2 pos = new Vector2(j * 100, j * 100);
                Vector2 vel = new Vector2(-1, 1);
                newWave.addEnemy(_creator.CreateEnemy(pos, vel, _content, moves));
            }
        }
        private void buildGruntBWave1(EnemyWave newWave, ContentManager _content)
        {
            _creator = new ConcreteGruntBCreator();
            int height = 50;
            for (int j = 1; j < 5; j++)
            {
                EnemyMovements moves = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves.addMovement(new MoveLeft(6.0));
                    moves.addMovement(new MoveRight(6.0));
                    
                }
                Vector2 pos = new Vector2(800 - (j * 100), height );
                Vector2 vel = new Vector2(-1, 1);
                newWave.addEnemy(_creator.CreateEnemy(pos, vel, _content, moves));
                height += 100;
            }
        }
        private void buildGruntAWave2(EnemyWave newWave, ContentManager _content)
        {
            _creator = new ConcreteGruntACreator();
            for (int j = 0; j < 4; j++)
            {
                EnemyMovements moves = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves.addMovement(new MoveDown(3.0));
                    moves.addMovement(new MoveUp(3.0));
                }
                Vector2 pos = new Vector2(j * 100, 0);
                Vector2 vel = new Vector2(-1, 1);
                newWave.addEnemy(_creator.CreateEnemy(pos, vel, _content, moves));
            }
        }
        private void buildGruntAWave3(EnemyWave newWave, ContentManager _content)
        {
            _creator = new ConcreteGruntACreator();
            for (int j = 0; j < 4; j++)
            {
                EnemyMovements moves = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves.addMovement(new MoveDownRight(3.0));
                    moves.addMovement(new MoveUpLeft(3.0));
                }
                Vector2 pos = new Vector2(j * 100, 0);
                Vector2 vel = new Vector2(-1, 1);
                newWave.addEnemy(_creator.CreateEnemy(pos, vel, _content, moves));
            }
        }
        private void buildGruntBWave2(EnemyWave newWave, ContentManager _content)
        {
            _creator = new ConcreteGruntBCreator();
            int width = 350;
            for (int j = 1; j < 5; j++)
            {
                EnemyMovements moves = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves.addMovement(new MoveUp(3.0));
                    moves.addMovement(new MoveDown(3.0));

                }
                Vector2 pos = new Vector2(width, 300);
                Vector2 vel = new Vector2(-1, 1);
                newWave.addEnemy(_creator.CreateEnemy(pos, vel, _content, moves));
                width += 100;
            }
        }
        private void buildMidBossWave(EnemyWave newWave, ContentManager _content)
        {
            _creator = new ConcreteMidBossCreator();
            Vector2 pos = new Vector2(300, 50);
            newWave.addEnemy(_creator.CreateEnemy(pos, Vector2.One, _content, new EnemyMovements()));
        }
        private void buildFinalBossWave(EnemyWave newWave, ContentManager _content)
        {
            _creator = new ConcreteFinalBossCreator();
            Vector2 pos = new Vector2(300, 50);
            newWave.addEnemy(_creator.CreateEnemy(pos, Vector2.One, _content, new EnemyMovements()));
        }
        
        private void buildWaveFromFile(EnemyWave newWave, String filename, ContentManager _content)
        {
            waveObj JSON = new JavaScriptSerializer().Deserialize<waveObj>(File.ReadAllText(dirpath + filename));
            
            for(int i = 0; i < JSON.enemies.Count; i++)
            {
                EnemyMovements moves = new EnemyMovements();
                if (JSON.enemies[i].type == "GruntA")
                {
                    _creator = new ConcreteGruntACreator();
                }
                else if(JSON.enemies[i].type == "GruntB")
                {
                    _creator = new ConcreteGruntBCreator();
                }
                else if (JSON.enemies[i].type == "MidBoss")
                {
                    _creator = new ConcreteMidBossCreator();
                }
                else if (JSON.enemies[i].type == "FinalBoss")
                {
                    _creator = new ConcreteFinalBossCreator();
                }

            }
        }
    }
    class waveObj
    {
        public List<enemyObj> enemies;
    }
    class enemyObj
    {
        public string type;
        public List<int> startPos;
        public List<int> startVel;
        public List<moveObj> movements;
        public int movementRepetitions;
    }
    class moveObj
    {
        public string type;
        public double duration;
    }
}
