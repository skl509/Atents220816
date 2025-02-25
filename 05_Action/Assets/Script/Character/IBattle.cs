using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    float AttackPower { get; }
    float DefencePower { get; }

    void Attack(IBattle target);
    void Defence(float damage);
}
