using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject prefab;

    public UnitController target;
    public int speed;
    public Skill skill;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Chase());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //todo, parabola motion?
    IEnumerator Chase()
    {
        while(target == null)
        {
            yield return null;
        }

        Vector3 targetPosition = new Vector3 (target.transform.position.x, target.transform.position.y + 10, target.transform.position.z);
        //float totalDistance = Vector3.Distance(transform.position, targetPosition);

        while (Vector3.Distance(transform.position, targetPosition) > 2f)
        {
            if (target.GetState() == "DEAD") break;
            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y + 10, target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
;
            //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
           // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQ, Time.deltaTime * 3f);
            //
            //  transform.Translate(Space.World);
            //Wait a frame and repeat
            yield return null;
        }

        skill.Resolve();
        Destroy(gameObject);
    }

}
