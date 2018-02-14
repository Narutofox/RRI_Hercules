using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private float CONST_DEFAULT_GRAVITY;

    public float maxSpeed = 100f;
    public bool facingRight = true;
    private Rigidbody2D RB2D;
    private Animator Anim;
    public bool isGrounded = false;
    public float jumpForce;
    private PlayerAttack PlayerAttackScript;
    private float jumpTimestamp = -1f;
    private Text BodoviText;
    private SpriteRenderer Sprite;
    // Use this for initialization
    void Start() {
        RB2D = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        PlayerAttackScript = GetComponent<PlayerAttack>();
        Sprite = GetComponent<SpriteRenderer>();
        if (PersistanceManager.SaveFileExists() && PersistanceManager.LoadGame().Level == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            BodoviText = GameObject.Find("Bodovi").GetComponent<Text>();
            SaveGameFile SaveFile = PersistanceManager.LoadGame();
            this.transform.position = new Vector3 { x = SaveFile.x, y = SaveFile.y, z = 0 };
            BodoviText.text = SaveFile.Points.ToString();
        }

        CONST_DEFAULT_GRAVITY = RB2D.gravityScale;

        //jump = new Vector2(0.0f, 2.0f);              
    }

    // Update is called once per frame
    void Update() {
       

        if (isGrounded)
        {
            Anim.SetBool("inAir", false);
            RB2D.gravityScale = CONST_DEFAULT_GRAVITY;
        }
        else
        {
            Anim.SetBool("inAir", true);
            RB2D.gravityScale += Time.deltaTime * 2;
        }

        // Ako pada nije na zemlji
        if (RB2D.velocity.y < -0.1)
        {
            isGrounded = false;
            Anim.SetBool("inAir", true);
            RB2D.gravityScale += Time.deltaTime * 2;
            Anim.SetBool("Landing", true);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Anim.GetBool("inAir") == false)
            {
                Duck(true);
            }
          
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (Anim.GetBool("inAir") == false)
            {
                Duck(false);
            }
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            isGrounded = false;
            Anim.SetTrigger("Jump");
            jumpTimestamp = Time.time;
        }

        if (jumpTimestamp != -1)
        {
            if (Time.time - jumpTimestamp < 1)
            {
                Vector3 newPosition = transform.position;
                newPosition.y += Time.deltaTime * jumpForce;
                transform.position = newPosition;
            }
            else jumpTimestamp = -1;
        }
    }


    private void Duck(bool duck)
    {
        //Mozda maknuti duck funkcionalnost, znam da je ima u pravoj igri, ali previse posla za tako malu stvar, rade se fokusiraj na enemije
        if (duck)
        {
            Anim.SetBool("Duck", true);
            RB2D.gravityScale = CONST_DEFAULT_GRAVITY * 2;
        }
        else
        {
            Anim.SetBool("Duck", false);
            RB2D.gravityScale = CONST_DEFAULT_GRAVITY;
        }
    }


  

    void FixedUpdate()
    {

        // Ovisno o tome koji horizontalan gumb se pritišče
        float move = Input.GetAxis("Horizontal");


        //rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);
        // Ukoliko idemo u lijevo, a ne gledamo u lijevo
        if (move > 0 && !facingRight)
        {
            RB2D.velocity = Vector3.zero;
            Flip();
        }
        // Ukoliko idemo u desno, a ne gledamo u desno
        else if (move < 0 && facingRight)
        {
            RB2D.velocity = Vector3.zero;
            Flip();
        }
        Vector2 movement = new Vector2(move, 0);
        RB2D.velocity = movement * maxSpeed;

        if (isGrounded)
        {
            Anim.SetBool("Landing", false);
            Anim.SetBool("DirectionalLanding", false);
            //rb2d.AddForce(movement * maxSpeed);
            // Pokretanje animacije, ako je 0 stojimo na mjestu ako je manje od 0 idem u lijevo a ako je veće u desno
            if (move != 0)
            {
                Anim.SetBool("Run", true);
            }
            else
            {
                RB2D.velocity = Vector3.zero;
                Anim.SetBool("Run", false);
            }
        }
        else
        {
            Anim.SetBool("Run", false);
            if (move != 0 && Anim.GetBool("inAir"))
            {
                Anim.SetBool("Landing", false);
                Anim.SetBool("DirectionalLanding", true);
            }
            else if (Anim.GetBool("inAir"))
            {
                RB2D.velocity = Vector3.zero;
                Anim.SetBool("DirectionalLanding", false);
                Anim.SetBool("Landing", true);
            }
        }
    }

    void Flip()
    {
        PlayerAttackScript.StopLightningAttack();
        facingRight = !facingRight;
        //// Dohvat trenutne skalacije
        //Vector3 theScale = transform.localScale;
        //// Obrtanje X-osi
        //theScale.x *= -1;
        //// Postavljanje vrijednosti
        //transform.localScale = theScale;
        Sprite.flipX = !Sprite.flipX;
    }

   

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (Anim.GetBool("inAir"))
            {
                Anim.SetTrigger("Land");               
                //transform.position = new Vector3(transform.position.x, 0.55f, transform.position.z);
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }
}
