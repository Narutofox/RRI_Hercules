using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour {
    private Text BodoviText;
    private int Bodovi;
    public int Value = 1;
    private void Start()
    {
        BodoviText = GameObject.Find("Bodovi").GetComponent<Text>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.Player)
        {
            Bodovi = int.Parse(BodoviText.text.Trim());
            Bodovi+= Value;
            BodoviText.text = Bodovi.ToString();
            Destroy(gameObject);
        }
    }
}
