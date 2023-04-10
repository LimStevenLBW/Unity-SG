using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitSkillDisplay : MonoBehaviour
{
    public TextMeshProUGUI cdDisplay;

    private int timer;
    private Skill skill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (skill != null)
        {
            timer = (int)skill.currentCooldown;
            cdDisplay.SetText(timer.ToString());
        }
        else
        {
            cdDisplay.SetText("?");
        }
        
    }

    public void Initialize(Skill skill)
    {
        if(this.skill == null) this.skill = skill;
    }
}
