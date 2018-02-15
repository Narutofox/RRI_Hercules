using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fireball : MonoBehaviour, IEnemyAttack{

    public Rigidbody2D rgbd;
    public GameObject fieryParticle;
    public GameObject smokeParticle;
    public GameObject explosionParticle;
    private float timeLeft = 2; // Nakon 2 sekunde se uništi

    public int Damage { get; set; }
    void Start()
    {
        Damage = 20;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            FireballHit();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Tags.Player)
        {
            FireballHit();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == Tags.Player)
    //    {
    //        FireballHit();
    //    }
    //}

    private void FireballHit()
    {

        fieryParticle.SetActive(false);
        smokeParticle.SetActive(false);
        explosionParticle.SetActive(true);
        rgbd.constraints = RigidbodyConstraints2D.FreezeAll;
        Destroy(this.gameObject);
    }
}
