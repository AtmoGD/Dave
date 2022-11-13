using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBallController : MonoBehaviour
{
    [SerializeField] private Nekromancer nekromancer = null;
    public Nekromancer Nekromancer { get { return nekromancer; } set { nekromancer = value; } }
    [SerializeField] private ShadowGunData data = null;

    private float lifeTime = 0f;

    private void OnEnable()
    {
        lifeTime = data.bulletLifeTime;
    }

    private void Update()
    {
        Move();

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Die();
    }

    private void Move()
    {
        transform.position += transform.right * data.bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage((int)(Nekromancer.Damage * data.bulletDamage));
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
