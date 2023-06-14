using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBlack : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter()
    {
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        animator.SetTrigger("exit");
    }

    public void ResetPosition()
    {
        gameObject.SetActive(false);
    }
}
