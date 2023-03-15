using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using States;

namespace GameSystems
{
    /*
     * Utility Class with functions for saving and loading savedata
     */
    internal class SaveDataManager
    {
        private static string path = Application.persistentDataPath + "/player.save";
        private static BinaryFormatter formatter = new BinaryFormatter();

        /* Builds a new save file using supplied data from various classes*/
        public void SaveGame(GameState gameState)
        {
            //Build a new Savedata
            FileStream fstream = new FileStream(path, FileMode.Create);

            try
            {
                formatter.Serialize(fstream, gameState);
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }
            finally
            {
                fstream.Close();
            }

        }

        /*
         * Returns a serializable instance of SaveData class
         */
        public GameState LoadGame()
        {
            GameState gameState = new GameState();
            FileStream fstream = new FileStream(path, FileMode.Open);
            Debug.Log("Loading Save from" + Application.persistentDataPath);

            try
            {
                if (File.Exists(path))
                {
                    gameState = (GameState)formatter.Deserialize(fstream);
                }
                else
                {
                    gameState = null;
                    Debug.Log("File not found");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.StackTrace);
            }
            finally
            {
                fstream.Close();
            }

            return gameState;

        }
    }

}
