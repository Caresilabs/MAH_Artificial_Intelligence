using Microsoft.Xna.Framework;
using Simon.Mah.Framework;
using System.Collections.Generic;

namespace Boids
{
    public class World
    {
        public List<Boid> Boids { get; private set; }

        public List<Interest> Interests { get; private set; }

        public SpatialHashGrid Grid { get; private set; }

        public World()
        {
            this.Boids = new List<Boid>();
            this.Interests = new List<Interest>();
            this.Grid = new SpatialHashGrid();
            this.Grid.Setup(BoidsScreen.WIDTH, BoidsScreen.HEIGHT, BoidsScreen.WIDTH / 4f);
            SpawnBoids(150);
        }

        public void SpawnBoids(int n = 1)
        {
            for (int i = 0; i < n; i++)
            {
                Boid boid = new Boid(this, Game1.BOID_TEXTURE);
                Boids.Add(boid);
            }
        }

        public void SpawnInterest(Interest.EType type, Vector2 pos)
        {
            Interests.Add(new Interest(type, pos));
        }

        public void Update(float delta)
        {
            Grid.ClearBuckets();
            foreach (var boid in Boids)
            {
                Grid.AddObject(boid);

                if (Interests.Count != 0)
                {
                    Interest nearest = null;
                    foreach (var interest in Interests)
                    {
                        if (nearest == null || (boid.Position - nearest.Position).Length() > (boid.Position - interest.Position).Length())
                            nearest = interest;
                    }

                    if (nearest != null)
                    {
                        nearest.Affect(boid);
                    }
                }
            }

            foreach (var boid in Boids)
            {
                boid.Update(delta);
            }

            foreach (var interest in Interests)
            {
                interest.Update(delta);
            }
        }
    }
}
