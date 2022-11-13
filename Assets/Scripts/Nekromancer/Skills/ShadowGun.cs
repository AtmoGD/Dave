using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGun : Skill
{
    private ShadowGunData shadowGunData = null;
    private int currentGun = 0;

    public override void Enter(Nekromancer _nekromancer, SkillData _skillData)
    {
        base.Enter(_nekromancer, _skillData);

        shadowGunData = _skillData as ShadowGunData;
    }

    public override void FrameUpdate(float _deltaTime)
    {
        base.FrameUpdate(_deltaTime);

        nekromancer.Move();
        nekromancer.Look();

        if (nekromancer.CurrentInput.BaseSkill)
        {
            if (!nekromancer.HasCooldown(shadowGunData.cooldowns[0].name))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        ShadowBallController bullet = ObjectPool.Instance.Get(shadowGunData.bulletPrefab).GetComponent<ShadowBallController>();

        bullet.transform.position = GetCurrentBulletPosition();
        bullet.transform.right = nekromancer.transform.right;
        bullet.Nekromancer = nekromancer;

        nekromancer.AddCooldown(shadowGunData.cooldowns[0].GetCopy());
    }

    private Vector3 GetCurrentBulletPosition()
    {
        currentGun = (currentGun + 1) % nekromancer.gunPoints.Count;
        return nekromancer.gunPoints[currentGun].position;
    }
}
