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
            CircleShape circleShape = new CircleShape(6f, 7);
            float mass = 1;
            Body = new Body(new PhysicsState(), circleShape, mass, coffecients, new Lifespan());
            Body.IsCollidable = true;
            Body.Collided += new System.EventHandler<CollisionEventArgs>(Body_Collided);
            Origin.X = 14;
            Origin.Y = 15;
        }

        void Body_Collided(object sender, CollisionEventArgs e)
        {
            Body gotHit = null;
            if (e.Contact.Body1 == Body)
            {
                gotHit = e.Contact.Body2;
            }
            else
            {
                gotHit = e.Contact.Body1;
            }
            Kill();
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
        public Vector2 Origin;
        public float Angle;
        public void Update()
        {
            Angle += 0.05f;
            if (Angle > 360)
            {
                Angle -= 360;
            }
            if (fire)
            {
                Body.ApplyImpulse(new Vector2D(0, 100));
                fire = false;
            }
        }
        public void Kill()
        {
            Body.Lifetime.IsExpired = true;
            Body.State.Position.Linear.Y = 1001;
        }
    }
}
