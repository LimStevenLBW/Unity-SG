using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitRoomContainer : MonoBehaviour
{
    [SerializeField] List<PortraitRoom> rooms;


    public PortraitRoom GetPortraitRoom(int cardNumber)
    {
        return rooms[cardNumber - 1];
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
