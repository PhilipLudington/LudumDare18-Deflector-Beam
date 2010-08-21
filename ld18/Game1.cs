/* All Rights Reserved. Copyright 2010 Philip Ludington */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace LD18
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool drawBeam2 = false;
        Texture2D texture2DBeam;
        Texture2D texture2DBeam1;
        Texture2D texture2DBeam2;
        Texture2D texture2DBeam3;
        Texture2D texture2DMotherShip;
        Texture2D texture2DMotherShip1;
        Texture2D texture2DMotherShip2;
        Texture2D texture2DEnemy1;
        Texture2D texture2DBullet1;
        DateTime stepTime = DateTime.Now;
        TimeSpan animationTime = new TimeSpan(0, 0, 0, 0, 400);
        Vector2 beamVector = new Vector2(422, 410);
        float beamAngle = MathHelper.ToRadians(45);
        float beamAngleSpin = MathHelper.ToRadians(0);
        List<Bullet> bullets = new List<Bullet>();
        Random random = new Random(DateTime.Now.Millisecond);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            int randomBullets = random.Next(0, 50000);
            Bullet bullet;
            for (int x = 0; x < randomBullets; x++)
            {
                bullet = new Bullet();
                bullet.Position.X = random.Next(0, 800);
                bullet.Position.Y = random.Next(0, 600);
                bullets.Add(bullet);
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Window.Title = "MrPhil's LD18 Entry";

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texture2DMotherShip1 = texture2DMotherShip = Content.Load<Texture2D>("MotherShip1");
            texture2DMotherShip2 = Content.Load<Texture2D>("MotherShip2");
            texture2DBeam1 = texture2DBeam = Content.Load<Texture2D>("Beam1");
            texture2DBeam2 = Content.Load<Texture2D>("Beam2");
            texture2DBeam3 = Content.Load<Texture2D>("Beam3");
            texture2DEnemy1 = Content.Load<Texture2D>("Enemy1");
            texture2DBullet1 = Content.Load<Texture2D>("Bullet");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // Apply momentum to the spindle
            if (keyboardState.IsKeyDown(Keys.C))
            {
                beamAngleSpin += Math.Abs(beamAngleSpin) * .2f + .001f;
            }
            else if (keyboardState.IsKeyDown(Keys.Z))
            {
                beamAngleSpin -= Math.Abs(beamAngleSpin) * .2f + .001f;
            }
            else
            {
                // Friction on track bean spindle
                if (beamAngleSpin > 0)
                {
                    beamAngleSpin -= Math.Abs(beamAngleSpin) * .015f;
                }
                else
                {
                    beamAngleSpin += Math.Abs(beamAngleSpin) * .015f;
                }
            }

            // The momentum moves the spindle
            beamAngle += beamAngleSpin;

            if (stepTime < DateTime.Now)
            {
                if (texture2DBeam == texture2DBeam1)
                {
                    drawBeam2 = true;
                    texture2DBeam = texture2DBeam2;
                }
                else
                {
                    drawBeam2 = false;
                    texture2DBeam = texture2DBeam1;
                }
                stepTime = DateTime.Now.Add(animationTime);
            }

            // Bullets
            List<Bullet> deadBullets = new List<Bullet>();
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
                if (bullet.Position.Y > 1000)
                {
                    deadBullets.Add(bullet);
                }
            }
            foreach (Bullet bullet in deadBullets)
            {
                bullets.Remove(bullet);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(texture2DBeam3, beamVector, null, Color.White, beamAngle, new Vector2(54, 274), 1, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2DBeam, beamVector, null, Color.White, beamAngle, new Vector2(54, 274), 1, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2DMotherShip, new Vector2(350, 400), Color.White);
            if (drawBeam2)
            {
                spriteBatch.Draw(texture2DMotherShip2, new Vector2(350, 400), Color.White);
            }
            spriteBatch.Draw(texture2DEnemy1, new Vector2(50, 40), Color.White);
            // Bullets
            foreach (Bullet bullet in bullets)
            {
                spriteBatch.Draw(texture2DBullet1, bullet.Position, null, Color.White, 0.0f, new Vector2(54, 274), 0.51f, SpriteEffects.None, 1);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
