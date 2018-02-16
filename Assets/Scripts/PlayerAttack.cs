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
    private Rigidbody2D RB2D;
    public int NumberOfFireballShots = 5;
    public float NumberOfLightningSeconds = 3;
    public float NumberOfInvincibleSeconds = 10;
    private float NumberOfInvincibleDamageSeconds = 5;
    public bool InvincibleBecauseOfDamage;
    private PlayerHealth PlayerHealthScript;
    public bool Attacking;
    private WeaponChangeImage WeaponChangeImageScript;
    private AudioSource Audio;
    public AudioClip IAmInvincible;
    // Use this for initialization
    void Start () {
        Anim = GetComponent<Animator>();
        WeapnosArray = new Weapons[] { Weapons.Normal, Weapons.Lightning, Weapons.Fire, Weapons.Invincible };
        Sprite = GetComponent<SpriteRenderer>();
        PlayerScript =GetComponent<Player>();
        PlayerHealthScript = GetComponent<PlayerHealth>();
        Audio = GetComponent<AudioSource>();
        RB2D = GetComponent<Rigidbody2D>();
        if (PersistanceManager.SaveFileExists() && PersistanceManager.LoadGame().Level == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            SaveGameFile SaveFile = PersistanceManager.LoadGame();
            NumberOfFireballShots = SaveFile.NumberOfFireballShots;
            NumberOfLightningSeconds = SaveFile.NumberOfLightningSeconds;
            NumberOfInvincibleSeconds = SaveFile.NumberOfInvincibleSeconds;
        }

        
        currentWeaponIndex = 0;
        Attacking = false;
        WeaponChangeImageScript = GameObject.Find("WeaponImage").GetComponent<WeaponChangeImage>();
        WeaponChangeImageScript.ChangeImage(WeapnosArray[currentWeaponIndex]);
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerHealthScript.IsDead || Time.timeScale == 0)
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
                var Lightnings = GameObject.FindGameObjectsWithTag("Lightning");

                if (Lightnings.Length > 0)
                {
                    foreach (var LightningObject in Lightnings)
                    {
                        var LightningScript = LightningObject.GetComponent<Lightning>();
                        NumberOfLightningSeconds -= LightningScript.NumberOfSecondsLasting;
                        LightningScript.Die();
                    }
                }

                if (NumberOfLightningSeconds <=0)
                {
                    NextWeapon();
                }
            }
        }

    }

    private void NextWeapon()
    {
        try
        {
            if (currentWeaponIndex + 1 < WeapnosArray.Length)
            {
                currentWeaponIndex++;
            }
            else
            {
                currentWeaponIndex = 0;
            }

            //Ako je primio štetu onda ne može prebaciti na oružje koje ga čini nepobjedivim
            if (InvincibleBecauseOfDamage && WeapnosArray[currentWeaponIndex] == Weapons.Invincible)
            {
                currentWeaponIndex++;
            }

            if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning && NumberOfLightningSeconds <= 0)
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

        }
        catch (System.IndexOutOfRangeException)
        {
            currentWeaponIndex = 0;
        }
        WeaponChangeImageScript.ChangeImage(WeapnosArray[currentWeaponIndex]);
    }
    private void PrevWeapon()
    {
        try
        {
            if (currentWeaponIndex - 1 >= 0)
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
        }
        catch (System.IndexOutOfRangeException)
        {
            currentWeaponIndex = 0;
        }
        WeaponChangeImageScript.ChangeImage(WeapnosArray[currentWeaponIndex]);
    }

    public void PlayerInvincible()
    {
        if (InvincibleBecauseOfDamage == false)
        {
            Audio.clip = IAmInvincible;
            Audio.volume = 1.0f;
            Audio.PlayDelayed(0.5f);
        }
        
        Color tmp = Sprite.color;
        tmp.a = .5f;
        Sprite.color = tmp;
    }
    public void PlayerStopInvincible()
    {
        if (Sprite.color.a != 1f)
        {
            if (Audio != null)
            {
                Audio.Stop();
            }
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
        RB2D.velocity = Vector3.zero;
        if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning && PlayerScript.facingRight && NumberOfLightningSeconds > 0)
        {
            Lightning.GetComponent<Lightning>().NumberOfSecondsRemaining = NumberOfLightningSeconds;
            Instantiate(Lightning, new Vector3(transform.position.x + 6, transform.position.y + 0.6f), Quaternion.identity);
            StartAttack();
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Lightning && PlayerScript.facingRight == false && NumberOfLightningSeconds > 0)
        {
            Lightning.GetComponent<Lightning>().NumberOfSecondsRemaining = NumberOfLightningSeconds;
            Instantiate(Lightning, new Vector3(transform.position.x - 6, transform.position.y), Quaternion.identity);
            StartAttack();
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Fire && PlayerScript.facingRight && NumberOfFireballShots > 0)
        {
            Instantiate(Fireball, new Vector3(transform.position.x + 2, transform.position.y), Quaternion.identity);
            NumberOfFireballShots--;
            if (NumberOfFireballShots <= 0)
            {
                NextWeapon();
            }
        }
        else if (WeapnosArray[currentWeaponIndex] == Weapons.Fire && PlayerScript.facingRight == false && NumberOfFireballShots > 0)
        {
            Instantiate(Fireball, new Vector3(transform.position.x - 2, transform.position.y + 0.6f), Quaternion.identity);
            NumberOfFireballShots--;
            if (NumberOfFireballShots <= 0)
            {
                NextWeapon();
            }
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

    public Weapons CurrentWeapon()
    {
        return WeapnosArray[currentWeaponIndex];
    }
}
