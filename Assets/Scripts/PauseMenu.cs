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
        if (GameObject.FindGameObjectsWithTag(Tags.PauseMenu).Length > 1)
        {
            Destroy(this.gameObject);
           
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            HideButtons();
        }             
    }

    //private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{
    //    if (arg0.name == "MainMenu")
    //    {
    //        try
    //        {
    //            this.enabled = false;
    //        }
    //        catch (MissingReferenceException)
    //        {
    //        }
    //    }
    //    else
    //    {
    //        try
    //        {
    //            this.enabled = true;
    //        }
    //        catch (MissingReferenceException)
    //        {

    //        }
            
    //    }
    //}

    // Update is called once per frame
    void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1)
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                Time.timeScale = 0;
                ShowButtons();
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HideButtons();
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
