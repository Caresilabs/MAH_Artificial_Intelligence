using System;
using System.Collections.Generic;

namespace Game1.FuSM
{
    public class FuSMMachine<T>
    {
        private T Entity { get; set; }

        private Dictionary<Type, FuSMState> States { get; set; }

        private Dictionary<Type, FuSMState> ActivatedStates { get; set; }

        public FuSMMachine(T entity)
        {
            this.Entity = entity;
        }

        public void Update(float delta)
        {
            if (States.Count == 0)
                return;

            ActivatedStates.Clear();

            var nonActiveStates = new Dictionary<Type, FuSMState>() ;

            foreach (var pair in States) {
                if (pair.Value.CalculateActivation() > 0)
                    ActivatedStates.Add(pair.Key, pair.Value);
                else
                    nonActiveStates.Add(pair.Key, pair.Value);
            }

            //Exit all non active states for cleanup
            if (nonActiveStates.Count != 0)
            {
                foreach (var pair in nonActiveStates)
                    pair.Value.Exit();
            }

            //Update all activated states
            if (ActivatedStates.Count != 0)
            {
                foreach (var pair in ActivatedStates)
                    pair.Value.Update(delta);
            }
        }

        public FuSMMachine<T> AddState(FuSMState state)
        {
            States.Add(typeof(T), state);
            return this;
        }

        public abstract class FuSMState
        {
            public T Entity { get; private set; }

            public FuSMState(T entity)
            {
                this.Entity = entity;
            }

            public abstract void Update(float delta);

            public abstract void Enter();

            public abstract void Exit();

            public abstract void Init();

            public abstract float CalculateActivation();// { return m_activationLevel; }
        }
    }


}
