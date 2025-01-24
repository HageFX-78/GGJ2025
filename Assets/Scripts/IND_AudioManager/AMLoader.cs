using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Audio Manager Loader")]
public class AMLoader : MonoBehaviour
{    
    [SerializeField] public AudioManager amPrefab;

    [TextArea(10,1000)]
    public string Comment = "Information Here.";
    void Awake()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(amPrefab);
        }
    }
}
