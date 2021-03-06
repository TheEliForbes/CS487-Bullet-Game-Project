﻿using ExampleGame.Entities;
using ExampleGame.Entities.BulletTypes;
using ExampleGame.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;
using System.Collections.Generic;
using ExampleGame.Movements;

namespace ExampleGame.PlayerFolder
{
    class Player : Entity
    {
        private double speed;
        private double originalSpeed;
        private double slowModeModifier;
        public bool isGod;
        public List<Bullets> bullets; //may depend on design
        public BulletFactory factory;
        ContentManager Content;
        private int winner = 0;
        private int lives = 0;
        private Vector2 initPos;
        public bool invincible = false;
        private static Timer invincibilityTimer;
        private Vector2 velocity = new Vector2(-1, 1);

        //Key mapping
        Keys upKey = Keys.Up;
        Keys downKey = Keys.Down;
        Keys leftKey = Keys.Left;
        Keys rightKey = Keys.Right;
        Keys shootKey = Keys.Space;
        Keys slowMode = Keys.S;
        Keys godMode = Keys.G;

        Keys win = Keys.W;
        Keys die = Keys.D;
        Keys hit = Keys.H; //for life testing
        
        KeyboardState pastKey; //2nd most recent key command

        public Player(ContentManager gameContent)
        {
            Content = gameContent;
            isGod = false;
            bullets = new List<Bullets>();
            factory = new BulletFactory(gameContent);
        }

        public void bulletsUpdateAndCleanup(GameTime gameTime)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);

                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Initialize(float initSpeed, Vector2 initPosition)
        {
            initPos = initPosition;
            originalSpeed = speed = initSpeed;
            position = initPosition;
            slowModeModifier = 4.0;
            lives = 3;
        }
        public void Load(Texture2D initTexture)
        {
            texture = initTexture;
        }
        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(slowMode))
                speed = (speed == originalSpeed) ? speed / slowModeModifier : speed * slowModeModifier;

            if (kstate.IsKeyDown(godMode) && pastKey.IsKeyUp(godMode))
            {
                isGod = !isGod;
            }

            // for testing the win states
            if (kstate.IsKeyDown(win) && pastKey.IsKeyUp(win))
                winner = 1; // win -> see win screen

            //for testing lose life
            if (kstate.IsKeyDown(hit) && pastKey.IsKeyUp(hit))
            {
                if (invincible == false)
                {
                    takeHit(); //player is hit   
                }
            }

            if (kstate.IsKeyDown(upKey))
                position = new MoveUp(speed).getNewPosition(position,velocity);
            if (kstate.IsKeyDown(downKey))
                position = new MoveDown(speed).getNewPosition(position, velocity);
            if (kstate.IsKeyDown(leftKey))
                position = new MoveLeft(speed).getNewPosition(position, velocity);
            if (kstate.IsKeyDown(rightKey))
                position = new MoveRight(speed).getNewPosition(position, velocity);
            if (kstate.IsKeyDown(shootKey) && pastKey.IsKeyUp(shootKey))
            {
                shoot();
            }
            pastKey = Keyboard.GetState();

            bulletsUpdateAndCleanup(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                position,
                null,
                Color.White,
                0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);

            foreach (Bullets bullet in bullets)
                bullet.Draw(spriteBatch);
        }
        public void shoot()
        {
            if (!isGod)
            {
                Bullets newBullet = factory.bulletFactory("bullet", position, new Vector2(0, -10), true, 1);
                newBullet.position = position; //allow setting through constructor?

                if (bullets.Count < 20)
                    bullets.Add(newBullet);
            }
            else
            {
                //BulletSpread spread = (BulletSpread)factory.bulletFactory("spread", position, Vector2.Zero, true, 5, -1);
                BulletSpread spread = (BulletSpread)factory.bulletFactory("spread", position, Vector2.Zero, true, 5);
                foreach (Bullets bullet in spread.bullets)
                {
                    if (bullets.Count < 80)
                    {
                        bullets.Add(bullet);
                    }
                }
            }
        }

        public void boundsCheck(GraphicsDeviceManager graphics)
        {
            //----------------v This MathHelper.Min(...) blob is essentially collision detection?
            position.X = MathHelper.Min(MathHelper.Max(texture.Width / 2, position.X), graphics.PreferredBackBufferWidth - texture.Width / 2);
            position.Y = MathHelper.Min(MathHelper.Max(texture.Height / 2, position.Y), graphics.PreferredBackBufferHeight - texture.Height / 2);
        }

        //call when the player is hit by a bullet
        public void takeHit()
        {
            movePositionToInitPos(); //move player to initPos
            loseLife(); //lose a life update texture for lives
            startInvincibility(); //5 seconds of invincibility
        }

        private void startInvincibility()
        {
            texture = Content.Load<Texture2D>("playerShield");
            invincible = true;
            invincibilityTimer = new System.Timers.Timer(5000);
            invincibilityTimer.Elapsed += setInvincibilityFalse;
            invincibilityTimer.Enabled = true;
            invincibilityTimer.AutoReset = false;
        }
        private void setInvincibilityFalse(Object source, ElapsedEventArgs e)
        {
            invincible = false;
            texture = Content.Load<Texture2D>("player");
        }

        private void movePositionToInitPos()
        {
            position = initPos;
        }
        public void removeBullets()
        {
            bullets.Clear();
        }

        private void loseLife()
        {
            lives -= 1;
        }

        public void AddLife(int number)
        {
            lives += number;
        }

        public int IsWinner()
        {
            return winner;
        }
        
        public int getLives()
        {
            return lives;
        }
    }
}
