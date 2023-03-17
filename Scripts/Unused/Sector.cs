using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    private bool focus;
    public float doubleClickTimer = 0.5f;
    public AudioSource SectorAudio;
    public AudioClip SectorClip;

    public CameraControl cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        focus = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // this object was clicked - do something
    void OnMouseDown()
    {
        if (focus != true)
        {
            focus = true;
        }
        
        Debug.Log("Mouse clicked");
       // StartCoroutine(ClickEvent());
        SectorAudio.clip = SectorClip;
        SectorAudio.Play();
    }

    /*
    private IEnumerator ClickEvent()
    {
        //pause a frame so you don't pick up the same mouse down event.
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < doubleClickTimer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = transform.position;
                //cameraScript.StartCoroutine(cameraScript.CameraJump(pos, 50 , -50));
                yield break;
            }
            count += Time.deltaTime;// increment counter by change in time between frames
            yield return null; // wait for the next frame
        }
    }
    */
}
