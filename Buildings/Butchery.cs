using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class Butchery : Building
    {
        [SerializeField] Slider slider;
        [SerializeField] RawImage fill;

        private int sheepNumber = 0;

        [SerializeField] private float processTimeDuration = 5f;
        float processTime;

        private bool isSheepProcess;
        // Start is called before the first frame update

        void Awake()
        {
            buildingType = BuildingType.BUTCHERY;
        }

        void Start()
        {
            isSheepProcess = false;
            processTime = 0f;
            SetTimer();
        }

        // Update is called once per frame
        void Update()
        {
            if (sheepNumber > 0)
            {
                if (isSheepProcess)
                {
                    processTime += Time.deltaTime;
                    ProcessTimer();
                }
                else
                {
                    DecreaseNumberOfSheeps();
                }

                if (processTime > processTimeDuration)
                {
                    ProcessSheep();
                    SetTimer();
                }
            }
        }

        void ProcessSheep()
        {
            resourceController.IncreaseNumberOfMeat();
            processTime = 0f;
            isSheepProcess = false;
        }

        public void IncreaseNumberOfSheeps()
        {
            sheepNumber++;
            isSheepProcess = true;
        }

        public void DecreaseNumberOfSheeps()
        {
            sheepNumber--;
        }

        public void SetTimer()
        {
            slider.maxValue = processTimeDuration;
            slider.value = processTime;
        }

        public void ProcessTimer()
        {
            slider.value = processTime;
        }
    }
}
