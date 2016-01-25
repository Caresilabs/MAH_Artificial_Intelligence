using MAH_Platformer.Entities;
using MAH_Platformer.Levels.Blocks;
using System;

namespace FSMGame.FSM
{
    class BossCircleShootState : FiniteStateMachine<BossEntity>.State
    {
        private float shootTime;

        public override Type CheckState()
        {
            // If he's not near the player
            if ((Entity.Level.GetPlayer().GetPosition() - Entity.GetPosition()).Length() > Block.BLOCK_SIZE * 15)
            {
                return typeof(BossPatrolState);
            }

            if (StateTime > 6)
            {
                return typeof(BossPlayerShootState);
            }

            return typeof(BossCircleShootState);
        }

        public override void Enter()
        {
        }

        public override void Update(float delta)
        {
            shootTime += delta;

            if (shootTime > 1.8f)
            {
                Entity.ShootCircle();
                shootTime = 0;
            }
        }

        public override void Exit()
        {
        }
    }
}
