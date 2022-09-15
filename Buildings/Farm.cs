using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core;

namespace Buildings
{
    public class Farm : Building
    {
        FlockingController flockingController;
        [SerializeField] int numberOfSheeps = 0;

        void Awake()
        {
            buildingType = BuildingType.FARM;
        }

        // Start is called before the first frame update
        void Start()
        {
            flockingController = FindObjectOfType<FlockingController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            AddShipToTheFarm(other);
        }

        private void AddShipToTheFarm(Collider other)
        {
            if (other.GetComponent<FlockAgent>())
            {
                resourceController.IncreaseNumberOfSheeps();
                numberOfSheeps++;
                flockingController.RemoveAgentFromList(other.gameObject.GetComponent<FlockAgent>());
                Destroy(other.gameObject);
            }
        }

        private void DecreaseNumberOfSheeps()
        {
            numberOfSheeps--;
        }

        public int GetNumberOfSheeps()
        {
            return numberOfSheeps;
        }
    }
}