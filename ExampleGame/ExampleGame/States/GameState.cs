using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using ExampleGame.Enemies;
using ExampleGame.PlayerFolder;
using ExampleGame.Movements;
using ExampleGame.waves;
using ExampleGame.Entities.BulletTypes;
using ExampleGame.Entities;

namespace ExampleGame.States
{
    public class GameState : State
    {
        List<Enemy> _enemies = new List<Enemy>();
        Random random = new Random();
        private static Timer GameTimer;
        Texture2D backgroundTexture;
        Player player;
        GraphicsDeviceManager _graphics;
        int waveNumber;
        WaveBuilder waves;
        int curLives = 3;
        Texture2D lifeTexture;
        Reward reward;

        public GameState(Game1 game, GraphicsDeviceManager graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _graphics = graphicsDevice;
            _content = content;
            player = new Player(content);
            player.Initialize(100f, new Vector2((graphicsDevice.PreferredBackBufferWidth / 2), (graphicsDevice.PreferredBackBufferHeight)-75));
            player.Load(content.Load<Texture2D>("player"));
            backgroundTexture = content.Load<Texture2D>("spaceBackground");
            lifeTexture = _content.Load<Texture2D>("lives3");

            reward = new Reward(content);
            //reward.Load(content.Load<Texture2D>("reward"));

            // This implementation will probably change when we read
            // in time values from the JSON script file
            SetWaveTimers();
            waves = new WaveBuilder();
            EnemyWave wave1 = waves.BuildWave(1, _content);
            _enemies = wave1.getAllEnemies();
            waveNumber = 1;
        }

        // Adds each enemy to the enemy list, implementing Factory Pattern
        void AddEnemy(EnemyCreator creator)
        {
            int randY = random.Next(100, 400); // height of viewport is 400

            Enemy enemy = creator.AddEnemy(_content, randY);
            _enemies.Add(enemy);
        }

        private void SetWaveTimers()
        {
            int i = 1;
            for(; i < 5; ++i)
            {
                GameTimer = new System.Timers.Timer(25000*i);
                GameTimer.Elapsed += loadNextWave;
                GameTimer.Enabled = true;
                GameTimer.AutoReset = false;
            }
            
            // Auto-win timer
            GameTimer = new System.Timers.Timer(25000*i);
            GameTimer.Elapsed += AutomaticWin;
            GameTimer.Enabled = true;
            GameTimer.AutoReset = false;
        }

        private void loadNextWave(Object source, ElapsedEventArgs e)
        {
            waveNumber++;
            //_enemies.Clear(); //Allow Overlapping Waves
            EnemyWave newWave = waves.BuildWave(waveNumber, _content);
            _enemies.AddRange(newWave.getAllEnemies());
        }

        private void AutomaticWin(Object source, ElapsedEventArgs e)
        {
            _game.ChangeState(new WinState(_game, _graphicsDevice, _content));
        }

        public void CleanupEnemies()
        {
            // clean up enemies when they go off the screen
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (!_enemies[i].isVisible) 
                {
                    _enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void removeAllBullets()
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.removeBullets(); //remove enemy bullets
            }
            player.removeBullets(); //remove players bullets
        }

        public void updateLivesTexture()
        {
            if(player.getLives() == 1)
                lifeTexture = _content.Load<Texture2D>("lives1");
            else if(player.getLives() == 2)
                lifeTexture = _content.Load<Texture2D>("lives2");
            else if (player.getLives() == 3)
                lifeTexture = _content.Load<Texture2D>("lives3");
            else if (player.getLives() == 4)
                lifeTexture = _content.Load<Texture2D>("lives4");
            else if (player.getLives() == 5)
                lifeTexture = _content.Load<Texture2D>("lives5");
            else if (player.getLives() == 6)
                lifeTexture = _content.Load<Texture2D>("lives6");
            else if (player.getLives() == 7)
                lifeTexture = _content.Load<Texture2D>("lives7");
            else if (player.getLives() == 8)
                lifeTexture = _content.Load<Texture2D>("lives8");
            else if (player.getLives() == 9)
                lifeTexture = _content.Load<Texture2D>("lives9");

        }

        public void checkHit()
        {
            if (curLives > player.getLives())
            {
                curLives = player.getLives();
                removeAllBullets();
                updateLivesTexture();
            }
        }

        public void IsPlayerDead()
        {
            if (player.getLives() == 0)
            {
                _game.ChangeState(new LoseState(_game, _graphicsDevice, _content));
            }
        }

        public void DidPlayerWin()
        {
            if (player.IsWinner() == 1)
            {
                _game.ChangeState(new WinState(_game, _graphicsDevice, _content));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                backgroundTexture,
                new Rectangle(0, 0, 800, 480),
                Color.White);

            spriteBatch.Draw(
                lifeTexture,
                new Vector2(lifeTexture.Width / 2, lifeTexture.Height / 2),
                null,
                Color.White,
                0f,
                new Vector2(lifeTexture.Width / 2, lifeTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

            player.Draw(spriteBatch);

            if (waveNumber == 4) // load reward on the 4th wave
            {
                if (reward != null)
                {
                    reward.Draw(spriteBatch);
                }
            }

            foreach (Enemy enemy in _enemies.ToList())
            {
                enemy.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
        
        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (waveNumber == 4) // when there is a reward
            {
                if (reward != null)
                {
                    if (reward.position.X <= player.position.X + 5 && reward.position.Y <= player.position.Y + 5
                                && reward.position.X >= player.position.X - 5 && reward.position.Y >= player.position.Y - 5)
                    {
                        player.AddLife(5); // update player life
                        updateLivesTexture();
                        reward = null; // remove reward
                    }
                }
            }
        
            foreach(Enemy enemy in _enemies)
            {
                enemy.Update(_graphics, gameTime);
                for (int i = 0; i < enemy.bullets.Count; i++)
                {
                    if (enemy.bullets[i].position.X <= player.position.X + 3 && enemy.bullets[i].position.Y <= player.position.Y + 3
                            && enemy.bullets[i].position.X >= player.position.X - 3 && enemy.bullets[i].position.Y >= player.position.Y - 3
                            && player.invincible == false)
                    {
                        player.takeHit();
                        enemy.bullets[i].isVisible = false;
                    }
                }
                for (int i = 0; i < player.bullets.Count; i++)
                {
                    if (player.bullets[i].position.X <= enemy.position.X + 10 && player.bullets[i].position.Y <= enemy.position.Y + 10
                        && player.bullets[i].position.X >= enemy.position.X - 10 && player.bullets[i].position.Y >= enemy.position.Y - 10)
                    {
                        enemy.isVisible = false;
                        player.bullets[i].isVisible = false;
                    }
                }
            }
            CleanupEnemies();
            player.Update(gameTime);
            player.boundsCheck(_graphics);
            IsPlayerDead();
            DidPlayerWin();
            checkHit();
        }
    }
}
