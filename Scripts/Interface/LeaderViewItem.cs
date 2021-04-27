using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderViewItem : MonoBehaviour
{
    public GameObject Name;
    public GameObject TroopCount;
    public GameObject Status;
    public GameObject Portrait;

    public void SetName(string _Name)
    {
        Name.GetComponent<TextMeshProUGUI>().SetText(_Name);
    }
    public void SetTroopCount(int _TroopCount)
    {
        TroopCount.GetComponent<TextMeshProUGUI>().SetText("" + _TroopCount+ "/" + _TroopCount);
    }
    public void SetStatus(string _Status)
    {
        Status.GetComponent<TextMeshProUGUI>().SetText(_Status);
    }
    public void SetImage(Sprite sprite)
    {
        Portrait.GetComponent<Image>().sprite = sprite;
    }
}
