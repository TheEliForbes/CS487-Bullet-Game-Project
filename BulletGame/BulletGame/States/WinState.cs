using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ExampleGame.States
{
    public class WinState : State
    {
        private List<Component> _components;
        public WinState(Game1 game, GraphicsDeviceManager graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexturePlay = _content.Load<Texture2D>("Controls/ButtonPlay");
            var buttonTextureQuit = _content.Load<Texture2D>("Controls/ButtonQuit");
            var buttonTextureWin = _content.Load<Texture2D>("Controls/WinButton");

            var newGameButton = new Button(buttonTexturePlay)
            {
                Position = new Vector2(260, 170),
            };

            newGameButton.Click += NewGameButton_Click;

            var quitGameButton = new Button(buttonTextureQuit)
            {
                Position = new Vector2(245, 260),
            };

            quitGameButton.Click += QuitGameButton_Click;

            var youWinButton = new Button(buttonTextureWin)
            {
                Position = new Vector2(180, 50),
            };

            _components = new List<Component>()
            {
                youWinButton,
                newGameButton,
                quitGameButton,
            };
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // load new game state
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var backgroundTexture = _content.Load<Texture2D>("spaceBackground");
            spriteBatch.Begin();
            spriteBatch.Draw(
                backgroundTexture,
                new Rectangle(0, 0, 800, 480),
                Color.White);
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }
    }
}
