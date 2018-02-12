using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour,IEnemyAttack {

    private Rigidbody2D RB2D;
    private float shootForce = 20f;
    private SpriteRenderer Sprite;
    private float timeLeft = 10; // Nakon 10 sekundi se uništi
    private Enemy EnemyScript;
    private bool? ShootDirectionRight = null;
    public int Damage { get; set; }

    void Start () {
        
        RB2D = GameObject.Find(gameObject.name).GetComponent<Rigidbody2D>();
    }
    public void Init(GameObject enemy, int damage)
    {
        EnemyScript = enemy.GetComponent<Enemy>();
        gameObject.name = "Arrow " + Guid.NewGuid().ToString();
        Damage = damage;
    }

        // Update is called once per frame
    void Update () {
        if (RB2D != null && EnemyScript != null)
        {
            if (ShootDirectionRight == null)
            {
                if (EnemyScript.facingRight)
                {
                    Sprite = GameObject.Find(gameObject.name).GetComponent<SpriteRenderer>();
                    Sprite.flipX = true;
                    RB2D.AddForce(-transform.forward * shootForce);
                    RB2D.velocity = new Vector2(10, 0);
                    ShootDirectionRight = true;
                }
                else
                {
                    RB2D.AddForce(transform.forward * shootForce);
                    RB2D.velocity = new Vector2(-10, 0);
                    ShootDirectionRight = false;
                }
            }
            else
            {
                if (ShootDirectionRight == true)
                {
                    RB2D.AddForce(-transform.forward * shootForce);
                    RB2D.velocity = new Vector2(10, 0);
                }
                else if (ShootDirectionRight == false)
                {
                    RB2D.AddForce(transform.forward * shootForce);
                    RB2D.velocity = new Vector2(-10, 0);
                }
            }
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Die();
        }

    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var CollisonGameObject = collision.gameObject;
        if (CollisonGameObject.tag != Tags.SaveFairy && CollisonGameObject.tag != Tags.Untagged)
        {
            Die();
        }      
    }
}
