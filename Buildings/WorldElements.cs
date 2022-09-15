using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public enum BuildingType
    {
        BARRACKS,
        FARM,
        BUTCHERY,
    }

    public class WorldElements : MonoBehaviour
    {
        [SerializeField] List<Building> validBuildings;
        public Dictionary<BuildingType, Building> buildings;

        void OnEnable()
        {
            buildings = new Dictionary<BuildingType, Building>();

            foreach (Building building in validBuildings)
            {
                buildings.Add(building.buildingType, building);
            }
        }
    }
}