using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    public float NumberOfSecondsLasting;
    public float NumberOfSecondsRemaining;
    private PlayerAttack PlayerAttackScript;
    private float TriggerTimer = 2;
    private bool TrigerStay = false;
    // Use this for initialization
    void Start () {
        PlayerAttackScript = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerAttack>();
    }
	
	// Update is called once per frame
	void Update () {
        NumberOfSecondsLasting += Time.deltaTime;
        if (NumberOfSecondsLasting >= NumberOfSecondsRemaining )
        {
            PlayerAttackScript.StopLightningAttack();
        }
        else if (PlayerAttackScript.CurrentWeapon() != Weapons.Lightning || PlayerAttackScript.IsAttacking() == false)
        {
            Destroy(gameObject);
        }

        if (TrigerStay)
        {
            TriggerTimer -= Time.deltaTime;
        }
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        TrigerStay = true;
        var Enemy = collision.gameObject.GetComponent(typeof(Enemy));
        if (Enemy != null)
        {
            (Enemy as Enemy).Die();
        }
        else if (collision.gameObject.GetComponent(typeof(BossEnemy)))
        {
            Enemy = collision.gameObject.GetComponent(typeof(BossEnemy));
            (Enemy as BossEnemy).TakeDamage(30,Weapons.Lightning);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TriggerTimer<= 0)
        {
            var Enemy = collision.gameObject.GetComponent(typeof(Enemy));
            if (Enemy != null && (Enemy as Enemy).IsDead == false)
            {
                (Enemy as Enemy).Die();
            }
            else if (collision.gameObject.GetComponent(typeof(BossEnemy)))
            {
                Enemy = collision.gameObject.GetComponent(typeof(BossEnemy));
                (Enemy as BossEnemy).TakeDamage(30, Weapons.Lightning);
            }
            TriggerTimer = 2;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TrigerStay = false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    var Enemy = collision.gameObject.GetComponent(typeof(Enemy));
    //    if (Enemy != null)
    //    {
    //        (Enemy as Enemy).Die();
    //    }
    //}
}

