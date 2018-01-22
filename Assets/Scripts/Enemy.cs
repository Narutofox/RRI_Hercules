using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy,IEnemyAttack{

    //public int Health = 100;
    public float maxSpeed = 1;
    internal bool facingRight;
    public GameObject Arrow;
    //private Rigidbody2D RB2D;
    private bool CanAttack;
    private float AttackInterval = 3;
    private Animator Anim;
    private float RaycastOffset = 0.5f;
    private float RaycastDistance = 15f;
    private Transform Player;
    private PlayerHealth PlayerHealthScript;
    public EnemyType Type;
    private float Distance = 0;
    private Rigidbody2D RB2D;
    private Vector2 direction;
    private int Move = 1;

    public int Damage { get; set; }

    // Use this for initialization
    void Start () {
        facingRight = false;
        CanAttack = true;
        RB2D = GameObject.Find(this.name).GetComponent<Rigidbody2D>();
        Anim = GameObject.Find(this.name).GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        CheckForFlip(Player);
        direction = new Vector2(1, 0);
        Damage = 0;       
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerHealthScript.IsDead)
        {
            return;
        }

        if (CanAttack == false)
        {
            AttackInterval -= Time.deltaTime;
            if (AttackInterval <= 0)
            {
                CanAttack = true;
            }
        }
        CheckForFlip(Player);

    }

    private void FixedUpdate()
    {
            RaycastCheckUpdate();               
    }

    private void RaycastCheckUpdate()
    {
        
        if (!facingRight)
        {
            direction *= -1;
            Move *= -1;
        }
        RaycastHit2D Hit = CheckRaycast(direction);
        if (Hit.collider != null && Hit.collider.gameObject.tag == "Player" && CanAttack)
        {
            Attack();
        }
        else if (Hit.collider != null && Hit.collider.gameObject.tag == this.tag)
        {
            RaycastOffset += 0.5f;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        // Dohvat trenutne skalacije
        Vector3 theScale = transform.localScale;
        // Obrtanje X-osi
        theScale.x *= -1;
        // Postavljanje vrijednosti
        transform.localScale = theScale;
    }
    
    private RaycastHit2D CheckRaycast(Vector2 direction)
    {
        float DirectionOffsetRayCast = RaycastOffset * (direction.x > 0 ? 1 : -1);
        Vector2 Position = new Vector2(transform.position.x + DirectionOffsetRayCast, transform.position.y);
        return Physics2D.Raycast(Position, direction, RaycastDistance);
    }

    private bool IsPlayerRightOfMe(Transform player) {

        if (player.position.x > transform.position.x)
        {
            return true;
        }
        return false;
    }

    private void CheckForFlip(Transform player)
    {
        if (IsPlayerRightOfMe(player) && !facingRight)
        {
            Flip();
        }
        else if (IsPlayerRightOfMe(player) == false && facingRight)
        {
            Flip();
        }
    }


    private void Attack()
    {
        if (Type == EnemyType.ShortRange)
        {
           
            Distance = Vector2.Distance(this.transform.position, Player.position);
            if (Distance <= 1.5 && Distance >= -2)
            {
                RB2D.velocity = Vector3.zero;
                Anim.SetTrigger("Attack");
                Anim.SetBool("Charge", false);
                Damage = 20;
            }
            else
            {
                Anim.SetBool("Charge", true);
                Vector2 movement = new Vector2(Move, 0);
                RB2D.velocity = movement * maxSpeed;
                Damage = 0;
            }
        }
        else
        {
            Anim.SetTrigger("Attack");
        }
        
    }

    public void ShootArrow()
    {
        GameObject InstantiateArrow = null;
        int ArrowDamage = 0;
        if (this.gameObject.tag == "WeakArcher")
        {
           ArrowDamage = 10;
        }
        else if (this.gameObject.tag == "AverageArcher")
        {
            ArrowDamage = 20;
        }
        else if (this.gameObject.tag == "StrongArcher")
        {
            ArrowDamage = 30;
        }

        if (facingRight)
        {
            InstantiateArrow = Instantiate(Arrow, new Vector3(transform.position.x + 2, transform.position.y), Quaternion.identity);
        }
        else
        {
            InstantiateArrow =  Instantiate(Arrow, new Vector3(transform.position.x - 2, transform.position.y), Quaternion.identity);
        }

        InstantiateArrow.GetComponent<ArrowScript>().Init(this.gameObject, ArrowDamage);
    }

    public void ShortRangeAttack()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var PlayerAttack = collision.gameObject.GetComponent(typeof(PlayerAttack));
        if (PlayerAttack != null && (PlayerAttack as PlayerAttack).IsAttacking())
        {
            Die();
        }
    }

    public void Die()
    {
        Anim.SetTrigger("Die");
        this.enabled = false;
       
    }

    public enum EnemyType
    {
        LongRange,
        ShortRange
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
