using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistanceManager  {

    public static string SavePath { get { return Application.persistentDataPath + "/SaveGame.bin"; }  }
    public static void SaveGame()
    {
       Transform PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerAttack PlayerAttackScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        PlayerHealth PlayerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        SaveGameFile Save = new SaveGameFile { Position = PlayerTransform, PlayerHealth = PlayerHealthScript, PlayerAttack = PlayerAttackScript, Level = SceneManager.GetActiveScene().name };

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(SavePath, FileMode.OpenOrCreate);
        bf.Serialize(file, JsonUtility.ToJson(Save));
        file.Close();

        
    }

    public static SaveGameFile LoadGame()
    {
        if (SaveFileExists())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SavePath, FileMode.Open);
            string stringFromFile = (string)bf.Deserialize(file);
            file.Close();
            return JsonUtility.FromJson<SaveGameFile>(stringFromFile);
        }
        else
        {
            return null;
        }
    }
    public static void DeleteSaveFile()
    {
        if (SaveFileExists())
        {
            File.Delete(SavePath);
        }
    }


   public static bool SaveFileExists()
    {
        if (File.Exists(SavePath))
        {
            return true;
        }

       return false;
    }
}
