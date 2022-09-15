using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Movement;
using Buildings;
using UnitComponents;

namespace FSM.States
{

    public enum ExecutionState
    {
        NONE,
        ACTIVE,
        COMPLETED,
        TERMINATED,
    }

    public enum StateType
    {
        IDLE,
        PATROL,
        TRANSPORT,
        ATTACK,
        FOLLOW_ENEMY,
        RUN_AWAY,
        PLAYER_CONTROLLER,
    }

    public abstract class State : ScriptableObject
    {
        protected NavMeshAgent _navMeshAgent;
        protected NPC _npc;
        protected Unit _unit;
        protected Mover _mover;
        protected Fighter _fighter;
        protected Health _health;
        protected StateController _fsm;
        protected WorldElements _worldElements;
        protected ResourceController _resourceController;

        public ExecutionState ExecutionState { get; protected set; }
        public StateType StateType { get; protected set; }
        public bool EnteredState { get; protected set; }

        public virtual void OnEnable()
        {
            ExecutionState = ExecutionState.NONE;
        }

        public virtual bool EnterState()
        {
            ExecutionState = ExecutionState.ACTIVE;
            /*
            bool successNavmesh = true;
            bool successNPC = true;

            // Does the nav mesh agent exist?
            successNavmesh = (_navMeshAgent != null);

            // Does the executing agent exist?
            successNPC = (_npc != null);

            return successNavmesh & successNPC;
            */
            return true;
        }

        public abstract void UpdateState();

        public virtual bool ExitState()
        {
            // Debug.Log("Exit from current state");
            ExecutionState = ExecutionState.COMPLETED;
            return true;
        }

        public virtual void SetNavMeshAgent(NavMeshAgent navMeshAgent)
        {
            if (navMeshAgent != null)
            {
                _navMeshAgent = navMeshAgent;
            }
        }

        public virtual void SetExecutingNPC(NPC npc)
        {
            if (npc != null)
            {
                _npc = npc;
            }
        }

        public virtual void SetMover(Mover mover)
        {
            if (mover != null)
            {
                _mover = mover;
            }
        }

        public virtual void SetFighter(Fighter fighter)
        {
            if (fighter != null)
            {
                _fighter = fighter;
            }
        }

        public virtual void SetHealth(Health health)
        {
            if (health != null)
            {
                _health = health;
            }
        }

        public virtual void SetExecutingFSM(StateController fsm)
        {
            if (fsm != null)
            {
                _fsm = fsm;
            }
        }

        public virtual void SetWorldElements(WorldElements wE)
        {
            if (wE != null)
            {
                _worldElements = wE;
            }
        }

        public virtual void SetResourceController(ResourceController resourceController)
        {
            if (resourceController != null)
            {
                _resourceController = resourceController;
            }
        }

        public virtual void SetUnit(Unit unit)
        {
            if (unit != null)
            {
                _unit = unit;
            }
        }

        public virtual void UnitControlledByPlayer()
        {
            if (_unit.isUnitSelected && Input.GetMouseButtonDown(1))
            {
                _fsm.EnterState(StateType.PLAYER_CONTROLLER);
                return;
            }
        }
    }
}
