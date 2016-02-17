using System;
using System.Collections.Generic;
using System.Linq;

namespace Game1.FuSM
{
    public class FuSMMachine<T>
    {
        /// <summary>
        /// Should It normalize activation levels?
        /// </summary>
        private const bool NORMALIZE_LEVELS = true;

        private T Entity { get; set; }

        /// <summary>
        /// All of the states
        /// </summary>
        public Dictionary<Type, FuSMState> States { get; private set; }
        
        /// <summary>
        /// All Activated states
        /// </summary>
        private Dictionary<Type, FuSMState> ActivatedStates { get; set; }

        /// <summary>
        /// Perception map
        /// </summary>
        private Dictionary<string, object> Perceptions { get; set; }

        public FuSMMachine(T entity)
        {
            this.Entity = entity;
            this.States = new Dictionary<Type, FuSMState>();
            this.ActivatedStates = new Dictionary<Type, FuSMState>();
            this.Perceptions = new Dictionary<string, object>();
        }

        public void Update(float delta)
        {
            if (States.Count == 0)
                return;

            ActivatedStates.Clear();

            var nonActiveStates = new Dictionary<Type, FuSMState>();

            foreach (var pair in States)
            {
                if (pair.Value.CalculateActivation() > 0)
                    ActivatedStates.Add(pair.Key, pair.Value);
                else
                    nonActiveStates.Add(pair.Key, pair.Value);
            }

            // Exit all non active states for cleanup
            if (nonActiveStates.Count != 0)
            {
                foreach (var pair in nonActiveStates)
                    pair.Value.Exit();
            }

            // Update all activated states
            if (ActivatedStates.Count != 0)
            {
                float sum = ActivatedStates.Sum(x => x.Value.ActivationLevel);

                foreach (var pair in ActivatedStates)
                {
                    // Normalize the activation level if sum is larger than 1.
                    if (NORMALIZE_LEVELS && sum > 1)
                    {
                        pair.Value.ActivationLevel = (pair.Value.ActivationLevel) / sum;
                    }

                    pair.Value.Update(delta);
                }
            }
        }

        public void SetPerception(string name, object value)
        {
            Perceptions[name] = value;
        }

        public P GetPerception<P>(string name)
        {
            object obj = Perceptions[name];
            if (obj == null || (obj != null && !(obj is P)))
            {
                throw new Exception("Wrong Type Exception");
                //return default(P);
            }
            return (P)obj;
        }

        public FuSMMachine<T> AddState(FuSMState state)
        {
            state.Entity = Entity;
            state.Machine = this;
            state.Init();
            States.Add(state.GetType(), state);
            return this;
        }

        public abstract class FuSMState
        {
            public float ActivationLevel { get; internal set; }

            public FuSMMachine<T> Machine { get; internal set; }

            public T Entity { get; internal set; }

            public abstract void Update(float delta);

            public abstract void Enter();

            public abstract void Exit();

            public abstract void Init();

            public abstract float CalculateActivation();
        }
    }


}
