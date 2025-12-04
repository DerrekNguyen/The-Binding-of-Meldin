using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// General Next and Previous Script

public class GeneralNextAndPrevious : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private GameObject[] objectArray;
    [SerializeField] private int currentLevelIndex = 0;

    // Start
    void Start()
    {
        foreach(GameObject obj in objectArray)
        {
            obj.SetActive(false);
        }        
        objectArray[currentLevelIndex].SetActive(true);
    }

    // Increment up the array
    public void Increment()
    {
        objectArray[currentLevelIndex].SetActive(false);
        
        if (objectArray != null && objectArray.Length > 0)
        {
            currentLevelIndex = (currentLevelIndex + 1) % objectArray.Length;
        }

        objectArray[currentLevelIndex].SetActive(true);
    }
    
    // Decrement down the array
    public void Decrement()
    {
        objectArray[currentLevelIndex].SetActive(false);

        if (objectArray != null && objectArray.Length > 0)
        {
            currentLevelIndex = (currentLevelIndex - 1 + objectArray.Length) % objectArray.Length;
        }

        objectArray[currentLevelIndex].SetActive(true);
    }

}
