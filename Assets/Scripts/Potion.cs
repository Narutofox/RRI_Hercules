using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public int Amount;
    public PotionType Type;

    public Potion(int amount, PotionType type)
    {
        Amount = amount;
        Type = type;
    }


}
