using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

namespace FSM.States
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "FSM/States/Attack", order = 4)]

    public class AttackState : State
    {
        Transform targetEnemy;

        float totalTime = 0;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = StateType.ATTACK;
        }

        public override bool EnterState()
        {
            return EnteredState;
        }

        public override void UpdateState()
        {
            UnitControlledByPlayer();

            // Determine target
            // CheckIfTargetEnemyIsDead();
            DetermineTarget();
            PerformAttackAfterTime();

            FollowEnemyIfNotInRange();

        }

        public override bool ExitState()
        {
            base.ExitState();
            return true;
        }

        private void FollowEnemyIfNotInRange()
        {
            // If enemy unit is running away, follow enemy
            if (!_fighter.IsInWeaponRange())
            {
                _fsm.EnterState(StateType.FOLLOW_ENEMY);
            }
        }

        private void PerformAttackAfterTime()
        {
            if (totalTime > _fighter.timeBetweenAttakcs)
            {
                _navMeshAgent.transform.LookAt(targetEnemy.transform);
                TriggerAttack();
                targetEnemy.GetComponent<Health>().TakeDamage(_fighter.GetWeaponDamage());
                totalTime = 0f;
            }
            else
            {
                totalTime += Time.deltaTime;
            }
        }

        // Check if target enemy is dead
        private void CheckIfTargetEnemyIsDead()
        {
            if (targetEnemy.GetComponent<Health>().isDead)
            {
                targetEnemy = null;
                if (_unit.IsEnemyInRange())
                {
                    targetEnemy = _unit.closestEnemy.transform;
                }
                else
                {
                    _fsm.EnterState(StateType.IDLE);
                }
            }
        }

        private void DetermineTarget()
        {
            if (_fighter.targetEnemy != null && !_fighter.targetEnemy.GetComponent<Health>().isDead)
            {
                targetEnemy = _fighter.targetEnemy.transform;
            }
            else
            {
                targetEnemy = _unit.closestEnemy.transform;
            }
        }

        private void TriggerAttack()
        {
            _unit.GetComponent<Animator>().ResetTrigger("stopAttack");
            _unit.GetComponent<Animator>().SetTrigger("attack");
        }
    }
}
