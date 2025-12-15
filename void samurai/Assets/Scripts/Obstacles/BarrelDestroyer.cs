using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDestroyer : MonoBehaviour
{

    void Start()
    {

    }

    public void BarrelDamage()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {

    }
}
