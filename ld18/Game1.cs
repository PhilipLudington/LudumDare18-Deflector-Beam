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
        DateTime stepTime = DateTime.Now;
        TimeSpan animationTime = new TimeSpan(0, 0, 0, 0, 400);

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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texture2DMotherShip1 = texture2DMotherShip = Content.Load<Texture2D>("MotherShip1");
            texture2DMotherShip2 = Content.Load<Texture2D>("MotherShip2");
            texture2DBeam1 = texture2DBeam = Content.Load<Texture2D>("Beam1");
            texture2DBeam2 = Content.Load<Texture2D>("Beam2");
            texture2DBeam3 = Content.Load<Texture2D>("Beam3");
            texture2DEnemy1 = Content.Load<Texture2D>("Enemy1");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

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
            spriteBatch.Draw(texture2DBeam3, new Vector2(350, 200), Color.White);
            spriteBatch.Draw(texture2DBeam1, new Vector2(400, 200), Color.White);
            spriteBatch.Draw(texture2DMotherShip, new Vector2(350, 400), Color.White);
            if (drawBeam2)
            {
                spriteBatch.Draw(texture2DMotherShip2, new Vector2(350, 400), Color.White);
            }
            spriteBatch.Draw(texture2DEnemy1, new Vector2(50, 40), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
