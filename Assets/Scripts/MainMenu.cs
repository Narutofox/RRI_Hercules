using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    Button LoadGameButton; 
	void Start () {
        LoadGameButton = GameObject.FindGameObjectWithTag("Player").GetComponent<Button>();
        if (PersistanceManager.SaveFileExists())
        {
            LoadGameButton.enabled = true;
        }
        else
        {
            LoadGameButton.enabled = false;
        }

    }
    public void StartGame()
    {
        PersistanceManager.DeleteSaveFile();
        SceneManager.LoadScene("S1");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(PersistanceManager.LoadGame().Level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
