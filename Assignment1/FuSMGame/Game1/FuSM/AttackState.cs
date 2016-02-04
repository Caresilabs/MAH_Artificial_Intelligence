using Game1.Entitys;

namespace Game1.FuSM
{
    public class AttackState : FuSMMachine<FuSMEnemy>.FuSMState
    {
        private float time;

        public override float CalculateActivation()
        {
            if (Machine.GetPerception<bool>("CanSeePlayer"))
            {
                ActivationLevel = 0.5f;

            }
            else
            {
                ActivationLevel = 0;
            }

            return ActivationLevel;
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
            time += delta;
            Entity.Face(Entity.World.Player.Position);

            if (time > 3.5f - (ActivationLevel * 3.5f))
            {
                time = 0;
                Entity.Fire();
            }
        }
    }
}
