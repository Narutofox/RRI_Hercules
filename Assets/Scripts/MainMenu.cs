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
    public PauseMenu Menu;
    public HUDCanvas HUDCanvas;
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

        YouDied = GameObject.Find("YouDiedText").GetComponent<Text>();
        if (GameManager.PlayerDied)
        {
            YouDied.enabled = true;
        }
        else
        {
            YouDied.enabled = false;
        }

        if (HealthCanvas != null)
        {
            Destroy(HealthCanvas);
        }
    }
    public void StartGame()
    {
        if (!string.IsNullOrEmpty(PlayerName.text))
        {
            PersistanceManager.DeleteSaveFile();
            GameManager.PlayerName = PlayerName.text;
            Menu.enabled = true;
            GameManager.LoadLevel("S1");
        }
    }

    public void LoadGame()
    {
        SaveGameFile SaveFile = PersistanceManager.LoadGame();
        PlayerName.text = SaveFile.PlayerName;
        GameManager.PlayerName = SaveFile.PlayerName;
        Menu.enabled = true;
        GameManager.LoadLevel(SaveFile.Level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
