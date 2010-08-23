/* All Rights Reserved. Copyright 2010 Philip Ludington */
using Physics2DDotNet.Shapes;
using Physics2DDotNet;
using AdvanceMath;
using Microsoft.Xna.Framework;
using Physics2DDotNet.Ignorers;

namespace LD18
{
    public class Player
    {
        public Vector2 Origin;
        public float Angle;
        public Body Body;
        public int Health;

        public Player()
        {
            Coefficients coffecients = new Coefficients(/*restitution*/1, /*friction*/.5f);
            CircleShape circleShape = new CircleShape(80, 9);
            float mass = 10000;
            Body = new Body(new PhysicsState(), circleShape, mass, coffecients, new Lifespan());
            Body.IsCollidable = true;
            Body.CollisionIgnorer = new PlayerIgnorer();
            Body.Tag = "PlayerTag";
            Body.Collided += new System.EventHandler<CollisionEventArgs>(Body_Collided);
            Origin.X = 73;
            Origin.Y = 73;
            Health = 1000;
        }

        void Body_Collided(object sender, CollisionEventArgs e)
        {
            Health -= 10;
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(Body.State.Position.Linear.X, Body.State.Position.Linear.Y);
            }
            set
            {
                Body.State.Position = new ALVector2D(0, value.X, value.Y);
            }
        }
        public void Update()
        {
            Angle += 0.05f;
            if (Angle > 360)
            {
                Angle -= 360;
            }
        }

        public class PlayerIgnorer : Ignorer
        {
            public override bool BothNeeded
            {
                get { return false; }
            }

            protected override bool CanCollide(Body thisBody, Body otherBody, Ignorer other)
            {
                if (otherBody.Tag == (object)"BeamTag")
                {
                    return false;
                }

                return true;
            }
        }
    }
}
