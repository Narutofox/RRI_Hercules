using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour, IEnemyAttack {
    public int Damage { get; set; }

    public bool IsAttacking { get; set; }

    public bool CanAttack { get; set; }

    public GameObject Coin;
    internal bool IsDead;
    public int SetDamage = 50;
    public int NumberOfCoinsToDrop = 2;
    public float MinDistanceToAttack = -2;
    public float MaxDistanceToAttack = 1.5f;
    //private bool CanAttack;
    private float AttackInterval = 2;
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
    private GameObject Portal;
    private int CurrentFormIndex = 0;
    private Slider HealthSlider;
    private Image FillImage;
    public Color MaxHealthColor = Color.green;
    public Color MinHealthColor = Color.red;
    private GameManager GameManager;
    private int PlayerAttackInterval = 1;
    private float PlayerAttackTimer = 0;
    public Transform[] BossMovmentDestinations;
    public float BossDestinationsChangeInterval = 3;
    private int CurrentMovmentDestinationIndex = 0;
    private bool ArrivedAtDestination = false;
    private bool StartMovingCoroutineDone = false;
    
    // Use this for initialization
    void Start () {
        CanAttack = true;
        IsAttacking = false;
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();
        PlayerHealthScript = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerHealth>();
       
        Direction = new Vector2(1, 0);
        Damage = SetDamage;
        IsDead = false;
        SpriteRend = GetComponent<SpriteRenderer>();
        SpriteRend.sprite = BossForms[0];
        Portal = GameObject.FindGameObjectWithTag(Tags.ExitPortal);
        Portal.SetActive(false);
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameManager.BossLevelStart();

        GetAndSetUIObjects();

        CheckForFlip(Player);

        if (BossMovmentDestinations.Length > 0)
        {
            StartCoroutine(StartMoving(BossMovmentDestinations[CurrentMovmentDestinationIndex].position));
        }
    }

    private void GetAndSetUIObjects()
    {
        if (HealthSlider == null && GameObject.FindGameObjectsWithTag("EnemyHealthSlider").Length == 1)
        {
            HealthSlider = GameObject.FindGameObjectWithTag("EnemyHealthSlider").GetComponent<Slider>();
            HealthSlider.maxValue = Health;
            HealthSlider.minValue = 0;
            HealthSlider.value = Health;
            RectTransform RectTrans = HealthSlider.GetComponent<RectTransform>();
            Vector2 WidthHeight = RectTrans.sizeDelta;
            WidthHeight.x = Health;
        }
        if (FillImage == null && GameObject.FindGameObjectsWithTag("EnemyFillImage").Length == 1)
        {
            FillImage = GameObject.FindGameObjectWithTag("EnemyFillImage").GetComponent<Image>();
        }       
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerHealthScript.IsDead || IsDead)
        {
            return;
        }

        if (CanAttack == false)
        {
            AttackInterval -= Time.deltaTime;
            if (AttackInterval <= 0)
            {
                CanAttack = true;
                AttackInterval = 2;
            }
        }
        
        CheckForFlip(Player);
        if (PlayerAttackTimer > 0 )
        {
            PlayerAttackTimer -= Time.deltaTime;
        }

        if (StartMovingCoroutineDone)
        {
            StartMovingCoroutineDone = false;
            StartCoroutine(StartMoving(BossMovmentDestinations[CurrentMovmentDestinationIndex].position));
        }
        GetAndSetUIObjects();
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
            IsAttacking = true;
        }
        else
        {
            ShootLongRange();
        }
        
    }

    public void ShootLongRange()
    {
        GameObject InstantiateLongRangeAttack;
        float forceX;
        if (facingRight)
        {
            InstantiateLongRangeAttack = Instantiate(LongRangeAttack, new Vector3(transform.position.x + 2, transform.position.y + 2), Quaternion.identity);
            forceX = 5;
        }
        else
        {
            InstantiateLongRangeAttack = Instantiate(LongRangeAttack, new Vector3(transform.position.x - 2, transform.position.y + 2), Quaternion.identity);
            forceX = -5;
        }

        InstantiateLongRangeAttack.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX * 100, 1 * 100));
        CanAttack = false;
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
        SpriteRend.flipX = !SpriteRend.flipX;
        //Vector3 theScale = transform.localScale;
        //// Obrtanje X-osi
        //theScale.x *= -1;
        //// Postavljanje vrijednosti
        //transform.localScale = theScale;
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
                Coin.GetComponent<Coins>().Value = 10;


                Instantiate(Coin, new Vector3(transform.position.x + 1, transform.position.y + 0.6f), Quaternion.identity);
                
                Destroy(gameObject);
            }
            Portal.SetActive(true);
            GameManager.BossLevelEnd();
        }
    }

    public void TakeDamage(int damage,Weapons weapon)
    {
        if (LightningResistance && weapon == Weapons.Lightning)
        {
            return;
        }
        else if (FireResistance && weapon == Weapons.Fire)
        {
            return;
        }
        else if (PhysicalResistance && weapon == Weapons.Normal)
        {
            return;
        }

        HealthSlider.value -= damage;
        try
        {
            FillImage.color = Color.Lerp(MinHealthColor, MaxHealthColor, HealthSlider.value / HealthSlider.maxValue);

        }
        catch (MissingReferenceException)
        {
            FillImage = GameObject.FindGameObjectWithTag("EnemyFillImage").GetComponent<Image>();
            HealthSlider = GameObject.Find("EnemyHealthSlider").GetComponent<Slider>();
            FillImage.color = Color.Lerp(MinHealthColor, MaxHealthColor, HealthSlider.value / HealthSlider.maxValue);
        }

        if (HealthSlider.value <= 0)
        {
            Die();
            return;
        }
        else if (HealthSlider.value < (Health / 2) && HealthSlider.value > (Health / 3) && CurrentFormIndex != 1)
        {
            CurrentFormIndex++;
            SpriteRend.sprite = BossForms[CurrentFormIndex];
            Damage += 5;
        }
        else if (HealthSlider.value < (Health / 3) && CurrentFormIndex != 2)
        {
            CurrentFormIndex++;
            SpriteRend.sprite = BossForms[CurrentFormIndex];
            Damage += 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var PlayerAttack = collision.gameObject.GetComponent(typeof(PlayerAttack));
        if (PlayerAttack != null && (PlayerAttack as PlayerAttack).IsAttacking() && IsAttacking == false)
        {
            TakeDamage(20,Weapons.Normal);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerAttackTimer <= 0)
        {
            var PlayerAttack = collision.gameObject.GetComponent(typeof(PlayerAttack));
            if (PlayerAttack != null && (PlayerAttack as PlayerAttack).IsAttacking())
            {
                TakeDamage(20, Weapons.Normal);
                PlayerAttackTimer = PlayerAttackInterval;
            }           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var PlayerAttack = collision.gameObject.GetComponent(typeof(PlayerAttack));
        if (PlayerAttack != null && (PlayerAttack as PlayerAttack).IsAttacking() && IsAttacking == false)
        {
            PlayerAttackTimer = 0;
        }
    }

    private IEnumerator StartMoving(Vector3 target)
    {
        float step = maxSpeed * Time.deltaTime;
     
        while (!ArrivedAtDestination)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (Vector3.Distance(transform.position, target) == 0) ArrivedAtDestination = true;
            yield return null;
        }


        if (ArrivedAtDestination)
        {
            yield return new WaitForSeconds(BossDestinationsChangeInterval);
            if (CurrentMovmentDestinationIndex +1 >  BossMovmentDestinations.Length -1)
            {
                CurrentMovmentDestinationIndex = 0;
            }
            else
            {
                CurrentMovmentDestinationIndex++;
            }
            ArrivedAtDestination = false;
            StartMovingCoroutineDone = true;
        }
    }
}
