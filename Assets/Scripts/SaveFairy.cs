using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFairy : MonoBehaviour {


    private Animator Anim;
    private bool CanSave;
    // Use this for initialization
    void Start () {
        Anim = GameObject.FindGameObjectWithTag("SaveFairy").GetComponent<Animator>();
        CanSave = true;
    }
	
	// Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanSave)
        {
            if (collision.gameObject.tag == Tags.Player)
            {
                PersistanceManager.SaveGame();
                Anim.SetTrigger("Save");
                CanSave = false;
            }
        }        
    }
}
