using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.States
{
    [CreateAssetMenu(fileName = "PatrolState", menuName = "FSM/States/Patrol", order = 2)]

    public class PatrolState : State
    {
        float waypointTolerance = 1f;
        int currentWaypointIndex = 0;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = StateType.PATROL;
        }

        public override bool EnterState()
        {
            EnteredState = false;
            EnteredState = base.EnterState();

            EnteredState = true;
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                PatrolBehaviour();
            }
        }

        public override bool ExitState()
        {
            base.ExitState();
            return true;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _navMeshAgent.transform.position;

            if (_npc.patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                    _fsm.EnterState(StateType.IDLE);
                    return;
                }
                nextPosition = GetCurrentWaypoint();
            }

            _mover.StartMoveAction(nextPosition);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(_navMeshAgent.transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = _npc.patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _npc.patrolPath.GetWaypoint(currentWaypointIndex);
        }
    }
}
