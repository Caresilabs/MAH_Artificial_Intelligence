using Game1.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Patrik.GameProject;
using System;

namespace Game1.Entitys
{
    public class BaseEnemy : Entity
    {
        public static readonly float ENEMY_STOP_RANGE = Tile.SIZE * 2.1f;

        protected Vector2 target;

        public BaseEnemy(Texture2D texture, Vector2 position, float speed, int size, SimulationWorld world) : base(texture, position, speed, size, world)
        {
            var rand = new Random();
            switch(rand.Next(0, 4))
            {
                case 0: weapon = new ShotGun(world, this);
                    break;
                case 1: weapon = new Bazooka(world, this);
                    break;
                case 2: weapon = new Rifle(world, this);
                    break;
                default: weapon = new Pistol(world, this);
                    break;
            }
        }

        public override bool Blocks(GameObject other)
        {
            return base.Blocks(other);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            UpdateAI(delta);
        }

        protected virtual void UpdateAI(float delta)
        {
            if (World.RayCast(this, World.Player))
            {
                Face(World.Player.GetPosition());
                Fire();

                float dst = (World.Player.GetPosition() - position).Length();
                if (dst < ENEMY_STOP_RANGE)
                {
                    target = Vector2.Zero;
                    direction = Vector2.Zero;
                    return;
                }
            }
            else
            {
                rotation = (float)Math.Atan2(direction.Y, direction.X);
            }

            if (target != Vector2.Zero && (target - position).Length() > 2)
            {
                float range = (target - position).Length();
                //position = Vector2.Lerp(position, target, delta * speed / range);
                direction = (target - position);
                direction.Normalize();

            }
            else
            {
                RebuildPath(World.Player.Position);
            }
        }

        protected void RebuildPath(Vector2 trgt)
        {//World.Player.GetPosition()
            var path = World.PathFinder.Pathfind(((position) / Tile.SIZE).ToPoint(), ((trgt) / Tile.SIZE).ToPoint()); //- new Vector2(Tile.SIZE/2f) , new Vector2(Tile.SIZE / 2f)
            if (path.Count > 1)
                target = path[1].ToVector2() * Tile.SIZE + new Vector2(Tile.SIZE / 2f);
        }
    }
}
