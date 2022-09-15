using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Buildings
{

    public abstract class Building : MonoBehaviour
    {
        public BuildingType buildingType;
        public Transform assemblyPoint = null;

        [SerializeField] public ResourceController resourceController;

        public Vector3 GetAssemblyPoint()
        {
            return assemblyPoint.transform.position;
        }

        private void Start()
        {
            resourceController = resourceController.GetComponent<ResourceController>();
        }
    }
}
