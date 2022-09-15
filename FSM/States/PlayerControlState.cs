using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

namespace FSM.States
{
    [CreateAssetMenu(fileName = "PlayerControlState", menuName = "FSM/States/PlayerControl", order = 6)]

    public class PlayerControlState : State
    {
        RaycastHit hitPoint;
        float toleranceDistance = 1f;
        Vector3 targetPosition;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = StateType.PLAYER_CONTROLLER;
        }

        public override bool EnterState()
        {
            EnteredState = false;
            EnteredState = base.EnterState();

            ProcessChecks();

            EnteredState = true;
            return EnteredState;
        }


        public override void UpdateState()
        {

            if (_unit.isUnitSelected && Input.GetMouseButtonDown(1))
            {
                ProcessChecks();
            }

            // Check if unit is at destination point, if yes stop moving and go to the IDLE state
            if (IsUnitAtDestination())
            {
                StopMovementProcess();
            }
        }

        public override bool ExitState()
        {
            base.ExitState();
            return true;
        }

        private void ProcessChecks()
        {
            // Check if enemy was clicked
            if (IsEnemyClicked())
            {
                // If target enemy is in weapon range go to the attack state
                if (_fighter.IsTargetEnemyInWeaponRange())
                {
                    _fsm.EnterState(StateType.ATTACK);
                }
                else
                {
                    _fsm.EnterState(StateType.FOLLOW_ENEMY);
                }
            }
            else
            {
                // If enemy wasn't clicked go to the clicked place
                ProcessMovement();
            }
        }

        // Process unit movement
        private void ProcessMovement()
        {
            RaycastHit hit;
            Physics.Raycast(GetMouseRay(), out hit);
            targetPosition = hit.point;
            _mover.StartMoveAction(targetPosition);
        }

        // Check if unit is in destination place
        private bool IsUnitAtDestination()
        {
            if (targetPosition != null && Vector3.Distance(_unit.transform.position, targetPosition) < toleranceDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void StopMovementProcess()
        {
            _mover.Cancel();
            _fsm.EnterState(StateType.IDLE);
            return;
        }


        public virtual bool IsEnemyClicked()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit rayHit in hits)
            {
                if (rayHit.collider.tag == "enemyUnit" && !rayHit.transform.GetComponent<Health>().isDead)
                {
                    _fighter.SetEnemyAsTarget(rayHit.collider.gameObject);
                    return true;
                }
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
