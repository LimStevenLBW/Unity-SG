using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandPanel : MonoBehaviour
{
    public Card[] cards = new Card[5];
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(Draw());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Draw()
    {
        //We draw cards from left to right 
        foreach (Card c in cards)
        {
            yield return new WaitForSeconds(.1f);
            c.gameObject.SetActive(true);
        }

    }


}
