using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    internal Slider HealthSlider;
    internal bool IsDead;
    private Animator Anim;
    private Player Player;
    private PlayerAttack PlayerAttack;
    public Color MaxHealthColor = Color.green;
    public Color MinHealthColor = Color.red;
    private Image FillImage;
    private GameManager GameManager;

    // Use this for initialization
    void Start () {
        Anim = GetComponent<Animator>();
        Player = GetComponent<Player>();
        PlayerAttack = GetComponent<PlayerAttack>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        FillImage = GameObject.FindGameObjectWithTag("FillImage").GetComponent<Image>();
        HealthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        if (PersistanceManager.SaveFileExists() && PersistanceManager.LoadGame().Level == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            SaveGameFile SaveFile = PersistanceManager.LoadGame();
            HealthSlider.maxValue = SaveFile.PlayerMaxHealth;
            HealthSlider.value = SaveFile.PlayerHealth;
        }

        

    }
	
	// Update is called once per frame
	void Update () {
        if (IsDead && Player.isGrounded)
        {
            Death();
        }
	}

    public void TakeDamage(int amount)
    {
        if (!IsDead && PlayerAttack.IsInvincible() == false)
        {
            HealthSlider.value -= amount;

            FillImage.color = Color.Lerp(MinHealthColor, MaxHealthColor, HealthSlider.value / HealthSlider.maxValue);


            if (HealthSlider.value <= 0)
            {
                IsDead = true;
            }
            else if(amount > 0)
            {
                PlayerAttack.InvincibleOnDamage();
            }
        }

    }

    private void Death()
    {        
        Anim.SetBool("Alive", false);
       
        
    }

    public void AfterDeathAnimation()
    {
        Player.enabled = false;
        GameManager.PlayerDead();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var EnemyAttack = collision.gameObject.GetComponent(typeof(IEnemyAttack));
        if (EnemyAttack != null)
        {
            TakeDamage((EnemyAttack as IEnemyAttack).Damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var EnemyAttack = collision.gameObject.GetComponent(typeof(IEnemyAttack));
        if (EnemyAttack != null && (EnemyAttack as IEnemyAttack).IsAttacking && PlayerAttack.IsAttacking() == false)
        {
            TakeDamage((EnemyAttack as IEnemyAttack).Damage);
            (EnemyAttack as IEnemyAttack).CanAttack = false;
        }
        if (collision.gameObject.GetComponent(typeof(Potion)) != null)
        {
            var Potion = collision.gameObject.GetComponent(typeof(Potion));
            UpdateHealth(Potion as Potion);
        }
    }

    private void UpdateHealth(Potion potion)
    {
        if (potion.Type == PotionType.Heal)
        {
            if (HealthSlider.value + potion.Amount <= HealthSlider.maxValue)
            {
                HealthSlider.value += potion.Amount;
                FillImage.color = Color.Lerp(MinHealthColor, MaxHealthColor, HealthSlider.value / HealthSlider.maxValue);
                Destroy(potion.gameObject);
            }

        }
        else if (potion.Type == PotionType.HealthBar)
        {
            HealthSlider.maxValue += potion.Amount;
            RectTransform RectTrans = HealthSlider.GetComponent<RectTransform>();
            Vector2 WidthHeight = RectTrans.sizeDelta;
            WidthHeight.x += potion.Amount;
            Destroy(potion.gameObject);
        }

       
    }

}
