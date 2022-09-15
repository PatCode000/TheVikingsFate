using UnityEngine;
using UnityEngine.UI;

namespace UnitComponents
{
    public class Health : MonoBehaviour
    {

        private Unit unit;
        [SerializeField] GameObject healthBar;
        [SerializeField] Slider slider;
        [SerializeField] RawImage fill;


        [SerializeField] float healthPoints = 100;
        [SerializeField] GameObject bloodParticle;

        GameObject blood;

        float timeToDeleteParticle = 3f;
        float particleTimer = 0f;

        public bool isDead = false;

        private void Start()
        {
            unit = GetComponent<Unit>();
            slider = GetComponentInChildren<Slider>();

            SetMaxHealth(healthPoints);
        }

        private void Update()
        {
            if (blood != null && (particleTimer >= timeToDeleteParticle))
            {
                Destroy(blood);
                particleTimer = 0f;
            }
            particleTimer += Time.deltaTime;
            if (Input.GetKeyDown("space"))
            {
                TakeDamage(10);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            // needs to be deleted after time
            // blood = Instantiate(bloodParticle, this.transform.position, Quaternion.identity);
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            SetBarHealth(healthPoints);

            if (healthPoints == 0)
            {
                Die();
            }

        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            healthBar.SetActive(false);
            unit.SetSelectedVisible(false);
            unit.SetCanBeControlled(false);
            GetComponent<Animator>().SetTrigger("die");
        }

        public void SetMaxHealth(float health)
        {
            slider.maxValue = healthPoints;
            slider.value = healthPoints;
        }

        public void SetBarHealth(float health)
        {
            slider.value = health;
        }
    }
}