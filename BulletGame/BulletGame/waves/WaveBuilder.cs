using ExampleGame.Enemies;
using ExampleGame.Factories;
using ExampleGame.Movements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
                //buildGruntAWave1(newWave, _content);
                //buildGruntBWave1(newWave, _content);
                buildWaveFromFile(newWave, "wave5.json", _content);
            } else if (wavenum == 2)
            {
                //buildGruntAWave2(newWave, _content);
                //buildGruntBWave2(newWave, _content);
                buildWaveFromFile(newWave, "wave2.json", _content);
            }
            else if (wavenum == 3)
            {
                //buildMidBossWave(newWave, _content);
                //buildGruntAWave1(newWave, _content);
                //buildGruntBWave1(newWave, _content);
                buildWaveFromFile(newWave, "wave3.json", _content);
            }
            else if (wavenum == 4)
            {
                buildGruntAWave3(newWave, _content);
                buildGruntBWave2(newWave, _content);
                //buildWaveFromFile(newWave, "wave4.json", _content);
            }
            else if (wavenum == 5)
            {
                buildFinalBossWave(newWave, _content);
                buildGruntAWave2(newWave, _content);
                buildGruntBWave1(newWave, _content);
                //buildWaveFromFile(newWave, "wave5.json", _content);
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
            waveObj JSON = JsonConvert.DeserializeObject<waveObj>(File.ReadAllText(dirpath + filename));
            
            for(int enemyIdx = 0; enemyIdx < JSON.enemies.Count; enemyIdx++)
            {
                
                if (JSON.enemies[enemyIdx].type == "GruntA")
                {
                    _creator = new ConcreteGruntACreator();
                }
                else if(JSON.enemies[enemyIdx].type == "GruntB")
                {
                    _creator = new ConcreteGruntBCreator();
                }
                else if (JSON.enemies[enemyIdx].type == "MidBoss")
                {
                    _creator = new ConcreteMidBossCreator();
                }
                else if (JSON.enemies[enemyIdx].type == "FinalBoss")
                {
                    _creator = new ConcreteFinalBossCreator();
                }
                EnemyMovements moves = new EnemyMovements();
                MovementFactory movementFactory = new MovementFactory();
                for(int rep = 0; rep < JSON.enemies[enemyIdx].movementRepetitions; rep++)
                {
                    for(int moveIdx = 0; moveIdx < JSON.enemies[enemyIdx].movements.Count; moveIdx++)
                    {
                        moves.addMovement(movementFactory.makeMovement(JSON.enemies[enemyIdx].movements[moveIdx].type, JSON.enemies[enemyIdx].movements[moveIdx].duration));
                    }
                }

                Vector2 pos = new Vector2(JSON.enemies[enemyIdx].startPos[0], JSON.enemies[enemyIdx].startPos[1]);
                Vector2 vel = new Vector2(JSON.enemies[enemyIdx].startVel[0], JSON.enemies[enemyIdx].startVel[1]);
                newWave.addEnemy(_creator.CreateEnemy(pos, vel, _content, moves));

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
