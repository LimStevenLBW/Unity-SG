using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopLeftPrompt : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI prompt;
    public void DisplayPrompt()
    {
        prompt.SetText("Select 3 cards to play");
        if (Director.Instance.GetPhase() == "CARDSELECT")
        {
           

        }
    }

    public void ResetText()
    {
        prompt.SetText("");
    }
}
