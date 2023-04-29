using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI counter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateCount(int count)
    {
        counter.SetText(count.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
