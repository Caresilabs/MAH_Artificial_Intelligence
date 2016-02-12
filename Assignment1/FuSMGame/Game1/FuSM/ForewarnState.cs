using Game1.Entitys;
using System;

namespace Game1.FuSM
{
    public class ForewarnState : FuSMMachine<FuSMEnemy>.FuSMState
    {
        private float time;

        public override float CalculateActivation()
        {
            float dst = Machine.GetPerception<float>("DistanceToPlayer");

            if (!Machine.GetPerception<bool>("CanSeePlayer") || dst < FuSMEnemy.FOREWARN_STOP_DISTANCE || dst > FuSMEnemy.FOREWARN_DISTANCE)
            {
                ActivationLevel = 0;
            }
            else
            {
                ActivationLevel =  1 - ((dst - FuSMEnemy.FOREWARN_STOP_DISTANCE) / (FuSMEnemy.FOREWARN_DISTANCE - FuSMEnemy.FOREWARN_STOP_DISTANCE));
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

            if (time > 2.5f - (ActivationLevel * 1.5f))
            {
                time = 0;
                if (ActivationLevel < 0.52f)
                {
                    Entity.World.Hud.AddText(Entity, "Stop right there, sir!");
                }
                else
                {
                    Entity.World.Hud.AddText(Entity, "Last warning!");
                }
            }
        }
    }
}
