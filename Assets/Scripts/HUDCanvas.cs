using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDCanvas : MonoBehaviour {
    public Text BodoviVrijednost;
    public Slider HealthSlider;

    private int NextLevelHealAmount = 10;
    void Start () {
        if (GameObject.FindGameObjectsWithTag(Tags.HUDCanvas).Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        
    }


    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    { 
            BodoviVrijednost.text = "0";
            if (HealthSlider.value + NextLevelHealAmount <= HealthSlider.maxValue)
            {
                HealthSlider.value += NextLevelHealAmount;
            }               
    }
}
