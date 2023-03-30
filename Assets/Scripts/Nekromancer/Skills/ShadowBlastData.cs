using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowBlast", menuName = "Nekromancer/Skills/ShadowBlast")]
public class ShadowBlastData : SkillData
{
    public float skillTime = 1f;
    public GameObject bulletPrefab = null;
    public float bulletDamage = 1f;
    public int bulletAmountMin = 5;
    public int bulletAmountMax = 8;
    public float stopSpeed = 0.1f;

    public override Skill GetSkillInstance()
    {
        return new ShadowBlast();
    }
}
