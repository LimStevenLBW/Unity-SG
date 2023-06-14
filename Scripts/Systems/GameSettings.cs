using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/*
 * Global Game settings object persists across scenes. Data is saved to filesystem.
 * Alert all subscribed objects when settings are changed
 */
public class GameSettings : MonoBehaviour
{
    private int saveFileHeader = 0;
    private int MASTER_VOLUME = 3;
    private int BGM_VOLUME = 3;
    private int SFX_VOLUME = 3;

    public Action OnSettingsChanged;

    public static GameSettings Instance { get; private set; }

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

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        try
        {
            Load(); //Load Settings at the start
        }
        catch (FileNotFoundException ex)
        {
            //No user settings have been saved
            //Debug.LogException(ex, this);

        }

    }

    public int GetMasterVolume()
    {
        return MASTER_VOLUME;
    }

    public void SetMasterVolume(float volumeLevel)
    {
        MASTER_VOLUME = (int)volumeLevel;
        OnSettingsChanged?.Invoke();
    }

    public int GetBGMVolume()
    {
        return BGM_VOLUME;
    }

    public void SetBGMVolume(float volumeLevel)
    {
        BGM_VOLUME = (int)volumeLevel;
        OnSettingsChanged?.Invoke();
    }

    public void SetSFXVolume(float volumeLevel)
    {
        SFX_VOLUME = (int)volumeLevel;
        OnSettingsChanged?.Invoke();
    }
    public int GetSFXVolume()
    {
        return SFX_VOLUME;
    }

    /*
     * Saves game settings to application file system
     */
    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, "settings.sav");
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(saveFileHeader);
            writer.Write(MASTER_VOLUME);
            writer.Write(BGM_VOLUME);
            writer.Write(SFX_VOLUME);
        }
        OnSettingsChanged?.Invoke();
    }

    /*
     * Load global game settings, variables must be read in the same order as they were written
     */
    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "settings.sav");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            saveFileHeader = reader.ReadInt32();
            MASTER_VOLUME = reader.ReadInt32();
            BGM_VOLUME = reader.ReadInt32();
            SFX_VOLUME = reader.ReadInt32();
        }

        OnSettingsChanged?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
