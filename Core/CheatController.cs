using UnityEngine;
using Buildings;

namespace Core
{
    public class CheatController : MonoBehaviour
    {
        public ResourceController resourceController;



        private void Update()
        {
            if (Input.GetKeyDown("m"))
            {
                resourceController.IncreaseNumberOfMeatBy(50);
            }

            if (Input.GetKeyDown("n"))
            {
                resourceController.IncreaseNumberOfSheeps();
            }
        }
    }


}