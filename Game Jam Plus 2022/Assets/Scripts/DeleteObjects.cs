using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjects : MonoBehaviour
{
    public List<GameObject> allObjects;
    public

    void Start()
    {
        if (allObjects.Count %2 != 0)
        {
            allObjects.RemoveAt(allObjects.Count - 1);
        }

        for (int i = 0; i < allObjects.Count - 1; i++)
        {
            if (i % 2 != 0)
            {
                Destroy(allObjects[i]);
            }
        }
    }

    
}
