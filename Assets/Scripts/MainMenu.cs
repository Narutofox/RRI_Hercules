using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    Button LoadGameButton;
    Text YouDied;
    private GameManager GameManager;
    public InputField PlayerName;
    void Start () {
        LoadGameButton = GameObject.FindGameObjectWithTag("btnLoadGame").GetComponent<Button>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (PersistanceManager.SaveFileExists())
        {
            LoadGameButton.interactable = true;
        }
        else
        {
            LoadGameButton.interactable = false;
        }

        YouDied = GameObject.Find("YouDiedText").GetComponent<Text>();
        if (GameManager.PlayerDied)
        {
            YouDied.enabled = true;
        }
        else
        {
            YouDied.enabled = false;
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
       else
        {
           
        }
    }

    public void LoadGame()
    {
        SaveGameFile SaveFile = PersistanceManager.LoadGame();
        PlayerName.text = SaveFile.PlayerName;
        GameManager.PlayerName = SaveFile.PlayerName;
        GameManager.LoadLevel(SaveFile.Level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
