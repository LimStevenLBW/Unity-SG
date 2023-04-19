using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataStore : MonoBehaviour
{
    //Stage type?
    public Stage stage;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = stage.icon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSceneData()
    {
        //If mob stage, send enemy deck to Director
        Director.Instance.GetEnemyDeck(stage.enemyDeck);
    }
}
