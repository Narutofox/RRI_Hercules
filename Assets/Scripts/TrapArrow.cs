using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour, IEnemyAttack
{
    public int Damage {get; set;}
    public bool IsAttacking { get; set; }
    public bool CanAttack { get; set; }
    private SpriteRenderer Sprite;
    private Animator Anim;
    private GameObject ArrowsTrap;
    // Use this for initialization
    void Start () {
        Damage = 10;
        Sprite = this.GetComponent<SpriteRenderer>();
        Color tmp = Sprite.color;
        tmp.a = 0f;
        Sprite.color = tmp;
       
        ArrowsTrap = this.transform.parent.gameObject;
        Anim = ArrowsTrap.GetComponent<Animator>();
        IsAttacking = true;
        CanAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.Player && Anim.GetBool("Active") == false)
        {
            IEnumerable<TrapArrow> TrapArrowScripts = ArrowsTrap.GetComponentsInChildren<TrapArrow>();
            foreach (TrapArrow TrapArrowScript in TrapArrowScripts)
            {
                Color tmp = TrapArrowScript.Sprite.color;
                tmp.a = 1f;
                TrapArrowScript.Sprite.color = tmp;
                TrapArrowScript.Sprite.sortingOrder = 3;
                var EdgeCollider = TrapArrowScript.gameObject.GetComponent<EdgeCollider2D>();
                if (EdgeCollider != null)
                {
                    EdgeCollider.enabled = false;
                }
            }

            Anim.SetTrigger("Activate");
            Anim.SetBool("Active", true);
        }
    }
}
