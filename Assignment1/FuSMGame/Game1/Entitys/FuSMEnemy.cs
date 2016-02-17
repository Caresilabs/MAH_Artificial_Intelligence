using Game1.FuSM;
using Game1.Scene;
using Microsoft.Xna.Framework;
using Patrik.GameProject;
using System;

namespace Game1.Entitys
{
    public class FuSMEnemy : BaseEnemy
    {
        public const float APPROACH_DISTANCE = Tile.SIZE * 20;
        public const float APPROACH_STOP_DISTANCE = Tile.SIZE * 2f;

        public const float FOREWARN_DISTANCE = Tile.SIZE * 8;
        public const float FOREWARN_STOP_DISTANCE = Tile.SIZE * 4;

        public const float SHOOT_DISTANCE = Tile.SIZE * 5.5f;

        public const float TIME_BEFORE_PATROLLING = 4;

        public const float DEFAULT_SPEED = 200;

        public FuSMMachine<FuSMEnemy> Machine { get; private set; }

        public FuSMEnemy(Vector2 position, SimulationWorld world) : base(Globals.player, position, 100, 40, world)
        {
            this.Machine = new FuSMMachine<FuSMEnemy>(this)
                .AddState(new ApproachState())
                .AddState(new AttackState())
                .AddState(new ForewarnState())
                .AddState(new PatrolState());

            health = 150;
            maxHealth = 150;
            color = Color.Cyan;
            weapon = new Rifle(world, this);
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

            // Look forward
            if (!Machine.GetPerception<bool>("CanSeePlayer"))
            {
                rotation = (float)Math.Atan2(direction.Y, direction.X);
            }

        }

        public void UpdateAIMovement()
        {
            UpdateAIMovement(World.Player.Position);
        }

        public void UpdateAIMovement(Vector2 trgt)
        {
            if (target != Vector2.Zero && (target - position).Length() > 2)
            {
                float range = (target - position).Length();
                direction = (target - position);
                direction.Normalize();
            }
            else
            {
                RebuildPath(trgt);
            }
        }

        public void UpdatePerceptions(float delta)
        {
            Machine.SetPerception("CanSeePlayer", World.RayCast(this, World.Player));

            float dstToPlayer = (World.Player.GetPosition() - position).Length();
            Machine.SetPerception("IsNearPlayer", dstToPlayer < APPROACH_DISTANCE && Machine.GetPerception<bool>("CanSeePlayer"));

            Machine.SetPerception("DistanceToPlayer", dstToPlayer);
        }
    }
}
