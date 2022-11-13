using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShadowGun", menuName = "Nekromancer/Skills/ShadowGun")]
public class ShadowGunData : SkillData
{
    public GameObject bulletPrefab = null;
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 5f;
    public float bulletDamage = 1f;

    public override Skill GetSkillInstance()
    {
        return new ShadowGun();
    }
}
