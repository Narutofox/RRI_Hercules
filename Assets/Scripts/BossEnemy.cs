using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour, IEnemyAttack {
    public int Damage { get; set; }
    public GameObject Coin;
    internal bool IsDead;
    public int SetDamage = 50;
    public int NumberOfCoinsToDrop = 2;
    public float MinDistanceToAttack = -2;
    public float MaxDistanceToAttack = 1.5f;
    private bool CanAttack;
    private float AttackInterval = 3;
    private Animator Anim;
    private float RaycastOffset = 0.5f;
    public float RaycastDistance = 10f;
    public Sprite[] BossForms;
    public int Health = 500;
    public float maxSpeed = 1;
    public bool facingRight;
    private Transform Player;
    private PlayerHealth PlayerHealthScript;
    private Vector2 Direction;
    private SpriteRenderer SpriteRend;
    private int Move = 1;
    public bool LightningResistance;
    public bool FireResistance;
    public bool PhysicalResistance;
    public GameObject LongRangeAttack;

    // Use this for initialization
    void Start () {
        facingRight = false;
        CanAttack = true;
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        CheckForFlip(Player);
        Direction = new Vector2(1, 0);
        Damage = SetDamage;
        IsDead = false;
        SpriteRend = GetComponent<SpriteRenderer>();
        SpriteRend.sprite = BossForms[0];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (PlayerHealthScript.IsDead || IsDead)
        {
            return;
        }
        RaycastCheckUpdate();
    }

    private void RaycastCheckUpdate()
    {
        if (facingRight && Direction.x < 0)
        {
            Direction *= -1;
            Move *= -1;
        }
        else if (facingRight == false && Direction.x > 0)
        {
            Direction *= -1;
            Move *= -1;
        }

        RaycastHit2D Hit = CheckRaycast(Direction);
        if (Hit.collider != null && Hit.collider.gameObject.tag == "Player" && CanAttack)
        {
            Attack(Hit);
        }
        else if (Hit.collider != null && Hit.collider.gameObject.tag == this.tag)
        {
            //Ovdje može nastati problem ako GameObject na koem je ova skripta nema tag
            RaycastOffset += 0.5f;
        }
    }

    private void Attack(RaycastHit2D raycast2D)
    {
        float distance = (raycast2D.point - (Vector2)transform.position).magnitude;
        if (distance <= MaxDistanceToAttack && distance >= MinDistanceToAttack)
        {
            // Short Range
        }
        else
        {
            // Long range
        }
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

    private bool IsPlayerRightOfMe(Transform player)
    {

        if (player != null && player.position.x > transform.position.x)
        {
            return true;
        }
        return false;
    }

    private RaycastHit2D CheckRaycast(Vector2 direction)
    {
        float DirectionOffsetRayCast = RaycastOffset * (direction.x > 0 ? 1 : -1);
        Vector2 Position = new Vector2(transform.position.x + DirectionOffsetRayCast, transform.position.y);
        return Physics2D.Raycast(Position, direction, RaycastDistance);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        // Dohvat trenutne skalacije
        Vector3 theScale = transform.localScale;
        // Obrtanje X-osi
        theScale.x *= -1;
        // Postavljanje vrijednosti
        transform.localScale = theScale;
    }

    public void Die()
    {
        if (IsDead == false)
        {
            if (Anim != null && Anim.HasParameterOfType("Die",AnimatorControllerParameterType.Trigger))
            {
                Anim.SetTrigger("Die");
                this.enabled = false;
                IsDead = true;
            }
            else
            {
                Destroy(gameObject);
            }
                     
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
}
