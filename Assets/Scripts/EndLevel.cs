using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour {

    // Use this for initialization
    private GameManager GameManager;
    void Start () {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.Player)
        {
           Text BodoviText = GameObject.Find("Bodovi").GetComponent<Text>();
            GameManager.NextLevel(int.Parse(BodoviText.text.Trim()));
        }
       
    }
}
