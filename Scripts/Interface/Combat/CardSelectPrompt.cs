using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardSelectPrompt : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI prompt;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayPrompt(string text)
    {
        gameObject.SetActive(true);
        prompt.SetText(text);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
