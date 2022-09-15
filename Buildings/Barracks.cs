using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitComponents;

namespace Buildings
{

    public class Barracks : Building
    {
        [SerializeField] Slider slider;
        [SerializeField] RawImage fill;

        [SerializeField] Unit unitPrefab = null;
        [SerializeField] int unitCost = 4;

        // List of already spawned units
        [SerializeField] private float recruitTimeDuration = 5f;
        float recruitTime;

        private bool isRecruitProcess;

        void Awake()
        {
            buildingType = BuildingType.BARRACKS;
        }

        void Start()
        {
            isRecruitProcess = false;
            recruitTime = 0f;
            SetTimer();
        }

        void Update()
        {
            if (isRecruitProcess)
            {
                recruitTime += Time.deltaTime;
                ProcessTimer();
            }

            if (recruitTime >= recruitTimeDuration)
            {
                SpawnUnit();
                isRecruitProcess = false;
                recruitTime = 0f;
                SetTimer();
            }
        }

        private void RecruitUnit()
        {
            if (resourceController.GetNumberOfMeat() >= unitCost && !isRecruitProcess)
            {
                resourceController.DecreaseNumberOfMeatBy(unitCost);
                isRecruitProcess = true;
            }
        }

        private void SpawnUnit()
        {
            Unit unit = Instantiate(unitPrefab, assemblyPoint.position, assemblyPoint.rotation);
            resourceController.playerUnitList.Add(unit);
        }

        // Shot the raycast from the camera
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public void SetTimer()
        {
            slider.maxValue = recruitTimeDuration;
            slider.value = recruitTime;
        }

        public void ProcessTimer()
        {
            slider.value = recruitTime;
        }
    }
}