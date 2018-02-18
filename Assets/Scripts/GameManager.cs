using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static object CurrentLevel;
    public static bool PlayerDied = false;
    public static int ScoreTotal = 0;
    public static string PlayerName;
    private GameObject EnemyHealthSlider;
    public static bool GameComplete = false;
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag(Tags.GameManager).Length > 1)
        {
            Destroy(this.gameObject);            
        }
       else
        {
            DontDestroyOnLoad(this.gameObject);
            EnemyHealthSlider = GameObject.Find("EnemyHealthSlider");
            if (EnemyHealthSlider != null)
            {
                EnemyHealthSlider.SetActive(false);
            }
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (EnemyHealthSlider == null)
        {
            EnemyHealthSlider = GameObject.Find("EnemyHealthSlider");
        }

        if (arg0.name != "S3")
        {
            BossLevelEnd();
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
        CurrentLevel = level;
    }

    public void LoadLevel(string level)
    {          
        SceneManager.LoadScene(level);
        CurrentLevel = level;
        PlayerDied = false;

    }

    public void NextLevel(int score)
    {
        ScoreTotal += score;

        if (CurrentLevel is int)
        {       
            int Level = (int)CurrentLevel;
            Level++;
            if (Level <= 3 && Application.CanStreamedLevelBeLoaded(Level))
            {
                SceneManager.LoadScene(Level);
                CurrentLevel = Level;
            }
            else
            {
                CurrentLevel = 0;
                Level = 0;
                SceneManager.LoadScene(Level);              
            }
        }
        else if (CurrentLevel is string)
        {
            string Level = (string)CurrentLevel;
            switch (Level)
            {
                case "S1":                   
                    Level = "S2";
                        break;
                case "S2":
                    Level = "S3";
                    break;
                case "S3":
                    GameComplete = true;
                    PersistanceManager.AddHighScore(PlayerName, ScoreTotal);
                    Level = "MainMenu";
                    break;
                default:
                    Level = "MainMenu";
                    break;
            }

            if (Application.CanStreamedLevelBeLoaded(Level) == false)
            {
                Level = "MainMenu";
            }
            SceneManager.LoadScene(Level);
            CurrentLevel = Level;
        }
        //else
        //{
        //    string Level = "MainMenu";
        //    SceneManager.LoadScene(Level);
        //    CurrentLevel = Level;
        //}
    }

    public void PlayerDead()
    {
        PlayerDied = true;
        LoadLevel("MainMenu");
    }

    public void BossLevelStart()
    {
        EnemyHealthSlider.SetActive(true);
    }

    public void BossLevelEnd()
    {
        if (EnemyHealthSlider == null)
        {
            EnemyHealthSlider = GameObject.Find("EnemyHealthSlider");
        }

        if (EnemyHealthSlider != null)
        {
            EnemyHealthSlider.SetActive(false);
        }
        
    }
}
