using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBlocker : MonoBehaviour
{
    [SerializeField] GameObject blockerPrefab;
    GameObject blockerInstance;
    int numberOfUnitsInArea = 0;

    // Start is called before the first frame update
    void Start()
    {
        blockerInstance = Instantiate(blockerPrefab, this.transform.position, Quaternion.Euler(0, 0, 0), this.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Predator>())
        {
            numberOfUnitsInArea++;
            if (numberOfUnitsInArea > 0)
            {
                blockerInstance.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Predator>())
        {
            numberOfUnitsInArea--;
            if (numberOfUnitsInArea == 0)
            {
                blockerInstance.SetActive(true);
            }
        }
    }
}
