using MAH_Platformer.Entities;
using MAH_Platformer.Levels.Blocks;
using Simon.Mah.Framework.Tools;
using System;

namespace FSMGame.FSM
{
    class BossPlayerShootState : FiniteStateMachine<BossEntity>.State
    {
        public override Type CheckState()
        {

            if (Entity.Life % 20 == 0)
            {
                return typeof(BossSprayShootState);
            }

            // If he's not near the player
            if ((Entity.Level.GetPlayer().GetPosition() - Entity.GetPosition()).Length() > Block.BLOCK_SIZE * 11)
            {
                return typeof(BossPatrolState);
            }

            if (MathUtils.random() < 0.0002f)
            {
                return typeof(BossCircleShootState);
            }

            return typeof(BossPlayerShootState);
        }

        public override void Enter()
        {
        }

        public override void Update(float delta)
        {
            if (StateTime > 1.3f)
            {
                Entity.ShootAtPlayer();
                StateTime = 0;
            }
        }

        public override void Exit()
        {
        }
    }
}
