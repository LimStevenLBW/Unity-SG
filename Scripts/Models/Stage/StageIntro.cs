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

    public TextMeshProUGUI stageNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(DeckDataStore playerDeck, DeckDataStore enemyDeck)
    {
        playerCommanderCount.SetText(playerDeck.GetCommanderCount().ToString() + " Commanders");
        enemyCommanderCount.SetText(enemyDeck.GetCommanderCount().ToString()+ " Commanders");

        playerTroopCount.SetText(playerDeck.GetTroopSize().ToString()+ " Total Troops");
        enemyTroopCount.SetText(enemyDeck.GetTroopSize().ToString()+ " Total Troops");

        if (Director.Instance.stageNum == 3) stageNum.SetText("Final Stage");
        else
        {
            stageNum.SetText("Stage " + Director.Instance.stageNum.ToString());
        }
      
    }

    public void GetDeck()
    {

    }
}
