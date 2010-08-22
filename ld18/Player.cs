/* All Rights Reserved. Copyright 2010 Philip Ludington */
using Physics2DDotNet.Shapes;
using Physics2DDotNet;
using AdvanceMath;
using Microsoft.Xna.Framework;

namespace LD18
{
    public class Player
    {
        public Player()
        {
            Coefficients coffecients = new Coefficients(/*restitution*/1, /*friction*/.5f);
            CircleShape circleShape = new CircleShape(28, 7);
            float mass = 1;
            Body = new Body(new PhysicsState(), circleShape, mass, coffecients, new Lifespan());
            Body.IsCollidable = true;
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
            Body.ApplyImpulse(new Vector2D(0, 100));
        }
    }
}
