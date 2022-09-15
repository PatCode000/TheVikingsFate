using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

namespace FSM.States
{
    [CreateAssetMenu(fileName = "FollowEnemyState", menuName = "FSM/States/FollowEnemy", order = 5)]

    public class FollowEnemyState : State
    {
        Vector3 targetEnemyPosition;

        int currentNumberOfEnemies = 0;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = StateType.FOLLOW_ENEMY;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();
            EnteredState = true;

            return EnteredState;
        }

        public override void UpdateState()
        {
            UnitControlledByPlayer();

            SetTarget();
            CheckIfEnemyInWeaponRange();
        }

        public override bool ExitState()
        {
            base.ExitState();
            return true;
        }

        private void CheckIfEnemyInWeaponRange()
        {
            if (_unit.enemyInRange)
            {
                if (_fighter.IsInWeaponRange())
                {
                    _mover.Cancel();
                    _fsm.EnterState(StateType.ATTACK);
                }
                else
                {
                    _mover.StartMoveAction(targetEnemyPosition);
                }
            }
            else
            {
                if (_fighter.targetEnemy != null)
                {
                    _mover.StartMoveAction(targetEnemyPosition);
                }
                else
                {
                    _mover.Cancel();
                    _fsm.EnterState(StateType.IDLE);
                    return;
                }
            }
        }

        private void SetTarget()
        {
            // Determine target
            if (_fighter.targetEnemy != null && !_fighter.targetEnemy.GetComponent<Health>().isDead)
            {
                targetEnemyPosition = _fighter.targetEnemy.transform.position;
            }
            else
            {
                // Check if there is any alive enemy around this unit
                if (_unit.IsEnemyInRange())
                {
                    _unit.SetClosestEnemy();
                    targetEnemyPosition = _unit.closestEnemy.transform.position;
                }
                else
                {
                    _fsm.EnterState(StateType.IDLE);
                }
            }
        }
    }
}
