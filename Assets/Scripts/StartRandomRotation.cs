using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRandomRotation : MonoBehaviour
{
    
    void Start()
    {
        transform.Rotate(0, 0, Random.Range(-360, 360));
    }

}
