using Game1.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1.Utilities
{
    class Spawner
    {
        SimulationWorld world;
        float timeAlive, interval, intervalTime;
        int spawned;
        public Spawner(SimulationWorld world)
        {
            this.world = world;
            this.timeAlive = 0;
            this.interval = 7f;
            this.spawned = 0;
            world.PlayerScore = 0;

            world.SpawnEnemy();
        }

        public void Update(float delta)
        {
            if (world.Enemies.Count == 0)
                world.SpawnEnemy();

            return;

            timeAlive += delta;
            intervalTime += delta;
            if(intervalTime >= interval)
            {
                intervalTime = 0;
                world.SpawnEnemy();
                spawned++;
                if(spawned >= 6)
                {
                    intervalTime -= 0.7f;
                    if (intervalTime < 1f)
                        intervalTime = 1f;
                }
            }
        }
    }
}
