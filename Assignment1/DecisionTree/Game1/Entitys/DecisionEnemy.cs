using Game1.FuSM;
using Game1.Scene;
using Microsoft.Xna.Framework;
using Patrik.GameProject;
using System;

namespace Game1.Entitys
{
    public class DecisionEnemy : BaseEnemy
    {
        public const float APPROACH_DISTANCE = Tile.SIZE * 20;
        public const float APPROACH_STOP_DISTANCE = Tile.SIZE * 2f;

        public const float FOREWARN_DISTANCE = Tile.SIZE * 8;
        public const float FOREWARN_STOP_DISTANCE = Tile.SIZE * 4;

        public const float SHOOT_DISTANCE = Tile.SIZE * 5.5f;

        public const float TIME_BEFORE_PATROLLING = 4;

        public const float DEFAULT_SPEED = 200;

        // Perceptions
        private bool CanSeePlayer;

        public DecisionEnemy(Vector2 position, SimulationWorld world) : base(Globals.player, position, 100, 40, world)
        {
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

            UpdateDecisions(delta);
        }

        private void UpdateDecisions(float delta)
        {
            if (CanSeePlayer)
            {

            }
            else
            {

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
            CanSeePlayer = World.RayCast(this, World.Player);

            //float dstToPlayer = (World.Player.GetPosition() - position).Length();
            //Machine.SetPerception("IsNearPlayer", dstToPlayer < APPROACH_DISTANCE && Machine.GetPerception<bool>("CanSeePlayer"));

            //Machine.SetPerception("DistanceToPlayer", dstToPlayer);
        }
    }
}
