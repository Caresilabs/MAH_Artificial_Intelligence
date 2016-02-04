using Game1.Entitys;

namespace Game1.FuSM
{
    public class ApproachState : FuSMMachine<FuSMEnemy>.FuSMState
    {
        private const int DEFAULT_SPEED = 200;

        public override float CalculateActivation()
        {
            float dst = (Entity.Position - Entity.World.Player.Position).Length();

            if ( Machine.GetPerception<bool>("IsNearPlayer") )
            {
                ActivationLevel = 0;
            }
            else
            {
                ActivationLevel = dst / FuSMEnemy.APPROACH_DISTANCE;
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
            Entity.Speed = ActivationLevel * DEFAULT_SPEED;
            Entity.UpdateAIMovement();
        }
    }
}
