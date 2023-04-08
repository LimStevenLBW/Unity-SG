using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Destroy itself if the particle system is finished
 */
public class DestroySelf : MonoBehaviour
{
    int time;
    void OnStart()
    {
       
    }

    public void SelfDestruct(int time)
    {
        this.time = time;
        StartCoroutine("CheckIfAlive");
    }

    //Destroy this object after its animation finishes, or after a certain amount of time
    IEnumerator CheckIfAlive()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        while (ps != null)
        {

            if (time > 0)
            {
                yield return new WaitForSeconds(time);
                Destroy(this.gameObject);
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                if (!ps.IsAlive(true))
                {

                   Destroy(this.gameObject);
                   break;
                }
            }

        }
    }
}

