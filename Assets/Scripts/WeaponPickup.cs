using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class WeaponPickup : MonoBehaviour {

    public int Amount = 5;
    private PlayerAttack PlayerAttackScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player PlayerScript = collision.GetComponent<Player>();
        if (PlayerScript != null)
        {
            PlayerAttackScript = collision.GetComponent<PlayerAttack>();
            if (this.gameObject.tag == Tags.FireSword)
            {
                PlayerAttackScript.AddFireballShots(Amount);
            }
            else if (this.gameObject.tag == Tags.LightningSword)
            {
                PlayerAttackScript.AddLightningSeconds(Amount);
            }
            Destroy(this.gameObject);
        }
    }

}
