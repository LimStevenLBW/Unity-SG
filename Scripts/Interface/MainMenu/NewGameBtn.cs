using UnityEngine;
using Buttons;
using UnityEngine.EventSystems;

internal sealed class NewGameBtn : Button
{
    public override void OnPointerClick(PointerEventData e)
    {
        //Call Super to play attached audio clip
        base.OnPointerClick(e);
        Debug.Log("LoadButton");
        //SaveableData savedata = SaveDataManager.LoadGame();
        // EventManager.TriggerDataLoaded(savedata);
    }
}

