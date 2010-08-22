/* All Rights Reserved. Copyright 2010 Philip Ludington */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Physics2DDotNet;
using AdvanceMath;

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
        Texture2D texture2DBackground;
        Texture2D texture2DHUD;
        Texture2D texture2DBeam;
        Texture2D texture2DBeam1;
        Texture2D texture2DBeam2;
        Texture2D texture2DBeam3;
        Texture2D texture2DMotherShip;
        Texture2D texture2DMotherShip1;
        Texture2D texture2DMotherShip2;
        Texture2D texture2DEnemy1;
        Texture2D texture2DBullet1;
        Texture2D dummyTexture;
        DateTime stepTime = DateTime.Now;
        TimeSpan animationTime = new TimeSpan(0, 0, 0, 0, 400);
        Vector2 beamVector = new Vector2(395, 480);
        float beamAngle = Microsoft.Xna.Framework.MathHelper.ToRadians(45);
        float beamAngleSpin = Microsoft.Xna.Framework.MathHelper.ToRadians(0);
        List<Bullet> bullets = new List<Bullet>();
        Random random = new Random(DateTime.Now.Millisecond);
        PhysicsEngine engine = new PhysicsEngine();
        Player player = new Player();
        bool showCollisionRectangles = true;

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
            // Pysics Simulation
            engine.BroadPhase = new Physics2DDotNet.Detectors.SelectiveSweepDetector();
            engine.Solver = new Physics2DDotNet.Solvers.SequentialImpulsesSolver();

            int randomBullets = random.Next(0, 50);
            Bullet bullet;
            for (int x = 0; x < randomBullets; x++)
            {
                //bullet = new Bullet();
                //bullet.Position = new Vector2(random.Next(0, 800), random.Next(0, 600));
                //engine.AddBody(bullet.Body);
                //bullets.Add(bullet);
            }

            // Special bullet for testing
            bullet = new Bullet();
            bullet.Position = new Vector2(375, 25);
            engine.AddBody(bullet.Body);
            bullets.Add(bullet);

            player.Position = new Vector2(400, 525);
            engine.AddBody(player.Body);
            player.Body.ApplyImpulse(new Vector2D(0, 10));

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

            texture2DBackground = Content.Load<Texture2D>("Background");
            texture2DHUD = Content.Load<Texture2D>("HUD");

            texture2DMotherShip1 = texture2DMotherShip = Content.Load<Texture2D>("MotherShip1");
            texture2DMotherShip2 = Content.Load<Texture2D>("MotherShip2");
            texture2DBeam1 = texture2DBeam = Content.Load<Texture2D>("Beam1");
            texture2DBeam2 = Content.Load<Texture2D>("Beam2");
            texture2DBeam3 = Content.Load<Texture2D>("Beam3");
            texture2DEnemy1 = Content.Load<Texture2D>("Enemy1");
            texture2DBullet1 = Content.Load<Texture2D>("Bullet");

            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            engine.Clear();
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
                    bullet.Kill();
                    deadBullets.Add(bullet);
                }
            }
            foreach (Bullet bullet in deadBullets)
            {
                bullets.Remove(bullet);
            }

            // Player
            player.Update();

            // Physic engine
            engine.Update((float)gameTime.ElapsedGameTime.TotalSeconds, (float)gameTime.ElapsedRealTime.TotalSeconds);

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

            // Background
            spriteBatch.Draw(texture2DBackground, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(texture2DBeam3, beamVector, null, Color.White, beamAngle, new Vector2(54, 274), 1, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2DBeam, beamVector, null, Color.White, beamAngle, new Vector2(54, 274), 1, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2DMotherShip, player.Position, null, Color.White, player.Angle, player.Origin, 1, SpriteEffects.None, 1);
            //if (drawBeam2)
            //{
            //    spriteBatch.Draw(texture2DMotherShip2, player.Position, null, Color.White, 0, player.Origin, 1, SpriteEffects.None, 1);
            //}
            spriteBatch.Draw(texture2DEnemy1, new Vector2(50, 40), Color.White);

            // Bullets
            foreach (Bullet bullet in bullets)
            {
                spriteBatch.Draw(texture2DBullet1, bullet.Position, null, Color.White, bullet.Angle, bullet.Origin, 0.51f, SpriteEffects.None, 1);
            }

            double top = 2 + (590 - player.Health * .59);
            Rectangle healthBar = new Rectangle(753, (int)top, 796, 592);
            spriteBatch.Draw(dummyTexture, healthBar, Color.Red);

            // Display the collision bounding boxes
            if (showCollisionRectangles)
            {
                foreach (Body body in engine.Bodies)
                {
                    Vector2D[] vertexes = body.Shape.Vertexes;
                    Vector2? previousVector = null;
                    Vector2? startVector = null;
                    foreach (Vector2D vector2D in vertexes)
                    {
                        if (previousVector == null)
                        {
                            startVector = previousVector = new Vector2(body.State.Position.Linear.X + vector2D.X, body.State.Position.Linear.Y + vector2D.Y);
                        }
                        else
                        {
                            Vector2 vector = new Vector2(body.State.Position.Linear.X + vector2D.X, body.State.Position.Linear.Y + vector2D.Y);
                            DrawLine((Vector2)previousVector, vector);
                            previousVector = vector;
                        }
                    }
                    // Draw line from start to end
                    DrawLine((Vector2)previousVector, (Vector2)startVector);
                }
            }

            // HUD
            spriteBatch.Draw(texture2DHUD, new Vector2(0, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawLine(Vector2 a, Vector2 b)
        {
            Vector2 Origin = new Vector2(0.5f, 0.0f);
            Vector2 diff = b - a;
            float angle;
            Vector2 Scale = new Vector2(1.0f, diff.Length() / dummyTexture.Height);

            angle = (float)(Math.Atan2(diff.Y, diff.X)) - Microsoft.Xna.Framework.MathHelper.PiOver2;

            spriteBatch.Draw(dummyTexture, a, null, Color.Red, angle, Origin, Scale, SpriteEffects.None, 1.0f);
        }
    }
}
