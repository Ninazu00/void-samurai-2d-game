using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollowCamera : MonoBehaviour
{
    public Transform cam;

    void LateUpdate()
    {
        transform.position = new Vector3(
            cam.position.x,
            cam.position.y + 10f,
            transform.position.z
        );
    }
}

