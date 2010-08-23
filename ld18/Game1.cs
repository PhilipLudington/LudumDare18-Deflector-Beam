/* All Rights Reserved. Copyright 2010 Philip Ludington */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Physics2DDotNet;
using AdvanceMath;
using Physics2DDotNet.Joints;
using System.Timers;

namespace LD18
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        bool gameOver = false;
        bool youWin = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //bool drawBeam2 = false;
        Texture2D texture2DGameOver;
        Texture2D texture2DYouWin;
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
        //DateTime stepTime = DateTime.Now;
        //TimeSpan animationTime = new TimeSpan(0, 0, 0, 0, 400);
        //Vector2 beamVector = new Vector2(395, 480);
        Beam beam = null;
        float beamAngle = Microsoft.Xna.Framework.MathHelper.ToRadians(45);
        float beamAngleSpin = Microsoft.Xna.Framework.MathHelper.ToRadians(0);
        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();
        public static Random random = new Random(DateTime.Now.Millisecond);
        PhysicsEngine engine = new PhysicsEngine();
        Player player = null;
        bool showCollisionRectangles = false;
        bool[] waves;
        DateTime enemyTimer;

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
            // Physics Simulation
            engine.BroadPhase = new Physics2DDotNet.Detectors.SelectiveSweepDetector();
            engine.Solver = new Physics2DDotNet.Solvers.SequentialImpulsesSolver();

            // Setup and play
            Restart();

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

            texture2DYouWin = Content.Load<Texture2D>("YouWin");
            texture2DBackground = Content.Load<Texture2D>("Background");
            texture2DHUD = Content.Load<Texture2D>("HUD");
            texture2DGameOver = Content.Load<Texture2D>("GameOver");

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

            if (player.Health <= 0 || gameOver)
            {
                gameOver = true;

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    Restart();
                }
            }
            else if (youWin)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    Restart();
                }
            }
            else
            {
                // Apply momentum to the spindle
                if (keyboardState.IsKeyDown(Keys.C))
                {
                    //beamAngleSpin += Math.Abs(beamAngleSpin) * .2f + .001f;
                    //beam.Body.State.Position.Angular += Math.Abs(beam.Body.State.Position.Angular) * .2f + .001f;
                    beam.Body.ApplyTorque(1000000f);
                }
                else if (keyboardState.IsKeyDown(Keys.Z))
                {
                    //beamAngleSpin -= Math.Abs(beamAngleSpin) * .2f + .001f;
                    //beam.Body.State.Position.Angular -= Math.Abs(beam.Body.State.Position.Angular) * .2f + .001f;
                    beam.Body.ApplyTorque(-1000000f);
                }
                //else
                //{
                //    // Friction on track bean spindle
                //    if (beamAngleSpin > 0)
                //    {
                //        beamAngleSpin -= Math.Abs(beamAngleSpin) * .015f;
                //    }
                //    else
                //    {
                //        beamAngleSpin += Math.Abs(beamAngleSpin) * .015f;
                //    }
                //}

                // The momentum moves the spindle
                beamAngle += beamAngleSpin;

                //if (stepTime < DateTime.Now)
                //{
                //    if (texture2DBeam == texture2DBeam1)
                //    {
                //        drawBeam2 = true;
                //        texture2DBeam = texture2DBeam2;
                //    }
                //    else
                //    {
                //        drawBeam2 = false;
                //        texture2DBeam = texture2DBeam1;
                //    }
                //    stepTime = DateTime.Now.Add(animationTime);
                //}

                // Is it time for a new wave?
                TimeSpan enemyTimerSpan = DateTime.Now - enemyTimer;
                if (waves[1] == false && enemyTimerSpan > new TimeSpan(0, 0, 1))
                {
                    Wave1();
                }
                else if (waves[2] == false && enemyTimerSpan > new TimeSpan(0, 0, 5))
                {
                    Wave2();
                }
                else if (waves[3] == false && enemyTimerSpan > new TimeSpan(0, 0, 10))
                {
                    Wave3();
                }
                else if (waves[4] == false && enemyTimerSpan > new TimeSpan(0, 0, 13))
                {
                    Wave4();
                }
                else if (waves[5] == false && enemyTimerSpan > new TimeSpan(0, 0, 14))
                {
                    Wave5();
                }
                else if (waves[5] == true && enemyTimerSpan > new TimeSpan(0, 0, 35))
                {
                    youWin = true;
                }

                // Enemies
                List<Enemy> deadEnemies = new List<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    enemy.Update();
                    if (enemy.Position.Y > 1000)
                    {
                        enemy.Kill();
                        deadEnemies.Add(enemy);
                    }
                }
                foreach (Enemy enemy in deadEnemies)
                {
                    enemies.Remove(enemy);
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

            // Background
            spriteBatch.Draw(texture2DBackground, new Vector2(0, 0), Color.White);

            // Tracker Beam
            spriteBatch.Draw(texture2DBeam3, beam.Position, null, Color.White, beam.Angle, beam.Origin, 1, SpriteEffects.None, 1);
            spriteBatch.Draw(texture2DBeam, beam.Position, null, Color.White, beam.Angle, beam.Origin, 1, SpriteEffects.None, 1);

            // Player's Ship
            spriteBatch.Draw(texture2DMotherShip, player.Position, null, Color.White, player.Angle, player.Origin, 1, SpriteEffects.None, 1);
            //if (drawBeam2)
            //{
            //    spriteBatch.Draw(texture2DMotherShip2, player.Position, null, Color.White, 0, player.Origin, 1, SpriteEffects.None, 1);
            //}

            // Enemies
            foreach (Enemy enemy in enemies)
            {
                spriteBatch.Draw(texture2DEnemy1, enemy.Position, null, Color.White, enemy.Angle, enemy.Origin, 1f, SpriteEffects.None, 1);
            }

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

            if (gameOver)
            {
                spriteBatch.Draw(texture2DGameOver, new Vector2(0, 0), Color.White);
            }
            else if (youWin)
            {
                spriteBatch.Draw(texture2DYouWin, new Vector2(0, 0), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void AddBullet(float x, float y)
        {
            AddBullet((int)x, (int)y, 0);
        }
        public void AddBullet(float x, float y, float angle)
        {
            AddBullet((int)x, (int)y, (int)angle);
        }
        public void AddBullet(int x, int y)
        {
            AddBullet(x, y, 0);
        }
        public void AddBullet(int x, int y, int angle)
        {
            Bullet bullet = new Bullet();
            bullet.Position = new Vector2(x, y);
            bullet.Body.State.Position.Angular = angle;
            engine.AddBody(bullet.Body);
            bullets.Add(bullet);
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

        private Enemy NewEnemy(int x, int y, int angle)
        {
            Enemy enemy = new Enemy(this, x, y);
            enemy.Angle = angle;
            engine.AddBody(enemy.Body);
            enemies.Add(enemy);

            return enemy;
        }
        private Enemy NewEnemy(int x, int y)
        {
            return NewEnemy(x, y, 0);
        }
        private void Restart()
        {
            engine.Clear();
            bullets.Clear();
            enemies.Clear();

            player = new Player();
            player.Position = new Vector2(400, 525);
            engine.AddBody(player.Body);
            player.Body.ApplyImpulse(new Vector2D(0, 10));

            beam = new Beam(this, 395, 480);
            engine.AddBody(beam.Body);
            FixedHingeJoint fixedHingeJoint = new FixedHingeJoint(beam.Body, new Vector2D(395, 480), new Lifespan());
            engine.AddJoint(fixedHingeJoint);

            waves = new bool[] { false, false, false, false, false, false };

            gameOver = false;
            youWin = false;
            enemyTimer = DateTime.Now;
        }

        private void Wave1()
        {
            NewEnemy(325, 100, 0);
            waves[1] = true;
        }
        private void Wave2()
        {
            NewEnemy(325, 100);

            NewEnemy(425, 100);

            waves[2] = true;
        }
        private void Wave3()
        {
            NewEnemy(325, 100);

            NewEnemy(425, 100);

            waves[3] = true;
        }
        private void Wave4()
        {
            NewEnemy(325, 100);

            NewEnemy(425, 100);

            NewEnemy(100, 300, 0).Body.ApplyImpulse(new Vector2D(50, 0));

            waves[4] = true;
        }
        private void Wave5()
        {
            NewEnemy(100, 200, 0).Body.ApplyImpulse(new Vector2D(25, 0));

            waves[5] = true;
        }

    }
}
