using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    private int hearts;
    public GameObject heartPrefab;
    public bool isPlayerHealth; //Determines who owns the heartbar

    private Animator animator;
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioDamaged;

    // Start is called before the first frame update
    void Start()
    {
      
        animator = GetComponent<Animator>();
    }

    public void HealMax()
    {
       for(int i = 0; i < hearts; i++)
        {
            GameObject prefab = Instantiate(heartPrefab);
            prefab.transform.SetParent(transform);
            prefab.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //Return false if player is dead
    public void TakeDamage(int damage)
    {
        StartCoroutine(AnimateDamage(damage));
    }

    IEnumerator AnimateDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            GameObject heart = transform.GetChild(0).gameObject;

            if (heart != null)
            {
                animator.SetTrigger("isDamaged");
                AudioPlayer.PlayOneShot(AudioDamaged);
                yield return new WaitForSeconds(0.25f);
                Destroy(heart);

                hearts--;

                if (hearts <= 0) break;
                yield return new WaitForSeconds(0.1f);

            }
        }

        Director.Instance.UpdateHealth(isPlayerHealth, hearts);
    }

    public void SetHearts(int hearts)
    {
        this.hearts = hearts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
