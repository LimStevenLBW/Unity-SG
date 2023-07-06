using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandLayout : MonoBehaviour
{
    [SerializeField] private List<GameObject> positions = new List<GameObject>();

    //Return the vector3 position of the desired child object
    public Vector3 GetPosition(int cardNum)
    {
        return gameObject.transform.GetChild(cardNum - 1).transform.position;
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
