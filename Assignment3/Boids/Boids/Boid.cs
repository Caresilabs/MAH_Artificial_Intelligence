using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    public class Boid
    {
        public const int MAX_SPEED = 100;

        public const float SEPARATION_WEIGHT = 12.5f;
        public const float COHESION_WEIGHT = 2;
        public const float ALIGNMENT_WEIGHT = 2f;

        public const float RADIUS = 11;

        public Texture2D Texture { get; private set; }
        public World World { get; private set; }

        public Vector2 Position { get { return position; } }
        public Vector2 Velocity { get { return velocity; } }

        public int NeighborCount { get; private set; }

        private Vector2 velocity;
        private Vector2 position;

        public Boid(World world, Texture2D texture)
        {
            this.World = world;
            this.Texture = texture;
            this.position = new Vector2((float)BoidsScreen.RANDOM.NextDouble() * BoidsScreen.WIDTH, (float)BoidsScreen.RANDOM.NextDouble() * BoidsScreen.HEIGHT);
            this.velocity = new Vector2(BoidsScreen.RANDOM.Next(-MAX_SPEED, MAX_SPEED), BoidsScreen.RANDOM.Next(-MAX_SPEED, MAX_SPEED));
        }

        public void Update(float delta)
        {
            // Cap speed
            //if (velocity.Length() > MAX_SPEED)
            {
                //CapSpeed();
            }

            // Add position
            position += velocity * delta;

            // Wrap screen
            position.X = (position.X + BoidsScreen.WIDTH) % BoidsScreen.WIDTH;
            position.Y = (position.Y + BoidsScreen.HEIGHT) % BoidsScreen.HEIGHT;

            var alignment = CalcAlignment();
            var cohesion = CalcCohesion();
            var separation = CalcSeparation();

            var flock = (alignment * ALIGNMENT_WEIGHT) + (cohesion * COHESION_WEIGHT) + (separation * SEPARATION_WEIGHT);
            if (flock != Vector2.Zero)
            {
                velocity += flock;
                CapSpeed();
            }
        }

        private void CapSpeed()
        {
            if (velocity == Vector2.Zero || velocity.Length() <= MAX_SPEED)
                return;

            velocity.Normalize();
            velocity *= MAX_SPEED;
        }

        private void MaxSpeed()
        {
            velocity.Normalize();
            velocity *= MAX_SPEED;
        }

        public void Wander(Vector2 pos)
        {
            var desiredVelocity = (pos - position);
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            velocity += (desiredVelocity - velocity) * 0.1f;
            MaxSpeed();
        }

        public void Pursuit(Vector2 pos, Vector2 vel)
        {
            var dst = pos - position;
            float time = dst.Length() / MAX_SPEED;
            var futurePosition = pos + time * velocity;
            Seek(futurePosition);
            MaxSpeed();
        }

        public void Arrive(Vector2 pos, float arriveRadius)
        {
            var desiredVelocity = pos - position;
            var distance = desiredVelocity.Length();

            if (distance < arriveRadius)
            {
                desiredVelocity.Normalize();
                desiredVelocity = desiredVelocity * MAX_SPEED * (distance / arriveRadius);
            }
            else
            {
                desiredVelocity.Normalize();
                desiredVelocity = desiredVelocity * MAX_SPEED;
            }

            velocity += desiredVelocity - velocity;
        }

        public void Flee(Vector2 pos)
        {
            var desiredVelocity = (position - pos);
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            velocity += (desiredVelocity - velocity) * 0.2f;
            MaxSpeed();
        }

        public void Seek(Vector2 pos)
        {
            var desiredVelocity = (pos - position);
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            velocity += (desiredVelocity - velocity) * 0.14f;
            MaxSpeed();
        }

        public Vector2 CalcAlignment()
        {
            NeighborCount = 0;

            Vector2 v = new Vector2();

            foreach (var boid in World.Grid.GetPossibleColliders(this))
            {
                if (boid == this)
                    continue;

                if (Distance(position, boid.position) < 120)
                {
                    v += boid.velocity;
                    ++NeighborCount;
                }
            }

            if (NeighborCount == 0)
                return v;

            v /= NeighborCount;
            v.Normalize();

            return v;
        }

        public Vector2 CalcCohesion()
        {
            NeighborCount = 0;

            Vector2 v = new Vector2();

            foreach (var boid in World.Grid.GetPossibleColliders(this))
            {
                if (boid == this)
                    continue;

                if (Distance(position, boid.position) < 120)
                {
                    v += boid.position;
                    ++NeighborCount;
                }
            }

            if (NeighborCount == 0)
                return v;

            v /= NeighborCount;
            v = new Vector2(v.X - position.X, v.Y - position.Y);
            v.Normalize();

            return v;
        }

        public Vector2 CalcSeparation()
        {
            NeighborCount = 0;

            Vector2 v = new Vector2();

            foreach (var boid in World.Grid.GetPossibleColliders(this))
            {
                if (boid == this)
                    continue;

                if (Distance(position, boid.position) < 40)
                {
                    v += boid.position - position;
                    ++NeighborCount;
                }
            }

            if (NeighborCount == 0)
                return v;

            v /= NeighborCount;
            // v = new Vector2(v.X - position.X, v.Y - position.Y);
            v *= -1;
            v.Normalize();

            return v;
        }

        public float Distance(Vector2 v1, Vector2 v2)
        {
            return (v1 - v2).Length();
        }
    }
}
