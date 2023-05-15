using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    public int heartMax;
    public int currentHearts;
    public GameObject heartPrefab;
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private AudioClip AudioDamaged;

    // Start is called before the first frame update
    void Start()
    {
        HealMax();
    }

    void HealMax()
    {
       for(int i = 0; i < heartMax; i++)
        {
            GameObject prefab = Instantiate(heartPrefab);
            prefab.transform.SetParent(transform);
            prefab.transform.localScale = new Vector3(1, 1, 1);
        }
        currentHearts = heartMax;
    }

    //Return false if player is dead
    public bool TakeDamage(int damage)
    {
        for(int i=0; i< damage; i++)
        {
            GameObject heart = transform.GetChild(0).gameObject;
            if (heart != null)
            {
                AudioPlayer.PlayOneShot(AudioDamaged);
                Destroy(heart);
                heartMax--;

                if (heartMax <= 0) return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
