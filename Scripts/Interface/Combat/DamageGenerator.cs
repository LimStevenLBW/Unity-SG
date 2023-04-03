using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Works with DamageTextAnimation
public class DamageGenerator : MonoBehaviour
{
    public static DamageGenerator gen;
    public GameObject prefab;

    private void Awake()
    {
        gen = this;
    }

    public void CreatePopup(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;
        Destroy(popup, 1f); //Destroy after one second
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            CreatePopup(Vector3.one, Random.Range(0, 1000).ToString(), Color.green);
        }
    }
}
