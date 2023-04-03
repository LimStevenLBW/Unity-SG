using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve sizeCurve;
    public AnimationCurve heightCurve;

    private Vector3 origin;
    private TextMeshProUGUI tmp;
    private float time = 0;

    void Awake()
    {
        //Slightly vary the starting origin
        float transformXModified = Random.Range(transform.position.x - 5, transform.position.x + 5);
        float transformYModified = Random.Range(transform.position.y - 5, transform.position.y + 4);
        transform.position = new Vector3(transformXModified, transformYModified, transform.position.z);
        origin = transform.position;
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
      

        
       // float originYModifier = Random.Range(origin.y - 1, origin.y + 1);

      // origin.y += originYModifier;
    }

    // Update is called once per frame
    void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * sizeCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time))
;       time += Time.deltaTime;
    }
}
