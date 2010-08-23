/* All Rights Reserved. Copyright 2010 Philip Ludington */
using Microsoft.Xna.Framework;
using Physics2DDotNet.Shapes;
using Physics2DDotNet;
using AdvanceMath;
using Physics2DDotNet.Ignorers;

namespace LD18
{
    public class Bullet
    {
        bool fire = true;
        private float angleStep = Game1.random.Next(0, 17) * 0.02f;
        public Bullet()
        {
            Coefficients coffecients = new Coefficients(/*restitution*/1, /*friction*/.5f);
            CircleShape circleShape = new CircleShape(6f, 7);
            float mass = 1f;
            Body = new Body(new PhysicsState(), circleShape, mass, coffecients, new Lifespan());
            Body.IsCollidable = true;
            Body.CollisionIgnorer = new BulletIgnorer();
            Body.Tag = "BulletTag";
            Body.Collided += new System.EventHandler<CollisionEventArgs>(Body_Collided);
            Origin.X = 14;
            Origin.Y = 15;
        }

        void Body_Collided(object sender, CollisionEventArgs e)
        {
            Body gotHitBy = null;
            if (e.Contact.Body1 == Body)
            {
                gotHitBy = e.Contact.Body2;
            }
            else
            {
                gotHitBy = e.Contact.Body1;
            }
            if (gotHitBy.Tag == (object)"BeamTag")
            {
                Body.ApplyImpulse(new Vector2D(10, 10));
            }
            else
            {
                Kill();
            }
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
            Angle += angleStep;
            if (Angle > 360)
            {
                Angle -= 360;
            }
            if (fire)
            {
                Vector2D vector = new Vector2D(0, 100);
                //float angle = Body.State.Position.Angular;
                //vector.Angle = angle;
                Body.ApplyImpulse(vector);
                fire = false;
            }
        }
        public void Kill()
        {
            Body.Lifetime.IsExpired = true;
            Body.State.Position.Linear.Y = 1001;
        }
    }

    public class BulletIgnorer : Ignorer
    {
        public override bool BothNeeded
        {
            get { return false; }
        }

        protected override bool CanCollide(Body thisBody, Body otherBody, Ignorer other)
        {
            if (otherBody.Tag == (object)"BulletTag")
            {
                return false;
            }

            return true;
        }
    }
}
