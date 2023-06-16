using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildRoster : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GuildRosterContentGroup gridContent;
    [SerializeField] private DropRate dropRateTool;
    [SerializeField] private Deck playerDeckBase;
    private DeckDataStore playerDeck;


    public void Init()
    {
        if(playerDeck == null) playerDeckBase = dropRateTool.GetRandomDeck();
        playerDeck = new DeckDataStore(playerDeckBase); //Preset currently in editor
        playerDeck.SortByClassAndRank();

        gridContent.Setup(playerDeck.unitList);
        gridContent.Display();

        GamePersistentData.Instance.SetPlayerDeck(playerDeckBase);
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

    public void Reroll()
    {
        gridContent.Clear();
        playerDeckBase = dropRateTool.GetRandomDeck();
        animator.SetTrigger("rerolling");
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
