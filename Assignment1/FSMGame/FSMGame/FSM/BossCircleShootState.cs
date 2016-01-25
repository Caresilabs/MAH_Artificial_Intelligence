using MAH_Platformer.Entities;
using System;

namespace FSMGame.FSM
{
    class BossCircleShootState : FiniteStateMachine<BossEntity>.State
    {
        public override Type CheckState()
        {
            if (StateTime > 2)
                return typeof(BossPlayerShootState);

            return typeof(BossCircleShootState);
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            Entity.ShootCircle();
        }

        public override void Exit()
        {
        }
    }
}
