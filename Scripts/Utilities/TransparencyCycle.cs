using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * A fading effect that cycles between opaque and transparent, can be used on image or text objects
 */
public class TransparencyCycle : MonoBehaviour
{
    public byte cycleSpeed = 3;
    public byte maxVisibility = 255;
    public byte minVisibility = 50;
    private TextMeshProUGUI text;
    private Image image;
    private byte transparencyLevel = 255;
    private bool shouldFade = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        image = gameObject.GetComponent<Image>();
        StartCoroutine(Cycle());
;    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Cycle()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(0.001f);
            if (shouldFade)
            {
                transparencyLevel -= 3;
                if (text != null) text.color = new Color32(255, 255, 255, transparencyLevel);
                if( image != null) image.color = new Color32(255, 255, 255, transparencyLevel);
            }
            else
            {
                transparencyLevel += 3;
                if (text != null) text.color = new Color32(255, 255, 255, transparencyLevel);
                if (image != null) image.color = new Color32(255, 255, 255, transparencyLevel);
            }

            if (transparencyLevel >= maxVisibility) shouldFade = true;
            if (transparencyLevel <= minVisibility) shouldFade = false;
        }
     

    }
}
