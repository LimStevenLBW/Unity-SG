using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Guild Roster works with UnitDataStores but can still receive List<Unit>
public class GuildRoster : MonoBehaviour
{
    //Guild Roster can be optionally connected to another list to transfer elemenets
    [SerializeField] private bool editable;
    [SerializeField] private GuildRoster otherRoster;

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioError;
    [SerializeField] private GuildRosterContentGroup gridContent;
   
    [SerializeField] private TextMeshProUGUI countText;
    private int count;

    List<UnitDataStore> unitListDataStore = new List<UnitDataStore>();

    public void Init(List<Unit> unitList)
    {
        unitListDataStore.Clear();
        foreach(Unit unit in unitList) unitListDataStore.Add(new UnitDataStore(unit));

        gridContent.Clear();

        gridContent.Setup(unitListDataStore);
        gridContent.DisplayDelayed();

        UpdateCount();
    }

    public void Init(List<UnitDataStore> unitList)
    {
        unitListDataStore.Clear();
        unitListDataStore = unitList;

        gridContent.Clear();

        gridContent.Setup(unitListDataStore);
        gridContent.DisplayDelayed();

        UpdateCount();
    }

    //Add a single unit to the list
    public void Add(UnitDataStore data)
    {
        unitListDataStore.Add(data);

        gridContent.Clear();

        gridContent.Setup(unitListDataStore);
        gridContent.DisplayImmediate();

        UpdateCount();
    }

    public void Transfer(UnitDataStore data)
    {
        unitListDataStore.Remove(data);
        gridContent.Clear();

        gridContent.Setup(unitListDataStore);
        gridContent.DisplayImmediate();

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
            count = unitListDataStore.Count;
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
