using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageIntro : MonoBehaviour
{
    public TextMeshProUGUI playerCommanderCount;
    public TextMeshProUGUI enemyCommanderCount;

    public TextMeshProUGUI playerTroopCount;
    public TextMeshProUGUI enemyTroopCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitFields(Deck playerDeck, Deck enemyDeck)
    {
        playerCommanderCount.SetText(playerDeck.GetCommanderCount().ToString() + " Commanders");
        enemyCommanderCount.SetText(enemyDeck.GetCommanderCount().ToString()+ " Commanders");

        playerTroopCount.SetText(playerDeck.GetTroopSize().ToString()+ " Total Troops");
        enemyTroopCount.SetText(enemyDeck.GetTroopSize().ToString()+ " Total Troops");
    }

    public void GetDeck()
    {

    }
}
