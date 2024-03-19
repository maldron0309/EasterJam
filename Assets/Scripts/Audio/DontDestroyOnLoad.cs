using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
        private static DontDestroyOnLoad instance;

    void Awake()
    {
        // Ensure that there's only one instance of this object
        if (instance == null)
        {
            // If this is the first instance, set it as the instance
            instance = this;
            // Mark this object to not be destroyed when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }
}
