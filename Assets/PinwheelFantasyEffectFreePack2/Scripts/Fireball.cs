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
    public Color FireballColor;
    public bool DestroyOnGroundHit;
    public bool GenerateItemsOnHit;
    public GameObject[] PickUps;
    public int Damage { get; set; }

    public bool IsAttacking { get; set; }

    public bool CanAttack{ get; set; }

    void Start()
    {
        Damage = 20;
        if (FireballColor.r == 0 && FireballColor.g == 0 && FireballColor.b == 0)
        {
            FireballColor.r = 255;
            FireballColor.g = 153;
            FireballColor.b = 0;
            FireballColor.a = 96;
        }

        ParticleSystem.MainModule Main = fieryParticle.GetComponent<ParticleSystem>().main;
        Main.startColor = new ParticleSystem.MinMaxGradient(FireballColor);
        IsAttacking = true;
        CanAttack = true;
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
        if (collision.gameObject.tag == Tags.Player || (DestroyOnGroundHit && collision.gameObject.layer == LayerMask.NameToLayer("Ground")))
        {
            FireballHit();
        }
    }


    private void FireballHit()
    {

        fieryParticle.SetActive(false);
        smokeParticle.SetActive(false);
        explosionParticle.SetActive(true);
        rgbd.constraints = RigidbodyConstraints2D.FreezeAll;
        if (GenerateItemsOnHit)
        {
            int i = UnityEngine.Random.Range(0, PickUps.Length-1);
            Instantiate(PickUps[i], new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
        }

        Destroy(this.gameObject);
    }
}
