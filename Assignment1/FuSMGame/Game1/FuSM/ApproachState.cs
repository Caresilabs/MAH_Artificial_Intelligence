using Game1.Entitys;

namespace Game1.FuSM
{
    public class ApproachState : FuSMMachine<FuSMEnemy>.FuSMState
    {
        public ApproachState(FuSMEnemy entity) : base(entity)
        {
        }

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
