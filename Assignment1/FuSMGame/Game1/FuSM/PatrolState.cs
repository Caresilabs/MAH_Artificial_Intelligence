using Game1.Entitys;
using Microsoft.Xna.Framework;
using Patrik.GameProject;
using System;

namespace Game1.FuSM
{
    public class PatrolState : FuSMMachine<FuSMEnemy>.FuSMState
    {
        private DateTime lastTimeSeenPlayer;

        private Vector2[] targets = new Vector2[2];
        private int targetIndex = 0;

        public override float CalculateActivation()
        {
            if (Machine.GetPerception<bool>("IsNearPlayer"))
            {
                ActivationLevel = 0;
                lastTimeSeenPlayer = DateTime.Now;
                targets[0] = Entity.World.Player.Position;
                targets[1] = Entity.Position;
            }
            else
            {
                ActivationLevel = MathHelper.Clamp((float)(DateTime.Now - lastTimeSeenPlayer).TotalSeconds / FuSMEnemy.TIME_BEFORE_PATROLLING, 0, 1);
            }

            return ActivationLevel;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
            targetIndex = 0;
        }

        public override void Init()
        {
            lastTimeSeenPlayer = DateTime.Now;
            targets[0] = Entity.World.Player.Position;
            targets[1] = Entity.Position;
        }

        public override void Update(float delta)
        {
            // If we reach waypoint x, go to next
            if ((Entity.Position - targets[targetIndex]).Length() < Tile.SIZE)
                targetIndex = (targetIndex + 1) % targets.Length;

            // lerp between waypoint and the player
            Vector2 target = Vector2.Lerp( Entity.World.Player.Position, targets[targetIndex], ActivationLevel);

            // Move
            Entity.Speed = FuSMEnemy.DEFAULT_SPEED * 0.7f;
            Entity.UpdateAIMovement(target);

            Entity.Face(target);

        }
    }
}
