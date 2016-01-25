using System;
using System.Collections.Generic;
using System.Linq;

namespace FSMGame.FSM
{
    public class FiniteStateMachine<T>
    {
        private Dictionary<Type, State> states;

        private State state;

        private T entity;

        public FiniteStateMachine(T entity)
        {
            this.states = new Dictionary<Type, State>();
            this.entity = entity;
        }

        public FiniteStateMachine<T> Add(State state)
        {
            states.Add(state.GetType(), state);
            return this;
        }

        public void Update(float delta)
        {
            if (state == null)
            {
                state = states.First().Value;
                if (state == null)
                    return;

                state.Entity = entity;
                state.Enter();
            }

            // Update
            state.StateTime += delta;
            state.Update(delta);

            // Check state
            State newState = states[state.CheckState()];
            if (newState != state && newState != null)
            {
                state.Exit();
                newState.Entity = entity;
                newState.StateTime = 0;
                newState.Enter();
                state = newState;
            }
        }

        public abstract class State
        {
            public float StateTime { get; internal set; }

            public T Entity { get; internal set; }

            public abstract void Enter();

            public abstract void Update(float delta);

            public abstract void Exit();

            public abstract Type CheckState();
        }

    }
}
