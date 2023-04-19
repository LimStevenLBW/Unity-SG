using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteOptionsDisplay : MonoBehaviour
{
    public StageDataStore option1;
    public StageDataStore option2;
    public StageDataStore option3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * If there is only one option, we just take that option, otherwise await the user input
     */
    public void NextStage()
    {
        if(option2 == null && option3 == null)
        {
            option1.InitSceneData();
        }
        else
        {
            DisplayStageOptions();
        }
    }

    void DisplayStageOptions()
    {

    }
}
