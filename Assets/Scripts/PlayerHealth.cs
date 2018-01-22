using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public Slider HealthSlider;
    public int currentHealth;
    internal bool IsDead;
    private Animator Anim;
    private Player Player;
    private PlayerAttack PlayerAttack;
    public Color MaxHealthColor = Color.green;
    public Color MinHealthColor = Color.red;
    public int StartHealth = 100;
    public Image FillImage;

    // Use this for initialization
    void Start () {
        Anim = GetComponent<Animator>();
        currentHealth = (int)HealthSlider.value;
        StartHealth = (int)HealthSlider.value;
        Player = GetComponent<Player>();
        PlayerAttack = GetComponent<PlayerAttack>();
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
            currentHealth -= amount;
            HealthSlider.value = currentHealth;
            FillImage.color = Color.Lerp(MinHealthColor, MaxHealthColor, currentHealth / StartHealth);
            if (currentHealth <= 0)
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
        //CapsuleCollider2D Collider = Player.GetComponent<CapsuleCollider2D>();
        //Collider.size = new Vector2(0.4f, 0.4f);
        //Collider.offset = new Vector2(0, 0.0061678f);
        Player.enabled = false;
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
        if (EnemyAttack != null)
        {
            TakeDamage((EnemyAttack as IEnemyAttack).Damage);
        }
    }
}
