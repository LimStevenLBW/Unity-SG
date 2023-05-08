using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Effect behaviour
 * Destroy itself if the particle system is finished
 */
public class DestroySelf : MonoBehaviour
{
    int time;
    Skill skill;
    void OnStart()
    {
       
    }

    public void SelfDestruct(int time)
    {
        this.time = time;
        StartCoroutine("CheckIfAlive");
    }
    public void SetSkill(Skill skill)
    {
        this.skill = skill;
    }
    public void CallBack()
    {
        if(skill != null) skill.EffectDestroyed();
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
                CallBack();
                Destroy(gameObject);
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                if (!ps.IsAlive(true))
                {
                   CallBack();
                   Destroy(gameObject);
                   break;
                }
            }

        }
    }
}

