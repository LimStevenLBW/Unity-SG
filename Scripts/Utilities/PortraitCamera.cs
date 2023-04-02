using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitCamera : MonoBehaviour
{
    private Transform target;

    Vector3 originalTransform;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //originalTransform = transform.position;
    }

    void Init()
    {


        offset = target.transform.position - transform.position;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
      
            int speed = 8;
            // Look

            var newRotation = Quaternion.LookRotation(target.transform.position + new Vector3(0, 17, 0) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed * Time.deltaTime);

            Vector3 newPosition = target.transform.position + target.forward * 30 - target.transform.up * offset.y;

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
}
