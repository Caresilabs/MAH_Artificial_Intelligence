using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boids
{
    public class Interest
    {
        public enum EType
        {
            Approach, Pursuit, Arrive, Wander, Flee
        }

        public EType Type { get; private set; }

        public Vector2 Position { get; private set; }

        public Interest(EType type, Vector2 pos)
        {
            this.Type = type;
            this.Position = pos;
        }

        public void Draw(SpriteBatch batch)
        {
            Color color = Color.White;

            switch (Type)
            {
                case EType.Approach:
                    color = Color.Green;
                    break;
                case EType.Pursuit:
                    color = Color.Yellow;
                    break;
                case EType.Arrive:
                    color = Color.Beige;
                    break;
                case EType.Wander:
                    color = Color.Aqua;
                    break;
                case EType.Flee:
                    color = Color.Red;
                    break;
                default:
                    break;
            }

            batch.Draw(Game1.RECT_TEXTURE, Position, null, color, 0, new Vector2(Game1.RECT_TEXTURE.Width / 2f, Game1.RECT_TEXTURE.Height / 2f), 0.5f, SpriteEffects.None, 0);
        }

        public Vector2 wanderTarget = new Vector2();
        public void Affect(Boid boid)
        {
            float dst = (boid.Position - Position).Length();

            if (Type == EType.Approach && dst < 250)
            {
                boid.Seek(Position);
            }
            else if (Type == EType.Flee && dst < 80)
            {
                boid.Flee(Position);
            }
            else if (Type == EType.Arrive && dst < 200)
            {
                boid.Arrive(Position, 100);
            }
            else if (Type == EType.Pursuit && dst < 150)
            {
                boid.Pursuit(Position);
            }
            else if (Type == EType.Wander && dst < 100 )
            {
                if (BoidsScreen.RANDOM.NextDouble() < 0.001f || (wanderTarget - Position).Length() > 90)
                {
                    wanderTarget = new Vector2((float)Math.Cos(BoidsScreen.RANDOM.NextDouble() * Math.PI * 2), (float)Math.Sin(BoidsScreen.RANDOM.NextDouble() * Math.PI * 2));
                    wanderTarget *= (float)(BoidsScreen.RANDOM.NextDouble() * 90);
                    wanderTarget += Position;
                }

                boid.Seek(wanderTarget);
                //boid.Wander(Position, 90);
            }
        }
    }
}
