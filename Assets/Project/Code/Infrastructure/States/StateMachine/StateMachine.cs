using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Scripts.Runtime.State_Machine
{
    [Serializable]
    public class StateMachine
    {
        [field: SerializeField]
        public bool TransitionsEnabled { get; set; } = true;
        [field: SerializeField]
        public bool HasCurrentState { get; private set; }

        [field: SerializeField]
        public State CurrentState { get; private set; }

        [field: SerializeField]
        public Transition CurrentTransition { get; private set; }

        private const int Capacity_DEFAULT = 3;

        private readonly HashSet<State> currentStates = new(Capacity_DEFAULT);

        private readonly List<Transition> anyTransitions = new(Capacity_DEFAULT);

        private readonly List<Transition> transitions = new(Capacity_DEFAULT);

        private bool isStatesAdded;

        public StateMachine() { }

        public StateMachine(params State[] states) : base() =>
            AddStates(states);

        public void AddStates(params State[] states)
        {
            if (isStatesAdded)
                throw new Exception("States already added!");

            for (int i = 0; i < states.Length; i++)
            {
                State state = states[i];

                if (states[i] == null)
                    throw new NullReferenceException(nameof(state));

                currentStates.Add(state);
            }

            if (states.Length > 0)
                isStatesAdded = true;
        }

        public async void SetState<TState>() where TState : State =>
            await SetState(typeof(TState));

        public async void SetState<TState, TPayload>(TPayload payload) where TState : State, IPayloadState<TPayload>
        {
            IPayloadState<TPayload> state = (IPayloadState<TPayload>)GetState(typeof(TState));
            if (HasCurrentState)
                await ExitCurrentStateAsync();
            CurrentState = GetState(typeof(TState));
            HasCurrentState = true;
            await state.OnEnterAsync(payload);
            CurrentState.IsActive = true;
        }

        public void AddTransition<TStateFrom, TStateTo>(Func<bool> condition)
            where TStateFrom : State
            where TStateTo : State
        {
            State stateFrom = GetState(typeof(TStateFrom));
            State stateTo = GetState(typeof(TStateTo));

            transitions.Add(new Transition(stateFrom, stateTo, condition));
        }

        public void AddAnyTransition<TStateTo>(Func<bool> condition)
            where TStateTo : State
        {
            State stateTo = GetState(typeof(TStateTo));

            anyTransitions.Add(new Transition(null, stateTo, condition));
        }

        public void Update()
        {
            if (TransitionsEnabled)
                SetStateByTransitions();

            if (HasCurrentState)
                CurrentState.OnUpdate();
        }

        public async UniTaskVoid SetStateByTransitions()
        {
            CurrentTransition = GetTransition();

            if (CurrentTransition == null)
                return;

            if (CurrentState == CurrentTransition.To)
                return;

            await SetState(CurrentTransition.To.GetType());
        }

        public TState GetState<TState>() where TState : State =>
            (TState)GetState(typeof(TState));

        private State GetState(Type type)
        {
            foreach (State state in currentStates)
            {
                if (state.GetType() == type)
                    return state;
            }

            throw new Exception($"The <{type.Name}> is not found!");
        }

        private async UniTask SetState(Type type)
        {
            if (HasCurrentState)
                await ExitCurrentStateAsync();

            CurrentState = GetState(type);
            HasCurrentState = true;

            await EnterCurrentState();
        }

        private async UniTask EnterCurrentState()
        {
            await CurrentState.OnEnterAsync();
            CurrentState.IsActive = true;
        }

        private async UniTask ExitCurrentStateAsync()
        {
            try
            {
                await CurrentState.OnExitAsync();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            CurrentState.IsActive = false;
        }

        private Transition GetTransition()
        {
            for (int i = 0; i < anyTransitions.Count; i++)
            {
                if (anyTransitions[i].Condition())
                    return anyTransitions[i];
            }

            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].From.IsActive == false)
                    continue;

                if (transitions[i].Condition())
                    return transitions[i];
            }

            return default;
        }
    }
}