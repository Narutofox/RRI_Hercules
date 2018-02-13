using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistanceManager  {

    private static string SaveGamePath { get { return Application.persistentDataPath + "/SaveGame.save"; }  }
    private static string HighScoreSavePath { get { return Application.persistentDataPath + "/HighScore.bin"; } }
    public static void SaveGame()
    {
       Transform PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerAttack PlayerAttackScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        PlayerHealth PlayerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        Text BodoviText = GameObject.Find("Bodovi").GetComponent<Text>();
        SaveGameFile Save = new SaveGameFile
        { x = PlayerTransform.position.x,
            y = PlayerTransform.position.y,
            PlayerMaxHealth = (int)PlayerHealthScript.HealthSlider.maxValue,
            PlayerHealth = (int)PlayerHealthScript.HealthSlider.value,
            NumberOfFireballShots = PlayerAttackScript.NumberOfFireballShots,
            NumberOfInvincibleSeconds = PlayerAttackScript.NumberOfInvincibleSeconds,
            NumberOfLightningSeconds = PlayerAttackScript.NumberOfLightningSeconds,
            Level = SceneManager.GetActiveScene().name,
            Points = int.Parse(BodoviText.text.Trim()),
            PlayerName = GameManager.PlayerName
        };

        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(SaveGamePath))
        {
            File.Delete(SaveGamePath);
        }
        FileStream file = File.Create(SaveGamePath);
        string SaveJSON = JsonUtility.ToJson(Save);
        bf.Serialize(file, SaveJSON);
        file.Close();

        
    }

    public static SaveGameFile LoadGame()
    {
        if (SaveFileExists())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SaveGamePath, FileMode.Open);
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
            File.Delete(SaveGamePath);
        }
    }

    public static void AddHighScore(string playerName,int scoreToAdd)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        HighScore Score = new HighScore { Name = playerName, Points = scoreToAdd };
        string SaveJSON = string.Empty;
        List<HighScore> HighScores = null;

        if (File.Exists(HighScoreSavePath))
        {         
            file = File.Open(HighScoreSavePath, FileMode.Open);
            string stringFromFile = (string)bf.Deserialize(file);
            HighScores = JsonUtility.FromJson<List<HighScore>>(stringFromFile);
        }
        else
        {
            HighScores = new List<HighScore>();
            file = File.Create(HighScoreSavePath);          
        }

        HighScores.Add(Score);
        SaveJSON = JsonUtility.ToJson(HighScores);
        bf.Serialize(file, SaveJSON);
        file.Close();
    }


   public static bool SaveFileExists()
    {
        if (File.Exists(SaveGamePath))
        {
            return true;                 
        }

       return false;
    }
}
