using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExampleGame.States
{
    public class GameState : State

    {
        List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();
        float spawn = 0;

        Texture2D backgroundTexture;
        Player player;
        // Enemy grunt;
        GraphicsDeviceManager _graphics;
        ContentManager _content;
        public GameState(Game1 game, GraphicsDeviceManager graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _graphics = graphicsDevice;
            _content = content;
            player = new Player(content);
            player.Initialize(100f, new Vector2(graphicsDevice.PreferredBackBufferWidth / 2, graphicsDevice.PreferredBackBufferHeight / 2));

            //grunt = new Enemy(content, new Vector2(1, 0));
            //grunt.Initialize(0f, new Vector2(graphicsDevice.PreferredBackBufferWidth / 3, graphicsDevice.PreferredBackBufferHeight / 3));
            //grunt.Load(content.Load<Texture2D>("invader1"));

            player.Load(content.Load<Texture2D>("player"));
            backgroundTexture = content.Load<Texture2D>("spaceBackground");
        }

        public void LoadEnemies()
        {
            int randY = random.Next(100, 400); // height of viewport

            if (spawn >= 1)
            {
                spawn = 0;
                if(enemies.Count() < 4)
                {
                    enemies.Add(new Enemy(_content.Load<Texture2D>("invader2"), new Vector2(1100, randY), _content));
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
            //grunt.Draw(spriteBatch);
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
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
            LoadEnemies();

            //grunt.Update(gameTime);
            player.Update(gameTime);
            player.boundsCheck(_graphics);
        }
    }
}
