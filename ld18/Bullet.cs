/* All Rights Reserved. Copyright 2010 Philip Ludington */
using Microsoft.Xna.Framework;
using Physics2DDotNet.Shapes;
using Physics2DDotNet;
using AdvanceMath;

namespace LD18
{
    public class Bullet
    {
        bool fire = true;
        public Bullet()
        {
            Coefficients coffecients = new Coefficients(/*restitution*/1, /*friction*/.5f);
            CircleShape circleShape = new CircleShape(8, 7);
            float mass = 1;
            Body = new Body(new PhysicsState(), circleShape, mass, coffecients, new Lifespan());
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
        public Body Body;
        public void Update()
        {
            if (fire)
            {
                Body.ApplyImpulse(new Vector2D(0, 100));
                fire = false;
            }
        }
    }
}
