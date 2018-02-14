using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour {

    public GameObject Continue;
    public GameObject ReturnToMainMenu;
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
        HideButtons();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            ShowButtons();
        }
    }

    private void ShowButtons()
    {
        Continue.SetActive(true);
        ReturnToMainMenu.SetActive(true);
 
    }

    private void HideButtons()
    {
        Continue.SetActive(false);
        ReturnToMainMenu.SetActive(false);   
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        HideButtons();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        HideButtons();
        SceneManager.LoadScene("MainMenu");
    }
}
