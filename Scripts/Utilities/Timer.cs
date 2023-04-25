using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Action OnSecondPassed;
    // Start is called before the first frame update
    void Start()
    {
        //Function name, start after this amount of seconds, repeat every certain seconds
        InvokeRepeating("Tick", 0.0f, 1f);
    }

    void Tick()
    {
        OnSecondPassed?.Invoke();
    }
}
