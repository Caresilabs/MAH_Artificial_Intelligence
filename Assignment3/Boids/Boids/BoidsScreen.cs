using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Simon.Mah.Framework;
using System;

namespace Boids
{
    public class BoidsScreen
    {
        public const int WIDTH = 1280;
        public const int HEIGHT = 720;

        public static readonly Random RANDOM = new Random();

        public Camera2D Camera { get; private set; }

        public World World { get; private set; }

        private int lastScrollWheel;

        private Interest.EType currentType = Interest.EType.Approach;

        public void OnCreate(GraphicsDevice device)
        {
            this.Camera = new Camera2D(device, WIDTH, HEIGHT);
            this.World = new World();
        }

        public void Update(float delta)
        {
            World.Update(delta);

            {
                int deltaScroll = Mouse.GetState().ScrollWheelValue - lastScrollWheel;
                lastScrollWheel = Mouse.GetState().ScrollWheelValue;

                int toChange = deltaScroll / 20;

                if (toChange > 0)
                {
                    World.SpawnBoids(toChange);
                }
                else if (toChange < 0)
                {
                    if (Math.Abs(toChange) <= World.Boids.Count)
                        World.Boids.RemoveRange(0, Math.Abs(toChange));
                }
            }

            UpdateInputs(delta);
        }

        private void UpdateInputs(float delta)
        {
            if (InputHandler.KeyReleased(Keys.Enter))
            {
                World.Interests.Clear();
            }

            if (InputHandler.KeyReleased(Keys.D1))
            {
                currentType = (Interest.EType)0;
            }
            if (InputHandler.KeyReleased(Keys.D2))
            {
                currentType = (Interest.EType)1;
            }
            if (InputHandler.KeyReleased(Keys.D3))
            {
                currentType = (Interest.EType)2;
            }
            if (InputHandler.KeyReleased(Keys.D4))
            {
                currentType = (Interest.EType)3;
            }
            if (InputHandler.KeyReleased(Keys.D5))
            {
                currentType = (Interest.EType)4;
            }

            if (InputHandler.Clicked())
            {
                var pos = Camera.Unproject(Mouse.GetState().X, Mouse.GetState().Y);
                World.SpawnInterest(currentType, pos);
            }

            InputHandler.Update(delta);
        }

        public void Draw(SpriteBatch batch)
        {
            // Draw World
            batch.Begin(SpriteSortMode.BackToFront,
                     BlendState.AlphaBlend,
                     SamplerState.LinearClamp,
                     null,
                     null,
                     null,
                     Camera.GetMatrix());

            foreach (var boid in World.Boids)
            {
                batch.Draw(boid.Texture, boid.Position, null, Color.White, (float)(Math.Atan2(boid.Velocity.Y, boid.Velocity.X) + Math.PI / 2f), new Vector2(boid.Texture.Width / 2f, boid.Texture.Height / 2f), Boid.RADIUS / boid.Texture.Width, SpriteEffects.None, 0);
            }

            foreach (var interest in World.Interests)
            {
                interest.Draw(batch);
            }

            // Draw ui
            batch.DrawString(Game1.FONT, String.Format("Num: {0}", World.Boids.Count), new Vector2(), Color.WhiteSmoke);

            batch.DrawString(Game1.FONT, currentType.ToString(), Camera.Unproject(Mouse.GetState().X, Mouse.GetState().Y) + new Vector2(10, - 16), Interest.GetColorFromType(currentType));

            batch.End();
        }
    }
}
