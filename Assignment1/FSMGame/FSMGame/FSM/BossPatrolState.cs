using MAH_Platformer.Entities;
using MAH_Platformer.Levels.Blocks;
using System;

namespace FSMGame.FSM
{
    class BossPatrolState : FiniteStateMachine<BossEntity>.State
    {
        private float myStateTime = 0;

        public override Type CheckState()
        {
            // If he's near the player
            if ((Entity.Level.GetPlayer().GetPosition() - Entity.GetPosition()).Length() < Block.BLOCK_SIZE * 11)
            {
                return typeof(BossPlayerShootState);
            }

            return typeof(BossPatrolState);
        }

        public override void Enter()
        {
            StateTime = myStateTime;
        }

        public override void Update(float delta)
        {
            if (StateTime % 8 < 4)
            {
                Entity.SetVelocity(-BossEntity.DEFAULT_SPEED, Entity.GetVelocity().Y);
            }
            else
            {
                Entity.SetVelocity(BossEntity.DEFAULT_SPEED, Entity.GetVelocity().Y);
            }
        }

        public override void Exit()
        {
            myStateTime = StateTime;
        }
    }
}
