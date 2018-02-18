using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    Button LoadGameButton;
    Text MsgText;
    private GameManager GameManager;
    public InputField PlayerName;
    public HUDCanvas HUDCanvas;
    public RectTransform ScrollContent;
    public GameObject HighScoreTextObject;
    void Start () {
        LoadGameButton = GameObject.FindGameObjectWithTag("btnLoadGame").GetComponent<Button>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject HealthCanvas = GameObject.Find("HUDCanvas");
        if (PersistanceManager.SaveFileExists())
        {
            LoadGameButton.interactable = true;
        }
        else
        {
            LoadGameButton.interactable = false;
        }

        MsgText = GameObject.Find("MsgText").GetComponent<Text>();
        if (GameManager.PlayerDied)
        {
            MsgText.text = "You died. Sry :( Try again.";
            MsgText.enabled = true;
            MsgText.color = Color.red;
        }
        else if (GameManager.GameComplete)
        {
            MsgText.text = "Congratualtions! Check high scores.";
            MsgText.color = Color.green;
            MsgText.enabled = true;
        }
        else
        {
            MsgText.enabled = false;
        }

        if (HealthCanvas != null)
        {
            Destroy(HealthCanvas);
        }

        ShowHighScores();
    }

    private void ShowHighScores()
    {
        List<HighScore> HighScores = PersistanceManager.GetHighScores();
        if (HighScores.Count > 0)
        {
            foreach (var highScore in HighScores.OrderByDescending(x => x.Points))
            {
                GameObject newText = (GameObject)Instantiate(HighScoreTextObject);
                newText.transform.SetParent(ScrollContent);
                newText.GetComponent<Text>().text = highScore.Name + " - " + highScore.Points;
            }
        }
       else
        {
            GameObject.Find("HighScoresPanel").SetActive(false);
        }
    }

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(PlayerName.text))
        {
            PersistanceManager.DeleteSaveFile();
            GameManager.PlayerName = PlayerName.text;
            GameManager.LoadLevel("S1");
        }
    }

    public void LoadGame()
    {
        SaveGameFile SaveFile = PersistanceManager.LoadGame();
        PlayerName.text = SaveFile.PlayerName;
        GameManager.PlayerName = SaveFile.PlayerName;
        GameManager.ScoreTotal = SaveFile.TotalPoints;
       
        GameManager.LoadLevel(SaveFile.Level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
