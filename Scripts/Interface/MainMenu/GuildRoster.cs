using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Guild Roster manages one deck datastore's display
public class GuildRoster : MonoBehaviour
{
    //Guild Roster can be optionally connected to another list to transfer elements
    [SerializeField] private bool editable;
    [SerializeField] private GuildRoster otherRoster;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioError;
    [SerializeField] private GuildRosterContentGroup rosterContent;
   
    [SerializeField] private TextMeshProUGUI countText;
    private int count;

    private DeckDataStore deck;

    List<UnitDataStore> unitListDataStore = new List<UnitDataStore>();

    //Receive the deck and refresh the list
    public void Init(DeckDataStore deck)
    {
        this.deck = deck;

        rosterContent.Clear();
        rosterContent.Setup(deck.GetCardList());
        rosterContent.DisplayDelayed();

        UpdateCount();
    }

    public void Init(List<Unit> unitList)
    {
        unitListDataStore.Clear();
        foreach(Unit unit in unitList) unitListDataStore.Add(new UnitDataStore(unit));

        rosterContent.Clear();

        rosterContent.Setup(unitListDataStore);
        rosterContent.DisplayDelayed();

        UpdateCount();
    }

    public void Init(List<UnitDataStore> unitList)
    {
        unitListDataStore.Clear();
        unitListDataStore = unitList;

        rosterContent.Clear();

        rosterContent.Setup(unitListDataStore);
        rosterContent.DisplayDelayed();

        UpdateCount();
    }

    //Add a single unit to the list
    public void Add(UnitDataStore data)
    {
        unitListDataStore.Add(data);

        rosterContent.Clear();

        rosterContent.Setup(unitListDataStore);
        rosterContent.DisplayImmediate();

        UpdateCount();
    }

    public void Transfer(UnitDataStore data)
    {
        unitListDataStore.Remove(data);
        rosterContent.Clear();

        rosterContent.Setup(unitListDataStore);
        rosterContent.DisplayImmediate();

        otherRoster.Add(data);

        UpdateCount();
    }

    public bool GetEditableStatus()
    {
        return editable;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void AnimateReroll()
    {   
        animator.SetTrigger("rerolling");
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public int GetCount()
    {
        return count;
    }

    public void DisplayCountError()
    {
        animator.SetTrigger("error");
        audioSource.PlayOneShot(audioError);
    }

    void UpdateCount()
    {
        if(countText != null)
        {
            count = deck.GetCardList().Count;
            countText.SetText(count + "/25");

            if (count < 25 || count > 25) countText.color = Color.red;
            else countText.color = Color.white;
        }

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
