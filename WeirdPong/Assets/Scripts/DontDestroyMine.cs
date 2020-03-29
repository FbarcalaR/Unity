using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMine : MonoBehaviour
{
    public static DontDestroyMine instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
