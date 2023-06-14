using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildRoster : MonoBehaviour
{
    [SerializeField] private Deck playerDeckBase;
    [SerializeField] private GuildRosterContentGroup gridContent;
    private DeckDataStore playerDeck;


    public void Init()
    {
        playerDeck = new DeckDataStore(playerDeckBase); //Preset currently in editor
        playerDeck.SortByClassAndRank();

        gridContent.Setup(playerDeck.unitList);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        gridContent.Display();

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
