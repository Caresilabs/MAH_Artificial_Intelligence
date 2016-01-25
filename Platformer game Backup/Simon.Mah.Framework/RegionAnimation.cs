using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Simon.Mah.Framework.Scene2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simon.Mah.Framework
{
    public class RegionAnimation : Animation
    {
        private List<TextureRegion> regions;
        private float stateTime;
        private float frameDuration;

        public RegionAnimation(float frameDuration, params TextureRegion[] regions)
        {
            this.stateTime = 0;
            this.frameDuration = frameDuration;
            this.regions = new List<TextureRegion>();
            foreach (var region in regions)
            {
                this.regions.Add(region);
            }
        }

        public override bool HasNext()
        {
            return lastFrame != currentFrame;
        }

        public override void Update(float delta)
        {
            lastFrame = currentFrame;
            stateTime += delta;
            currentFrame = (int)(stateTime / (float)frameDuration) % regions.Count;
        }

        public override TextureRegion GetRegion()
        {
            return regions[currentFrame];
        }
    }
}
