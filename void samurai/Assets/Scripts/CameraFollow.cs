using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float Camspeed;
    public float minX,maxX,minY,maxY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(target!=null){
            Vector2 newCamP = Vector2.Lerp(transform.position,target.position,Time.deltaTime*Camspeed);
            float ClampX = Mathf.Clamp(newCamP.x,minX,maxX);
            float ClampY = Mathf.Clamp(newCamP.y,minY,maxY);
            transform.position = new Vector3(ClampX,ClampY,-10f);
        }
    }
}
