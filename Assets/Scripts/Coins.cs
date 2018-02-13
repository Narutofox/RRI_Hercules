using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour {
    private Text BodoviText;
    private int Bodovi;

    private void Start()
    {
        BodoviText = GameObject.Find("Bodovi").GetComponent<Text>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.Player)
        {
            Bodovi = int.Parse(BodoviText.text.Trim());
            Bodovi++;
            BodoviText.text = Bodovi.ToString();
            Destroy(gameObject);
        }
    }
}
