using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField]
    float Timer = 3;
    void Start()
    {
        Destroy(this.gameObject, Timer);
    }
}
