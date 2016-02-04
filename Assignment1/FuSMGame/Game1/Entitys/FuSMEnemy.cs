using Game1.FuSM;
using Game1.Scene;
using Microsoft.Xna.Framework;
using Patrik.GameProject;
using System;

namespace Game1.Entitys
{
    public class FuSMEnemy : BaseEnemy
    {
        public const int APPROACH_DISTANCE = Tile.SIZE * 6;

        private FuSMMachine<FuSMEnemy> Machine { get; set; }

        public FuSMEnemy(Vector2 position, SimulationWorld world) : base(Globals.player, position, 100, 40, world)
        {
            this.Machine = new FuSMMachine<FuSMEnemy>(this)
                .AddState(new ApproachState())
                .AddState(new AttackState());

            health = 150;
            maxHealth = 150;
            color = Color.Cyan;
            weapon = new Pistol(world, this);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        protected override void UpdateAI(float delta)
        {
            // Update le Perceptions
            UpdatePerceptions(delta);

            Machine.Update(delta);

            if (!Machine.GetPerception<bool>("CanSeePlayer"))
            {
                rotation = (float)Math.Atan2(direction.Y, direction.X);
            }

            //if (CanSeePlayer())
            //{
            //    Face(World.Player.GetPosition());
              //Fire();

                //    float dst = (World.Player.GetPosition() - position).Length();
                //    if (dst < ENEMY_STOP_RANGE)
                //    {
                //        target = Vector2.Zero;
                //        direction = Vector2.Zero;
                //        return;
                //    }
                //}
        }

        public void UpdateAIMovement()
        {
            if (target != Vector2.Zero && (target - position).Length() > 2)
            {
                float range = (target - position).Length();
                direction = (target - position);
                direction.Normalize();
            }
            else
            {
                RebuildPath();
            }
        }

        public void UpdatePerceptions(float delta)
        {
            Machine.SetPerception("CanSeePlayer", World.RayCast(this, World.Player));

            float dstToPlayer = (World.Player.GetPosition() - position).Length();
            Machine.SetPerception("IsNearPlayer", dstToPlayer > APPROACH_DISTANCE && Machine.GetPerception<bool>("CanSeePlayer"));


        }
    }
}
