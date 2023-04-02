using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Destroy itself if the particle system is finished
 */
public class DestroySelf : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine("CheckIfAlive");
    }

    IEnumerator CheckIfAlive()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        while (true && ps != null)
        {
            yield return new WaitForSeconds(0.5f);
            if (!ps.IsAlive(true))
            {

                GameObject.Destroy(this.gameObject);
                break;
            }
        }
    }
}

