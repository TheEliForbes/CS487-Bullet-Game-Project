using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
namespace ExampleGame.States
{
    public class GameState : State

    {
        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> bosses = new List<Enemy>();
        Random random = new Random();
        float spawn = 0;
        private static System.Timers.Timer GameTimer;
        Texture2D backgroundTexture;
        Player player;
        GraphicsDeviceManager _graphics;
        ContentManager _content;
        public GameState(Game1 game, GraphicsDeviceManager graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _graphics = graphicsDevice;
            _content = content;
            player = new Player(content);
            player.Initialize(100f, new Vector2(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2));
            player.Load(content.Load<Texture2D>("player"));
            backgroundTexture = content.Load<Texture2D>("spaceBackground");
            SetBossTimer();
        }
        private void SetBossTimer()
        {
            // Mid boss appearance timer
            GameTimer = new System.Timers.Timer(48000); 
            GameTimer.Elapsed += loadMidBoss;
            GameTimer.Enabled = true;
            GameTimer.AutoReset = false;

            // Mid boss disappearance timer
            GameTimer = new System.Timers.Timer(75000);
            GameTimer.Elapsed += clearBosses;
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
            bosses.Add(new MidBoss(new Vector2(300, 50), _content));
        }
        private void clearBosses(Object source, ElapsedEventArgs e)
        {
            bosses.Clear();
        }
        private void loadFinalBoss(Object source, ElapsedEventArgs e)
        {
            bosses.Add(new FinalBoss(new Vector2(100, 50), _content));
        }
        public void LoadBoss()
        {
            // the bosses are kept separate for now because they currently don't go off the screen

            for (int i = 0; i < bosses.Count; i++)
            {
                if (!bosses[i].isVisible) // when the boss is off the screen remove it 
                {
                    bosses.RemoveAt(i);
                    i--;
                }
            }

        }

        public void LoadEnemies()
        {
            int randY = random.Next(100, 400); // height of viewport
            

            if (spawn >= 1)
            {
                spawn = 0;
        
                // the normal enemies go off the screen, so as they are deleted, new ones are spawned
                if (enemies.Count() < 5)
                {
                    enemies.Add(new GruntA(new Vector2(1100, randY), _content));
                    enemies.Add(new GruntB(new Vector2(1100, randY), _content));
                }

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (!enemies[i].isVisible) // when the enemy is off the screen remove it 
                    {
                        enemies.RemoveAt(i);
                        i--;
                    }
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

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (Enemy boss in bosses)
            {
                boss.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // implement
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();


            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach(Enemy enemy in enemies)
            {
                enemy.Update(_graphics, gameTime);
            }
            foreach (Enemy boss in bosses)
            {
                boss.Update(_graphics, gameTime);
            }
            LoadEnemies();
            LoadBoss();
            
            player.Update(gameTime);
            player.boundsCheck(_graphics);
        }
    }
}
