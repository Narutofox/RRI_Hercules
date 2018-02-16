using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack  {
    int Damage { get; set; }
    bool IsAttacking { get; set; }
}
