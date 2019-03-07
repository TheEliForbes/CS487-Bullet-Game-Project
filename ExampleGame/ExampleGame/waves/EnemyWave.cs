using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ExampleGame.waves
{
    class EnemyWave
    {
        private List<Enemy> enemies;
        public int waveNum;
        public EnemyWave(int newWaveNumber)
        {
            enemies = new List<Enemy>();
            this.waveNum = newWaveNumber;
        }
        public EnemyWave(int newWaveNumber,List<Enemy> newEnemyList)
        {
            enemies = newEnemyList;
            this.waveNum = newWaveNumber;
        }
        public void addEnemy(Enemy newEnemy)
        {
            enemies.Add(newEnemy);
        }
        public List<Enemy> getAllEnemies()
        {
            return enemies;
        }
    }
}
