using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents
{

    public class Fighter : MonoBehaviour
    {
        Unit unitComponent;
        [SerializeField] private float weaponRange = 3f;
        public float timeBetweenAttakcs = 1f;
        [SerializeField] private float weaponDamage = 5f;

        public GameObject targetEnemy;

        // public GameObject targetEnemy;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            unitComponent = GetComponent<Unit>();
        }

        private void Update()
        {
            if (targetEnemy != null && targetEnemy.GetComponent<Health>().isDead)
            {
                targetEnemy = null;
            }
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public bool IsInWeaponRange()
        {
            if (unitComponent.enemyInRange)
            {
                if (targetEnemy != null)
                {
                    return Vector3.Distance(transform.position, targetEnemy.transform.position) < weaponRange;
                }
                else if (unitComponent.closestEnemy != null)
                {
                    return Vector3.Distance(transform.position, unitComponent.closestEnemy.transform.position) < weaponRange;
                }
            }
            return false;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public void SetEnemyAsTarget(GameObject enemy)
        {
            if (!enemy.GetComponent<Health>().isDead)
            {
                targetEnemy = enemy;
            }
            else
            {
                targetEnemy = null;
            }
        }

        // Check if the unit which is target is in the weapon range
        public bool IsTargetEnemyInWeaponRange()
        {
            if (targetEnemy != null)
            {
                return Vector3.Distance(transform.position, targetEnemy.transform.position) < weaponRange;
            }
            return false;
        }
    }
}
