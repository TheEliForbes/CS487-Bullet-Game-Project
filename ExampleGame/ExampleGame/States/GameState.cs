﻿using System;
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
        ContentManager _content;

        // hardcoded values for now, when we read in a JSON script
        // file later, we can set these numbers to match what the file says?
        int gruntACount = 3;
        int gruntBCount = 2;
        //int midBossCount = 1;
        //int finalBossCount = 1;

        public GameState(Game1 game, GraphicsDeviceManager graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _graphics = graphicsDevice;
            _content = content;
            player = new Player(content);
            player.Initialize(100f, new Vector2(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2));
            player.Load(content.Load<Texture2D>("player"));
            backgroundTexture = content.Load<Texture2D>("spaceBackground");

            // This implementation will probably change when we read
            // in time values from the JSON script file
            SetMidBossTimer();
            for (int j = 0; j < 4; j++)
            {
                EnemyMovements moves1 = new EnemyMovements();
                for (int i = 0; i < 10; i++)
                {
                    moves1.addMovement(new MoveLeft(6.0));
                    moves1.addMovement(new MoveRight(6.0));
                }
                Vector2 pos = new Vector2(j * 100, j * 100);
                Vector2 vel = new Vector2(1, 0);
                _enemies.Add(new GruntA(pos, vel, _content, moves1));
            }
        }

        // Adds each enemy to the enemy list, implementing Factory Pattern
        void AddEnemy(EnemyCreator creator)
        {
            int randY = random.Next(100, 400); // height of viewport is 400

            Enemy enemy = creator.AddEnemy(_content, randY);
            _enemies.Add(enemy);
        }

        private void SetMidBossTimer()
        {
            // Mid boss appearance timer
            GameTimer = new System.Timers.Timer(48000); 
            GameTimer.Elapsed += loadMidBoss;
            GameTimer.Enabled = true;
            GameTimer.AutoReset = false;

            // Mid boss disappearance timer
            GameTimer = new System.Timers.Timer(75000);
            //GameTimer.Elapsed += clearBosses;
            GameTimer.Enabled = true;
            GameTimer.AutoReset = false;

            // Final boss appearance timer
            GameTimer = new System.Timers.Timer(90000);
            GameTimer.Elapsed += loadFinalBoss;
            GameTimer.Enabled = true;
            GameTimer.AutoReset = false;
        }

        private void loadMidBoss(Object source, ElapsedEventArgs e)
        {
            AddEnemy(new ConcreteMidBossCreator());
        }

        private void loadFinalBoss(Object source, ElapsedEventArgs e)
        {
            AddEnemy(new ConcreteFinalBossCreator());
        }

        public void LoadEnemies()
        {
            int randY = random.Next(100, 400); // height of viewport

           
            
            // spawn number of gruntA
            for (int i = 0; i < gruntACount; i++, gruntACount--)
            {
                AddEnemy(new ConcreteGruntACreator());
            }

            // spawn number of gruntB
            for (int i = 0; i < gruntBCount; i++, gruntBCount--)
            {
                AddEnemy(new ConcreteGruntBCreator());
            }
            
            // spawn number of midboss
            /*
            for (int i = 0; i < gruntACount; i++)
            {
                AddEnemy(new ConcreteMidBossCreator());
            }
            */

            // spawn number of boss
            /*
            for (int i = 0; i < gruntACount; i++)
            {
                AddEnemy(new ConcreteFinalBossCreator());
            }
            */

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

        public void IsPlayerDead()
        {
            if (player.GetHealth() == 0)
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

            player.Draw(spriteBatch);

            foreach (Enemy enemy in _enemies.ToList())
            {
                enemy.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
        
        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
        
            foreach(Enemy enemy in _enemies)
            {
                enemy.Update(_graphics, gameTime);
            }
            LoadEnemies();
            player.Update(gameTime);
            player.boundsCheck(_graphics);
            IsPlayerDead();
            DidPlayerWin();
        }
    }
}
