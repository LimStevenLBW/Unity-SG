using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CenterPrompt : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI prompt;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayPrompt(int playerCardSelect)
    {
        gameObject.SetActive(true);
        prompt.SetText("Select " + playerCardSelect + " cards to play");
    }

    public void ResetText()
    {
        prompt.SetText("");
    }
}
