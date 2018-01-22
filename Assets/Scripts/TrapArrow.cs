using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour, IEnemyAttack
{
    public int Damage {get; set;}
    private SpriteRenderer Sprite;
    // Use this for initialization
    void Start () {
        Damage = 10;
        Sprite = this.GetComponent<SpriteRenderer>();
        Color tmp = Sprite.color;
        tmp.a = 0f;
        Sprite.color = tmp;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
