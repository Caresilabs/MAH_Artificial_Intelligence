﻿using Game1.Scene;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1.Entitys
{
    class FastEnemy : BaseEnemy
    {
        public FastEnemy(Vector2 position, SimulationWorld world) : base(Globals.player, position, 200, 30, world)
        {
            Health = 70;
            MaxHealth = 70;
            color = Color.Pink;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }
    }
}
