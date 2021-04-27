using GameSystems;
using UnityEngine;

/*
 * Program Entry Point and Application Composition Root.
 * The Composition Root is where dependencies are created and injected
 */
public sealed class Root: MonoBehaviour
{
    private GameManager manager;

    private void Start()
    {
        /*
         * deprecated
         * Context is a managing data structure for entities.
         * An entity can not be created stand alone, it has to be created through context.CreateEntity(). 
         * This way a context can manage the life cycle of all entities we create. It also is the first observer which get notified when we manipulate an entity
         */
    }

    void Awake()
    {
        //If the game manager singleton does not exist, create the game manager
        if (manager == null)
        {
            Debug.Log("Manager Instantiated");
            Instantiate(manager);
        }
    }

}
