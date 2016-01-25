using MAH_Platformer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSMGame.FSM
{
    class BossPlayerShootState : FiniteStateMachine<BossEntity>.State
    {
        public override Type CheckState()
        {
            return typeof(BossPlayerShootState);
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        public override void Exit()
        {
        }
    }
}
