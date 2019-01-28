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
        Texture2D backgroundTexture;
        Player player;
        Enemy grunt;
        GraphicsDeviceManager _graphics;
        public GameState(Game1 game, GraphicsDeviceManager graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _graphics = graphicsDevice;
            player = new Player(content);
            player.Initialize(100f, new Vector2(graphicsDevice.PreferredBackBufferWidth / 2,
                                       graphicsDevice.PreferredBackBufferHeight / 2));

            grunt = new Enemy(content, new Vector2(1, 0));
            grunt.Initialize(0f, new Vector2(graphicsDevice.PreferredBackBufferWidth / 3,
                                         graphicsDevice.PreferredBackBufferHeight / 3));

            player.Load(content.Load<Texture2D>("player"));
            grunt.Load(content.Load<Texture2D>("invader1"));
            backgroundTexture = content.Load<Texture2D>("spaceBackground");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                backgroundTexture,
                new Rectangle(0, 0, 800, 480),
                Color.White);

            player.Draw(spriteBatch);
            grunt.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // implement
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            grunt.Update(gameTime);
            player.Update(gameTime);
            player.boundsCheck(_graphics);
        }
    }
}
