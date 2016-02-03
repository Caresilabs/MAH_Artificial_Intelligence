using Game1.FuSM;
using Game1.Scene;
using Microsoft.Xna.Framework;
using Patrik.GameProject;
using System;

namespace Game1.Entitys
{
    public class FuSMEnemy : BaseEnemy
    {
        private FuSMMachine<FuSMEnemy> Machine { get; set; }

        public FuSMEnemy(Vector2 position, SimulationWorld world) : base(Globals.player, position, 100, 40, world)
        {
            health = 150;
            maxHealth = 150;
            color = Color.Cyan;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

        }

        protected override void UpdateAI(float delta)
        {
            Machine.Update(delta);

            if (world.RayCast(this, world.Player))
            {
                Face(world.Player.GetPosition());
                Fire();

                float dst = (world.Player.GetPosition() - position).Length();
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
                RebuildPath();
            }
        }

        public void UpdatePerceptions(float dt)
        {


        }
    }
}
