using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents
{
    public enum UnitType
    {
        WORKER,
        WARRIOR,
    }

    public class Unit : MonoBehaviour
    {
        private Health health;
        public UnitType unitType;
        public List<GameObject> enemiesInRange;
        public bool enemyInRange = false;

        private float detectionRange = 20f;
        private string enemyTag = null;

        public GameObject selectedUnit;
        public bool isUnitSelected = false;

        public bool canBeControlled = true;

        public GameObject closestEnemy;

        private void Awake()
        {
            if (this.tag == "playerUnit")
            {
                selectedUnit = transform.Find("Selected").gameObject;
                selectedUnit.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            RecogniseEnemy();
        }

        // Update is called once per frame
        void Update()
        {
            enemiesInRange = GetNearbyObjects();
            enemyInRange = IsEnemyInRange();
            if (enemyInRange)
            {
                SetClosestEnemy();
            }
        }

        // Return all of the neighbours for the current flock
        List<GameObject> GetNearbyObjects()
        {
            enemiesInRange = new List<GameObject>();

            Collider[] contextColliders = Physics.OverlapSphere(transform.position, detectionRange);

            foreach (Collider c in contextColliders)
            {
                if (c.gameObject.tag == enemyTag)
                {
                    if (!c.GetComponent<Health>().isDead)
                    {
                        enemiesInRange.Add(c.gameObject);
                    }
                }
            }
            return enemiesInRange;
        }

        // Return position of the closest enemy
        public void SetClosestEnemy()
        {
            GameObject closeEnemy = null;

            float shortestDistance = 100f;

            foreach (GameObject enemy in enemiesInRange)
            {
                float tempDistance;
                tempDistance = Vector3.Distance(transform.position, enemy.transform.position);
                if (tempDistance < shortestDistance)
                {
                    closeEnemy = enemy;
                    shortestDistance = tempDistance;
                }
            }
            closestEnemy = closeEnemy;
        }

        public bool IsEnemyInRange()
        {
            if (enemiesInRange.Count > 0)
            {
                return true;
            }
            return false;
        }

        void RecogniseEnemy()
        {
            if (this.tag == "playerUnit")
            {
                enemyTag = "enemyUnit";
            }
            else
            {
                enemyTag = "playerUnit";
            }
        }

        public void SetSelectedVisible(bool visible)
        {
            if (this.tag == "playerUnit")
            {
                isUnitSelected = visible;
                selectedUnit.SetActive(visible);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }

        public int GetNumberOfEnemies()
        {
            return enemiesInRange.Count;
        }

        public void SetCanBeControlled(bool value)
        {
            canBeControlled = value;
        }
    }
}
