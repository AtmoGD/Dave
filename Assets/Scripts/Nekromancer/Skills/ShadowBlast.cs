using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBlast : Skill
{
    private ShadowBlastData shadowBlastData = null;

    private List<float> bulletTimes = new List<float>();

    public override void Enter(Nekromancer _nekromancer, SkillData _skillData)
    {
        base.Enter(_nekromancer, _skillData);

        shadowBlastData = (ShadowBlastData)_skillData;

        bulletTimes.Clear();

        CreateBulletTimes();
    }

    public override void FrameUpdate(float _deltaTime)
    {
        base.FrameUpdate(_deltaTime);

        if (timer >= shadowBlastData.skillTime)
        {
            nekromancer.ChangeSkill();
            return;
        }

        if (bulletTimes.Count > 0)
        {
            if (timer >= bulletTimes[0])
            {
                bulletTimes.RemoveAt(0);

                Shoot();
            }
        }
    }

    public override void PhysicsUpdate(float _deltaTime)
    {
        base.PhysicsUpdate(_deltaTime);

        nekromancer.rb.velocity = Vector2.Lerp(nekromancer.rb.velocity, Vector2.zero, shadowBlastData.stopSpeed * _deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void CreateBulletTimes()
    {
        int bulletAmount = Random.Range(shadowBlastData.bulletAmountMin, shadowBlastData.bulletAmountMax);

        for (int i = 0; i < bulletAmount; i++)
            bulletTimes.Add(Random.Range(0f, shadowBlastData.skillTime));

        bulletTimes.Sort();
    }

    private void Shoot()
    {
        ShadowBallController bullet = ObjectPool.Instance.Get(shadowBlastData.bulletPrefab).GetComponent<ShadowBallController>();
        bullet.Sender = nekromancer.gameObject;
        bullet.transform.position = nekromancer.transform.position;

        bullet.transform.Rotate(0f, 0f, Random.Range(0f, 360f));

        bullet.Nekromancer = nekromancer;
        bullet.UpdateBaseDamage(nekromancer.Damage, nekromancer.gameObject);
    }
}
