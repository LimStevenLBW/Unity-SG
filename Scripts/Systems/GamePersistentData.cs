using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/*
 * Global Game settings object persists across scenes. Data is saved to filesystem.
 * Alert all subscribed objects when settings are changed
 */
public class GamePersistentData : MonoBehaviour
{
    //We store just deck, because we work with copies when going into combat
    private Deck arcadeDeck;
    public Deck testDeck;


    //Set and save arcade deck
    public void SetArcadeDeck(Deck deck)
    {
        arcadeDeck = deck;
    }

    public void SetArcadeDeck(DeckDataStore deck)
    {
        arcadeDeck = Deck.CreateInstance(deck);
    }

    public Deck GetArcadeDeck() {
        if(arcadeDeck == null)
        {
            //If null, use the test deck
            Debug.Log("Using Test Deck");
            arcadeDeck = testDeck;
        }
        return arcadeDeck;
    }

    public static GamePersistentData Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }



    /*
     * Saves game settings to application file system
     */
    public void Save()
    {
        /*
        string path = Path.Combine(Application.persistentDataPath, "settings.sav");
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(saveFileHeader);
            writer.Write(MASTER_VOLUME);
            writer.Write(BGM_VOLUME);
            writer.Write(SFX_VOLUME);
        }
        OnSettingsChanged?.Invoke();
        */
    }

    /*
     * Load global game settings, variables must be read in the same order as they were written
     */
    public void Load()
    {
        /*
        string path = Path.Combine(Application.persistentDataPath, "settings.sav");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            saveFileHeader = reader.ReadInt32();
            MASTER_VOLUME = reader.ReadInt32();
            BGM_VOLUME = reader.ReadInt32();
            SFX_VOLUME = reader.ReadInt32();
        }

        OnSettingsChanged?.Invoke();
        */
    }

    // Update is called once per frame
    void Update()
    {

    }


}