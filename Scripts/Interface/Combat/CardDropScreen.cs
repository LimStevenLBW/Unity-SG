using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDropScreen : MonoBehaviour
{
    public DropRate dropRateStage1;
    public DropRate dropRateStage2;
    public DropRate dropRateStage3;
    public DropRate dropRateStage4;
    public Card drop1;
    public Card drop2;
    public Card drop3;

    public List<Unit> dropSelection;

    public void GenerateRandomDrops()
    {
        dropSelection = new List<Unit>();
        int stageID = Director.Instance.tempCurrentStageID;

        if (stageID == 1) dropSelection = dropRateStage1.GetDropSelection(3);
        else if (stageID == 2) dropSelection = dropRateStage2.GetDropSelection(3);
        else if (stageID == 3) dropSelection = dropRateStage3.GetDropSelection(3);
        else if (stageID == 4) dropSelection = dropRateStage4.GetDropSelection(3);

        drop1.GetCard(new UnitDataStore(dropSelection[0]));
        drop2.GetCard(new UnitDataStore(dropSelection[1]));
        drop3.GetCard(new UnitDataStore(dropSelection[2]));
        
    }

    public List<Unit> GetDrops()
    {
        return dropSelection;
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
