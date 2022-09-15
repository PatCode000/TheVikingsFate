using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

namespace FSM.States
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "FSM/States/Idle", order = 1)]

    public class IdleState : State
    {
        [SerializeField] float _idleDuration = 5f;
        float _totalDuration;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = StateType.IDLE;
        }

        public override bool EnterState()
        {
            EnteredState = false;
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                _totalDuration = 0f;
            }

            EnteredState = true;
            return EnteredState;
        }

        public override void UpdateState()
        {
            // Make player control
            UnitControlledByPlayer();
            
            EnemyInRange();

            EnemyInWeaponRange();

            TransportSheep();

            PatrolIfIdle();
        }

        private void PatrolIfIdle()
        {
            if (EnteredState)
            {
                _totalDuration += Time.deltaTime;

                if (_totalDuration >= _idleDuration)
                {
                    _fsm.EnterState(StateType.PATROL);
                    return;
                }
            }
        }

        private void TransportSheep()
        {
            // Call only if unit is worker type
            if (_unit.unitType == UnitType.WORKER)
            {
                if (_resourceController.GetNumberOfSheeps() > 0)
                {
                    _fsm.EnterState(StateType.TRANSPORT);
                }
            }
        }

        private void EnemyInWeaponRange()
        {
            // Don't call if unit is worker type
            if (_unit.unitType != UnitType.WORKER)
            {
                // Attack if player is in range
                if (_fighter.IsInWeaponRange())
                {
                    _mover.Cancel();
                    _fsm.EnterState(StateType.ATTACK);
                }
            }
        }

        private void EnemyInRange()
        {
            // Action if there is an enemy in range
            if (_unit.enemyInRange)
            {
                if (_unit.unitType == UnitType.WARRIOR)
                {
                    _fsm.EnterState(StateType.FOLLOW_ENEMY);
                }
                else if (_unit.unitType == UnitType.WORKER)
                {
                    // _fsm.EnterState(FSMStateType.RUN_AWAY);
                }
            }
        }

        public override bool ExitState()
        {
            base.ExitState();
            return true;
        }
    }
}
