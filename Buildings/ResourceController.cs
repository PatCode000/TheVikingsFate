using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnitComponents;

namespace Buildings
{
    public class ResourceController : MonoBehaviour
    {
        public List<Unit> playerUnitList = new List<Unit>();

        public TMP_Text sheepText;
        public TMP_Text meatText;

        [SerializeField] private int numberOfSheeps;
        [SerializeField] private int numberOfMeat;

        // Start is called before the first frame update
        void Start()
        {
            numberOfSheeps = 0;
            numberOfMeat = 0;
        }

        public int GetNumberOfSheeps()
        {
            return numberOfSheeps;
        }

        public int GetNumberOfMeat()
        {
            return numberOfMeat;
        }

        public void IncreaseNumberOfSheeps()
        {
            numberOfSheeps++;
            sheepText.text = numberOfSheeps.ToString();
        }

        public void DecreaseNumberOfSheeps()
        {
            numberOfSheeps--;
            sheepText.text = numberOfSheeps.ToString();
        }

        public void IncreaseNumberOfMeat()
        {
            numberOfMeat += 2;
            meatText.text = numberOfMeat.ToString();
        }

        public void IncreaseNumberOfMeatBy(int meat)
        {
            numberOfMeat += meat;
            meatText.text = numberOfMeat.ToString();
        }

        public void DecreaseNumberOfMeatBy(int meat)
        {
            numberOfMeat -= meat;
            meatText.text = numberOfMeat.ToString();
        }

        public List<Unit> GetUnitList()
        {
            return playerUnitList;
        }
    }
}
