using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangeImage : MonoBehaviour {

    public Sprite Fire;
    public Sprite Lightning;
    public Sprite Invincible;
    public Sprite Sword;
    private Image WeaponImage;
    // Use this for initialization
    private void Start()
    {
        WeaponImage =  this.gameObject.GetComponent<Image>();
    }

    public void ChangeImage(Weapons weapon)
    {
        switch (weapon)
        {
            case Weapons.Normal:
                WeaponImage.sprite = Sword;
                break;
            case Weapons.Fire:
                WeaponImage.sprite = Fire;
                break;
            case Weapons.Lightning:
                WeaponImage.sprite = Lightning;
                break;
            case Weapons.Invincible:
                WeaponImage.sprite = Invincible;
                break;
            default:
                break;
        }
    }
}
