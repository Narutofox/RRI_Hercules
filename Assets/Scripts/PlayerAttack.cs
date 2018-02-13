using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public GameObject Lightning;
    public GameObject Fireball;
    private Animator Anim;
    private SpriteRenderer Sprite;
    private Weapons[] WeapnosArray;
    private int currentWeaponIndex;
    private Player PlayerScript;
    private int NumberOfFireballShots = 10;
    private float NumberOfLightningSeconds = 10;
    private float NumberOfInvincibleSeconds = 10;
    private float NumberOfInvincibleDamageSeconds = 5;
    public bool InvincibleBecauseOfDamage;
    private PlayerHealth PlayerHealthScript;
    public bool Attacking;
    private WeaponChangeImage WeaponChangeImageScript;
    // Use this for initialization
    void Start () {
        Anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        WeapnosArray = new Weapons[] { Weapons.Normal, Weapons.Lightning, Weapons.Fire, Weapons.Invincible };
        Sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (PersistanceManager.SaveFileExists() && PersistanceManager.LoadGame().Level == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            PlayerHealthScript = PersistanceManager.LoadGame().PlayerHealth;
        }
        else
        {
            PlayerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }
        
        currentWeaponIndex = 0;
        Attacking = false;
        WeaponChangeImageScript = GameObject.Find("WeaponImage").GetComponent<WeaponChangeImage>();
        WeaponChangeImageScript.ChangeImage(WeapnosArray[currentWeaponIndex]);
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerHealthScript.IsDead)
        {
            return;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            PrevWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            NextWeapon();
        }

        if (Input.GetMouseButtonDown(0) && Anim.GetBool("inAir"))
        {
            AirAttack();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        else if (Input.GetMouseButtonUp(0) && Anim.GetBool("SpecialAttack"))
        {           
            StopLightningAttack();
        }

        if (InvincibleBecauseOfDamage)
        {
            NumberOfInvincibleDamageSeconds -= Time.deltaTime;
            if (NumberOfInvincibleDamageSeconds<= 0)
            {
                PlayerStopInvincible();
                InvincibleBecauseOfDamage = false;
                NumberOfInvincibleDamageSeconds = 5;
            }
            else
            {
                PlayerInvincible();
            }         
        }
        // Ako ima oružje za nepobjedivost odbrojavaj kad DOĐE DO NULE PROMJENI NA SLJDEĆE ORUŽJE
        if (WeapnosArray[currentWeaponIndex] == Weapons.Invincible)
        {
            NumberOfInvincibleSeconds -= Time.deltaTime;
            if (NumberOfInvincibleDamageSeconds <= 0)
            {
                NextWeapon();
            }
        }
    }

    public void StopLightningAttack()
    {
        EndAttack();
        Anim.SetBool("SpecialAttack", false);
        if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning)
        {
            if (GameObject.FindGameObjectWithTag("Lightning") != null)
            {
                Lightning LightningScript = GameObject.FindGameObjectWithTag("Lightning").GetComponent<Lightning>();
                NumberOfLightningSeconds -= LightningScript.NumberOfSecondsLasting;
                LightningScript.Die();
            }
        }

    }

    private void NextWeapon()
    {
        if (currentWeaponIndex + 1 < WeapnosArray.Length)
        {
            currentWeaponIndex++;
        }
        else
        {
            currentWeaponIndex = 0;
        }

        //Ako je prinmio štetu onda ne može prebaciti na oružje koje ga čini nepobjedivim
        if (InvincibleBecauseOfDamage && WeapnosArray[currentWeaponIndex] == Weapons.Invincible)
        {
            currentWeaponIndex++;
        }

        if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning && NumberOfLightningSeconds <=0)
        {
           NextWeapon();
           return;
        }

        if (WeapnosArray[currentWeaponIndex] == Weapons.Fire && NumberOfFireballShots <= 0)
        {
             NextWeapon();
             return;
        }

        if (WeapnosArray[currentWeaponIndex] == Weapons.Invincible && NumberOfInvincibleSeconds <= 0)
        {
            NextWeapon();
            return;
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Invincible)
        {
            PlayerInvincible();
        }
        else
        {
            PlayerStopInvincible();
        }

        WeaponChangeImageScript.ChangeImage(WeapnosArray[currentWeaponIndex]);
    }
    private void PrevWeapon()
    {
        if (currentWeaponIndex - 1 > 0)
        {
            currentWeaponIndex--;
        }
        else
        {
            currentWeaponIndex = WeapnosArray.Length - 1;
        }

        //Ako je prinmio štetu onda ne može prebaciti na oružje koje ga čini nepobjedivim
        if (InvincibleBecauseOfDamage && WeapnosArray[currentWeaponIndex] == Weapons.Invincible)
        {
            PrevWeapon();
            return;
        }

        if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning && NumberOfLightningSeconds <= 0)
        {
            PrevWeapon();
            return;
        }

        if (WeapnosArray[currentWeaponIndex] == Weapons.Fire && NumberOfFireballShots <= 0)
        {
            PrevWeapon();
            return;
        }

        if (WeapnosArray[currentWeaponIndex] == Weapons.Invincible && NumberOfInvincibleSeconds <= 0)
        {
            PrevWeapon();
            return;
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Invincible)
        {
            PlayerInvincible();
        }
        else
        {
            PlayerStopInvincible();
        }
        WeaponChangeImageScript.ChangeImage(WeapnosArray[currentWeaponIndex]);
    }

    public void PlayerInvincible()
    {
        Color tmp = Sprite.color;
        tmp.a = .5f;
        Sprite.color = tmp;
    }
    public void PlayerStopInvincible()
    {
        if (Sprite.color.a != 1f)
        {
            Color tmp = Sprite.color;
            tmp.a = 1f;
            Sprite.color = tmp;
        }

    }

    private void AirAttack()
    {
        Anim.SetTrigger("Attack");
    }

    void Attack()
    {
        if ((WeapnosArray[currentWeaponIndex] == Weapons.Lightning && NumberOfLightningSeconds > 0) || (WeapnosArray[currentWeaponIndex] == Weapons.Fire && NumberOfFireballShots > 0))
        {
            Anim.SetBool("SpecialAttack", true);
            Anim.SetTrigger("Attack");
        }
        else
        {
            Anim.SetTrigger("Attack");
        }

    }

    public void SpecialAttack()
    {

        if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning && PlayerScript.facingRight && NumberOfLightningSeconds > 0)
        {
            Lightning.GetComponent<Lightning>().NumberOfSecondsRemaining = NumberOfLightningSeconds;
            Instantiate(Lightning, new Vector3(transform.position.x + 6, transform.position.y + 0.6f), Quaternion.identity);
            StartAttack();
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning && PlayerScript.facingRight == false && NumberOfLightningSeconds > 0)
        {
            Lightning.GetComponent<Lightning>().NumberOfSecondsRemaining = NumberOfLightningSeconds;
            Instantiate(Lightning, new Vector3(transform.position.x - 6, transform.position.y + 0.6f), Quaternion.identity);
            StartAttack();
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Fire && PlayerScript.facingRight && NumberOfFireballShots > 0)
        {
            Instantiate(Fireball, new Vector3(transform.position.x + 2, transform.position.y + 0.6f), Quaternion.identity);
            NumberOfFireballShots--;
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Fire && PlayerScript.facingRight == false && NumberOfFireballShots > 0)
        {
            Instantiate(Fireball, new Vector3(transform.position.x - 2, transform.position.y + 0.6f), Quaternion.identity);
            NumberOfFireballShots--;
        }
    }

    public bool IsInvincible()
    {
        if (WeapnosArray[currentWeaponIndex] == Weapons.Invincible || InvincibleBecauseOfDamage)
        {
            return true;
        }
        return false;
    }

    public void InvincibleOnDamage()
    {
        InvincibleBecauseOfDamage = true;
    }
   

    public void StartAttack()
    {
        Attacking = true;
    }

    public void EndAttack()
    {
        Attacking = false;
    }

    public bool IsAttacking()
    {
        return Attacking;
    }


    public void AddFireballShots(int amount)
    {
        NumberOfFireballShots += amount;
    }

    public void AddLightningSeconds(int amount)
    {
        NumberOfLightningSeconds += amount;
    }
}
