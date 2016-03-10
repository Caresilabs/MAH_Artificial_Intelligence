﻿using Boids;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simon.Mah.Framework
{
    /// <summary>
    /// TODO Explain
    /// Insert O(1) (hashtable)
    /// Remove Average O(1)
    /// Clear O(1)
    /// 
    /// </summary>
    public class SpatialHashGrid
    {
        private List<Boid>[] Buckets;

        private int Rows;
        private int Cols;
        private int SceneWidth;
        private int SceneHeight;
        private float CellSize;

        public void Setup(int scenewidth, int sceneheight, float cellsize)
        {
            Cols = (int)Math.Ceiling(scenewidth / cellsize);
            Rows = (int)Math.Ceiling(sceneheight / cellsize);
            Buckets = new List<Boid>[Cols * Rows];

            for (int i = 0; i < Cols * Rows; i++)
            {
                Buckets[i] = new List<Boid>();
            }

            SceneWidth = scenewidth;
            SceneHeight = sceneheight;
            CellSize = cellsize;
        }


        public void ClearBuckets()
        {
            //Buckets.Clear();
            for (int i = 0; i < Cols * Rows; i++)
            {
                Buckets[i].Clear();
            }
        }

        public void AddObject(Boid obj)
        {
            var cellIds = GetIdForObj(obj);
            foreach (var item in cellIds)
            {
                Buckets[(item)].Add(obj);
            }
        }

        public void AddObject(System.Collections.Generic.IEnumerable<Boid> objs)
        {
            foreach (var item in objs)
            {
                AddObject(item);
            }
        }

        private IList<int> GetIdForObj(Boid obj)
        {
            var bucketsObjIsIn = new List<int>();

            Vector2 min = new Vector2(
               Math.Max(Math.Min( obj.Position.X - (Boid.RADIUS), SceneWidth - 1), 1),
                 Math.Max(Math.Min(obj.Position.Y - (Boid.RADIUS), SceneHeight -1), 1));

            Vector2 max = new Vector2(
                Math.Max(Math.Min(obj.Position.X + (Boid.RADIUS), SceneWidth - 1), 1),
               Math.Max(Math.Min(obj.Position.Y + (Boid.RADIUS), SceneHeight - 1), 1));

            float width = Cols;

            //TopLeft
            AddBucket(min, width, bucketsObjIsIn);

            //TopRight
            AddBucket(new Vector2(max.X, min.Y), width, bucketsObjIsIn);

            //BottomRight
            AddBucket(new Vector2(max.X, max.Y), width, bucketsObjIsIn);

            //BottomLeft
            AddBucket(new Vector2(min.X, max.Y), width, bucketsObjIsIn);

            return bucketsObjIsIn;
        }

        private void AddBucket(Vector2 vector, float width, IList<int> buckettoaddto)
        {
            int cellPosition = (int)((Math.Floor(vector.X / CellSize)) + (Math.Floor(vector.Y / CellSize)) * width);

            if (!buckettoaddto.Contains(cellPosition))
                buckettoaddto.Add(cellPosition);

        }

        public Boid[] GetPossibleColliders(Boid obj)
        {
            var objects = new List<Boid>();
            var bucketIds = GetIdForObj(obj);
            foreach (var item in bucketIds)
            {
                objects.AddRange(Buckets[item]);
            }
            return objects.Distinct().ToArray();
        }


    }
}
