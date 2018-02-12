using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class WeaponPickup : MonoBehaviour {

    public int Amount = 5;
    private PlayerAttack PlayerAttackScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerAttackScript = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerAttack>();
        if (PlayerAttackScript != null)
        {
            if (this.gameObject.tag == Tags.FireSword)
            {
                PlayerAttackScript.AddFireballShots(Amount);
            }
            else if (this.gameObject.tag == Tags.LightningSword)
            {
                PlayerAttackScript.AddLightningSeconds(Amount);
            }
            Destroy(gameObject);
        }
    }

}
