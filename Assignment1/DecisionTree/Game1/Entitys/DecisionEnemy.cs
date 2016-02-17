using Game1.Scene;
using Microsoft.Xna.Framework;
using Patrik.GameProject;
using System;

namespace Game1.Entitys
{
    /// <summary>
    /// Enemy that uses a decision tree.
    /// </summary>
    public class DecisionEnemy : BaseEnemy
    {
        public const float SHOOT_DISTANCE = Tile.SIZE * 5.5f;

        public const float DEFAULT_SPEED = 200;

        private int targetIndex = 0;
        private Vector2[] targets = new Vector2[2];

        // Perceptions
        public bool CanSeePlayer { get; set; }
        public bool IsPlayerInShootDistance { get; set; }

        public DecisionEnemy(Vector2 position, SimulationWorld world) : base(Globals.player, position, 100, 40, world)
        {
            Health = 150;
            MaxHealth = 150;
            color = Color.Cyan;
            weapon = new Rifle(world, this);
            GeneratePatrolPath();
        }

        /// <summary>
        /// Try to generate a random walking path
        /// </summary>
        private void GeneratePatrolPath()
        {
            var rnd = new Random();
            Vector2 targetB = Position;
            for (int i = 0; i < 1000; i++)
            {
                // Random position
                var tryPos = Position / Tile.SIZE + new Vector2((float)(rnd.NextDouble() * 8 - 4), (float)(rnd.NextDouble() * 8 - 4));
                Tile t = World.Map.GetTile((int)tryPos.X, (int)tryPos.Y);
                if (t != null && !t.Blocks(this))
                {
                    targetB = t.Position;
                    break;
                }
            }
            targets[0] = targetB;
            targets[1] = Position;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        protected override void UpdateAI(float delta)
        {
            // Update le Perceptions
            UpdatePerceptions(delta);

            // Update our decision tree
            UpdateDecisions(delta);
        }

        private void UpdateDecisions(float delta)
        {
            if (CanSeePlayer)
            {
                // Low on health
                if (Health < MaxHealth * 0.35f)
                {
                    // Seek health and Cover
                    SeekHealthAndCover();
                    Speed = DEFAULT_SPEED * 1.4f;
                }
                else
                {
                    if (IsPlayerInShootDistance)
                    {
                        // Fire!!
                        Fire();
                        Face(World.Player.Position);
                        Speed = 0;
                    }
                    else
                    {
                        if (World.Player.Health < World.Player.MaxHealth * 0.3f)
                        {
                            // Chase down wounded player
                            Speed = DEFAULT_SPEED * 1.3f;
                            UpdateAIMovement(World.Player.Position);
                            Face(World.Player.Position);
                        }
                        else
                        {
                            // Approach
                            Speed = DEFAULT_SPEED * 1f;
                            UpdateAIMovement(World.Player.Position);
                            Face(World.Player.Position);
                        }

                    }
                }
            }
            else
            {
                // Medium on health
                if (Health < MaxHealth * 0.8f)
                {
                    // Seek Health
                    SeekNearestHealth();
                    Speed = DEFAULT_SPEED * 1.4f;
                }
                else
                {
                    // Patrol
                    Patrol();
                    Speed = DEFAULT_SPEED * 0.8f;
                }
            }
        }

        /// <summary>
        /// Seek the closest (by distance) health station
        /// </summary>
        private void SeekNearestHealth()
        {
            Tile closest = null;
            foreach (var item in World.Map.getTileMap())
            {
                if (item.GetTileType() == ETileType.SPAWN)
                {
                    if (closest == null ||
                        (closest != null && (Position - item.Position).Length() < (Position - closest.Position).Length()))
                        closest = item;
                }
            }
            UpdateAIMovement(closest.Position);
        }

        /// <summary>
        /// Seek a health station, and try to choose a cover place
        /// </summary>
        private void SeekHealthAndCover()
        {
            Tile cover = null;
            foreach (var item in World.Map.getTileMap())
            {
                if (item.GetTileType() == ETileType.SPAWN)
                {
                    // Choose only if player can't see the spawn.
                    if (!World.RayCast(item, World.Player, this))
                    {
                        if (cover == null)
                            cover = item;

                        // If it's closer than prev.
                        if ((Position - item.Position).Length() < (Position - cover.Position).Length())
                            cover = item;
                    }
                }
            }
            UpdateAIMovement(cover.Position);
        }

        /// <summary>
        /// Patrol and area
        /// </summary>
        private void Patrol()
        {
            // Regenerate path if the length is to far away
            if ((targets[targetIndex] - Position).Length() > Tile.SIZE * 7)
                GeneratePatrolPath();

            if ((Position - targets[targetIndex]).Length() < Tile.SIZE)
                targetIndex = (targetIndex + 1) % targets.Length;

            Vector2 target = targets[targetIndex];

            // Move
            UpdateAIMovement(target);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public void UpdatePerceptions(float delta)
        {
            CanSeePlayer = World.RayCast(this, World.Player);

            float dstToPlayer = (World.Player.GetPosition() - position).Length();

            IsPlayerInShootDistance = dstToPlayer <= SHOOT_DISTANCE;

        }

        public void UpdateAIMovement()
        {
            UpdateAIMovement(World.Player.Position);
        }

        public void UpdateAIMovement(Vector2 trgt)
        {
            if (target != Vector2.Zero && (target - position).Length() > 6)
            {
                float range = (target - position).Length();
                direction = (target - position);
                direction.Normalize();
            }
            else
            {
                RebuildPath(trgt);
            }
        }
        
    }
}
