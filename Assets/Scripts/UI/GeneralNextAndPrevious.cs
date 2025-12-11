using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// General Next and Previous Script

public class GeneralNextAndPrevious : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private GameObject[] objectArray;
    [SerializeField] private int currentLevelIndex = 0;

    void Start()
    {
        foreach(GameObject obj in objectArray)
        {
            obj.SetActive(false);
        }        
        objectArray[currentLevelIndex].SetActive(true);
    }

    public void Increment()
    {
        objectArray[currentLevelIndex].SetActive(false);
        
        if (objectArray != null && objectArray.Length > 0)
        {
            currentLevelIndex = (currentLevelIndex + 1) % objectArray.Length;
        }

        objectArray[currentLevelIndex].SetActive(true);
    }
    
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
