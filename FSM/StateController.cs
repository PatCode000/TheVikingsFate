using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM.States;
using Movement;
using Buildings;
using UnitComponents;

namespace FSM
{
    public class StateController : MonoBehaviour
    {
        public State _currentState;

        [SerializeField] List<State> _validStates;
        Dictionary<StateType, State> _fsmStates;

        NavMeshAgent navMeshAgent;
        Unit unit;
        NPC npc;
        Mover mover;
        Fighter fighter;
        Health health;

        // Set all of important references at the very start
        private void Awake()
        {
            _currentState = null;

            _fsmStates = new Dictionary<StateType, State>();

            GetReferencesToComponents();
            SetReferencesForStates();
        }

        // Enter first state
        private void Start()
        {
            EnterState(StateType.IDLE);
        }

        // Update appropriate states
        private void Update()
        {
            if (_currentState != null && !health.isDead)
            {
                _currentState.UpdateState();
            }
        }

        // This function will take references to every component of this NPC
        private void GetReferencesToComponents()
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            unit = this.GetComponent<Unit>();
            npc = this.GetComponent<NPC>();
            mover = this.GetComponent<Mover>();
            fighter = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
        }

        // This function create an instance of state and will save references for every state in the list.
        // After all instntiaded state is added to the dictionary.
        private void SetReferencesForStates()
        {
            // Create an instance of the State and add it 
            foreach (State state in _validStates)
            {
                State instanceOfState = Object.Instantiate(state);
                instanceOfState.SetNavMeshAgent(navMeshAgent);
                instanceOfState.SetMover(mover);
                instanceOfState.SetFighter(fighter);
                instanceOfState.SetHealth(health);
                instanceOfState.SetUnit(unit);
                instanceOfState.SetExecutingNPC(npc);
                instanceOfState.SetExecutingFSM(this);
                instanceOfState.SetWorldElements(FindObjectOfType<WorldElements>());
                instanceOfState.SetResourceController(FindObjectOfType<ResourceController>());
                _fsmStates.Add(instanceOfState.StateType, instanceOfState);
            }
        }

        #region STATE MANAGEMENT
        // Call this function when enter new state to end the current state and set the new one
        public void EnterState(State nextState)
        {
            if (nextState == null)
            {
                return;
            }

            if (_currentState != null)
            {
                _currentState.ExitState();
            }

            _currentState = nextState;
            _currentState.EnterState();
        }

        // Call this function when enter the state to start new state end call EnterState() functions
        public void EnterState(StateType stateType)
        {
            if (_fsmStates.ContainsKey(stateType))
            {
                State nextState = _fsmStates[stateType];
                EnterState(nextState);
            }
        }
        #endregion

    }
}