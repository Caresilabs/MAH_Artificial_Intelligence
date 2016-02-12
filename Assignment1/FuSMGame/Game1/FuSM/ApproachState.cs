using Game1.Entitys;

namespace Game1.FuSM
{
    public class ApproachState : FuSMMachine<FuSMEnemy>.FuSMState
    {

        public override float CalculateActivation()
        {
            float dst = Machine.GetPerception<float>("DistanceToPlayer");

            if (!Machine.GetPerception<bool>("CanSeePlayer") || dst < FuSMEnemy.APPROACH_STOP_DISTANCE || dst > FuSMEnemy.APPROACH_DISTANCE )
            {
                ActivationLevel = 0;
            }
            else
            {
                // y = a*(x - x1)*(x - x2);

                float vertexX = ((FuSMEnemy.APPROACH_DISTANCE + FuSMEnemy.APPROACH_STOP_DISTANCE) * 0.5f);

                // Offset in roots to avoid REALLY small numbers
                float offset = 2;

                float a = 1 / ((vertexX - (FuSMEnemy.APPROACH_DISTANCE - offset)) * (vertexX - (FuSMEnemy.APPROACH_STOP_DISTANCE + offset)));

                ActivationLevel = a * (dst - FuSMEnemy.APPROACH_DISTANCE) * (dst - FuSMEnemy.APPROACH_STOP_DISTANCE);

            }

            return ActivationLevel;
        }

        public override void Enter()
        {
          
        }

        public override void Exit()
        {
            Entity.Speed = 0;
        }

        public override void Init()
        {
        }

        public override void Update(float delta)
        {
            Entity.Speed = ActivationLevel * FuSMEnemy.DEFAULT_SPEED;
            Entity.UpdateAIMovement();
        }
    }
}
