using Game1.Entitys;
using System;

namespace Game1.FuSM
{
    public class ForewarnState : FuSMMachine<FuSMEnemy>.FuSMState
    {
        public override float CalculateActivation()
        {
            return 0;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Init()
        {
        }

        public override void Update(float delta)
        {
        }
    }
}
