using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boids
{
    public class Boid
    {
        public const int MAX_SPEED = 100;
        //public const int RADIUS = 32 * 3;

        public const float SEPARATION_WEIGHT = 14.5f;
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
            if (velocity.Length() > MAX_SPEED)
            {
                velocity.Normalize();
                velocity *= MAX_SPEED;
            }

            // Add position
            position += velocity * delta;

            // Wrap screen
            position.X = (position.X + BoidsScreen.WIDTH) % BoidsScreen.WIDTH;
            position.Y = (position.Y + BoidsScreen.HEIGHT) % BoidsScreen.HEIGHT;

            var alignment = CalcAlignment();
            var cohesion = CalcCohesion();
            var separation = CalcSeparation();

            velocity += (alignment * ALIGNMENT_WEIGHT) + (cohesion * COHESION_WEIGHT) + (separation * SEPARATION_WEIGHT);
        }

        //      public function pursuit(t :Boid) :Vector3D {
        //var distance :Vector3D = t.position - position;
        //var T :int = distance.length / MAX_VELOCITY;
        //futurePosition :Vector3D = t.position + t.velocity* T;
        //return seek(futurePosition);
        //  }

        public void Pursuit(Vector2 pos)
        {
            var dst = pos - position;
            //float dst.Length() / MAX_SPEED;
            var futurePosition = pos;
            Seek(futurePosition);
        }

       
        public void Wander(Vector2 pos, float radius)
        {
           
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
            velocity += desiredVelocity - velocity;
        }

        public void Seek(Vector2 pos)
        {
            var desiredVelocity = (pos - position);
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            velocity += desiredVelocity - velocity;
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
