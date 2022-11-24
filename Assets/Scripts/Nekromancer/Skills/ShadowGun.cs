using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGun : Skill
{
    private ShadowGunData shadowGunData = null;
    private int currentGun = 0;
    private bool isCharging = false;
    private bool isReleasing = false;
    private int currentCharged = 0;
    private float currentChargeTime = 0f;

    private Vector2 lastMovement = Vector2.zero;

    public override void Enter(Nekromancer _nekromancer, SkillData _skillData)
    {
        base.Enter(_nekromancer, _skillData);

        shadowGunData = _skillData as ShadowGunData;
    }

    public override void FrameUpdate(float _deltaTime)
    {
        base.FrameUpdate(_deltaTime);

        if (!isCharging && !isReleasing && nekromancer.WantsToUseSkills())
            return;

        if (nekromancer.CurrentInput.BaseSkillCharge && nekromancer.CurrentInput.BaseSkill)
        {
            if (!isCharging)
            {
                isCharging = true;
                currentChargeTime = 0f;

                if (!shadowGunData.canMoveWhileCharging)
                    lastMovement = nekromancer.CurrentInput.MoveDir;
            }
        }

        if (isCharging)
        {
            if (nekromancer.CurrentInput.BaseSkillCharge)
            {
                if (currentCharged < shadowGunData.maxChargeAmount)
                {
                    currentChargeTime += _deltaTime;
                    if (currentChargeTime >= shadowGunData.chargeTime)
                    {
                        currentChargeTime = 0f;
                        currentCharged++;
                    }
                }

                if (!shadowGunData.canMoveWhileCharging)
                    nekromancer.CurrentInput.MoveDir = Vector2.zero;
            }
            else
            {
                isCharging = false;
                isReleasing = true;

                if (!shadowGunData.canMoveWhileCharging)
                    nekromancer.CurrentInput.MoveDir = lastMovement;

                if (!shadowGunData.canMoveWhileReleasing)
                    lastMovement = nekromancer.CurrentInput.MoveDir;
            }
        }
        else if (isReleasing)
        {
            if (currentCharged > 0)
            {
                if (!nekromancer.HasCooldown(shadowGunData.cooldowns[1].name))
                {
                    Shoot(true);
                    currentCharged--;
                }

                if (!shadowGunData.canMoveWhileReleasing)
                    nekromancer.CurrentInput.MoveDir = Vector2.zero;
            }
            else
            {
                isReleasing = false;

                if (!shadowGunData.canMoveWhileReleasing)
                    nekromancer.CurrentInput.MoveDir = lastMovement;
            }
        }
        else
        {
            if (nekromancer.CurrentInput.BaseSkill)
            {
                if (!nekromancer.HasCooldown(shadowGunData.cooldowns[0].name))
                {
                    Shoot(false);
                }
            }

        }

        nekromancer.Move();
        nekromancer.Look();
    }

    private void Shoot(bool _charged)
    {
        ShadowBallController bullet = ObjectPool.Instance.Get(shadowGunData.bulletPrefab).GetComponent<ShadowBallController>();

        bullet.transform.position = GetCurrentBulletPosition();
        bullet.transform.right = nekromancer.transform.right;
        bullet.Nekromancer = nekromancer;

        if (_charged)
        {
            bullet.DamageMultiplier = shadowGunData.chargeDamage;
            nekromancer.AddCooldown(shadowGunData.cooldowns[1].GetCopy());
        }
        else
        {
            bullet.DamageMultiplier = 1f;
            nekromancer.AddCooldown(shadowGunData.cooldowns[0].GetCopy());
        }
    }

    private Vector3 GetCurrentBulletPosition()
    {
        currentGun = (currentGun + 1) % nekromancer.gunPoints.Count;
        return nekromancer.gunPoints[currentGun].position;
    }
}
