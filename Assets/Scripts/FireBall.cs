using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

    private Animator Anim;
    private Rigidbody2D RB2D;
    private float timeLeft = 2; // Nakon 2 sekundi se uništi
    private float shootForce = 20f;
    private SpriteRenderer Sprite;
    private Player PlayerScript;

    // Use this for initialization
    void Start () {
        Anim = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (PlayerScript.facingRight)
        {
            RB2D.AddForce(transform.forward * shootForce);
            RB2D.velocity = new Vector2(10, 0);
        }
        else
        {
            Sprite = GameObject.Find(gameObject.name).GetComponent<SpriteRenderer>();
            Sprite.flipX = true;
            RB2D.AddForce(-transform.forward * shootForce);
            RB2D.velocity = new Vector2(-10, 0);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Anim.SetBool("Hit", true);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != Tags.SaveFairy && collision.gameObject.tag != Tags.Player && collision.gameObject.tag != Tags.FireSword && collision.gameObject.tag != Tags.LightningSword)
        {
            var Enemy = collision.gameObject.GetComponent(typeof(Enemy));
            if (Enemy != null)
            {
                (Enemy as Enemy).Die();
            }
            else if (collision.gameObject.GetComponent(typeof(BossEnemy)))
            {
                Enemy = collision.gameObject.GetComponent(typeof(BossEnemy));
                (Enemy as BossEnemy).TakeDamage(30, Weapons.Fire);
            }
            Anim.SetBool("Hit", true);
        }
        
    }
}
