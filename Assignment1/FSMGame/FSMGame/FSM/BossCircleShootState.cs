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
            if (StateTime > 10)
            {
                return typeof(BossPlayerShootState);
            } else if ((Entity.Level.GetPlayer().GetPosition() - Entity.GetPosition()).Length() > Block.BLOCK_SIZE * 11)
            {
                return typeof(BossPatrolState);
            }

            return typeof(BossCircleShootState);
        }

        public override void Enter()
        {
        }

        public override void Update(float delta)
        {
            shootTime += delta;

            if (shootTime > 3)
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
