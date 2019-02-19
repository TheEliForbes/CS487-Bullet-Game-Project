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
            // Create a timer with a two second interval.
            GameTimer = new System.Timers.Timer(10000); 
            GameTimer.Elapsed += loadMidBoss;
            GameTimer.Enabled = true;
            GameTimer.AutoReset = false;

            // Create a timer with a 10 second interval.
            GameTimer = new System.Timers.Timer(20000);
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
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // update
        }
    }
}
