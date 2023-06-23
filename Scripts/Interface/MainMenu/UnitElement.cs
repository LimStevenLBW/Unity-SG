using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private Image classImage;
    [SerializeField] private Image factionImage;
    [SerializeField] private Color32 elementColor;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip hoverSFX;
    [SerializeField] private AudioClip transferSFX;

    private GuildRoster guildRoster;
    private UnitDataStore data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        image.color = new Color32(70, 70, 70, 255);
       // guildRoster.PlaySound(hoverSFX);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        image.color = elementColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(guildRoster.GetEditableStatus()){
            guildRoster.PlaySound(transferSFX);
            guildRoster.Transfer(data);}
        }

    public void GetData(UnitDataStore data, GuildRoster guildRoster)
    {
        this.guildRoster = guildRoster;
        this.data = data;

        animator = GetComponent<Animator>();
        unitName.SetText(data.unitName);
        string rankText = data.GetRank();
        rank.SetText(rankText);
        classImage.sprite = data.unitClass.icon;

        Image image = GetComponent<Image>();
        if (rankText == "D") elementColor = Color.grey;
        if (rankText == "C") elementColor = new Color32(139, 35, 136, 255);
        if (rankText == "B") elementColor = new Color32(73, 156, 255, 255);
        if (rankText == "A") elementColor = new Color32(205, 75, 0, 255);

        image.color = elementColor;
    }

    public void Enter()
    {
        animator.SetTrigger("Enter");
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
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
