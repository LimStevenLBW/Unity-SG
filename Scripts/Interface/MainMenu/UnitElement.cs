using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private Image classImage;
    [SerializeField] private Image factionImage;
    [SerializeField] private Animator animator;

    public void GetData(UnitDataStore data)
    {
        animator = GetComponent<Animator>();
        unitName.SetText(data.unitName);
        string rankText = data.GetRank();
        rank.SetText(rankText);
        classImage.sprite = data.unitClass.icon;

        Image image = GetComponent<Image>();
        if (rankText == "D") image.color = Color.grey;
        if (rankText == "C") image.color = new Color32(139, 35, 136, 255);
        if (rankText == "B") image.color = new Color32(73, 156, 255, 255);
        if (rankText == "A") image.color = new Color32(205, 75, 0, 255);
    }

    public void Enter()
    {
        animator.SetTrigger("Enter");
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
