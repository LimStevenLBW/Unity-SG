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
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
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
