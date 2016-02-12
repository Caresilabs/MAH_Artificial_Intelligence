using Game1.Entitys;

namespace Game1.FuSM
{
    public class AttackState : FuSMMachine<FuSMEnemy>.FuSMState
    {
        private float time;

        public override float CalculateActivation()
        {
            float dst = Machine.GetPerception<float>("DistanceToPlayer");

            if (!Machine.GetPerception<bool>("CanSeePlayer") || dst > FuSMEnemy.SHOOT_DISTANCE)
            {
                ActivationLevel = 0;
            }
            else
            {
                ActivationLevel = (FuSMEnemy.SHOOT_DISTANCE - dst) / FuSMEnemy.SHOOT_DISTANCE;
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

            if (time > 3.0f - (ActivationLevel * 3.0f))
            {
                time = 0;
                Entity.Fire();
            }
        }
    }
}
