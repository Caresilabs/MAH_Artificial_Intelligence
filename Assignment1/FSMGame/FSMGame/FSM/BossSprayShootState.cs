using MAH_Platformer.Entities;
using MAH_Platformer.Levels.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSMGame.FSM
{
    class BossSprayShootState : FiniteStateMachine<BossEntity>.State
    {
        private float shootTime;

        public override Type CheckState()
        {
            if (StateTime > 8)
            {
                return typeof(BossPatrolState);
            }

            return typeof(BossSprayShootState);
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update(float delta)
        {
            shootTime += delta;

            if (shootTime > Entity.Life * 0.08)
            {
                Entity.ShootSpray();
                Entity.Reloading = false;
                Entity.ShootAtPlayer();
                shootTime = 0;
            }
        }
    }
}
