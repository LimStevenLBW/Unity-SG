using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardSelectOrder : MonoBehaviour
{
    public TextMeshProUGUI textNumber;


    public void UpdateOrder(int num)
    {
        textNumber.SetText(num.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
