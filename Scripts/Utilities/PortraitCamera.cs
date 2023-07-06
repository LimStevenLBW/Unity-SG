using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitCamera : MonoBehaviour
{
    //public UnitController unit;
    private float rotationX; //Higher values turns the camera upwards
    private Transform target;
    private Vector3 offset;
    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    void Init()
    {
        offset = target.transform.position - originalPosition; //Higher values lower the camera at the end, but dont bother messing with offset, it doesnt work too well
        BoxCollider box = target.GetComponent<BoxCollider>();
        Vector3 boxSizeConverted = target.transform.TransformVector(box.size);
       // Debug.Log("box" + boxSizeConverted.y);
       // Debug.Log("offset" + offset.y);
        //Debug.Log("tranform pos" + target.transform.position);

        float height = boxSizeConverted.y;

      

        rotationX = height - 4;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
      
            int speed = 8;
            // Look

            var newRotation = Quaternion.LookRotation(target.transform.position + new Vector3(0, rotationX, 0) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed * Time.deltaTime);

            Vector3 newPosition = target.transform.position + target.forward * 30 - target.transform.up * (offset.y);
            //Vector3 newPosition = target.transform.position - target.transform.right * offset.x - target.transform.up * offset.y;
            transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * speed);
        }
    }
 
    public void SetTargetObject(UnitController unit)
    {
        target = unit.transform;
        //transform.position = unit.transform.position + new Vector3(0, 16, 0);

       // Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, unit.transform.eulerAngles.y, transform.eulerAngles.z);

        //transform.rotation = Quaternion.Euler(eulerRotation);
          
        Init();
    }

    public Texture GetRenderTexture()
    {
        Camera camera = gameObject.GetComponent<Camera>();
        return camera.activeTexture;
    }
}
