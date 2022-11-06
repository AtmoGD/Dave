using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBallController : MonoBehaviour
{
    [SerializeField] private ShadowBall attackData = null;

    private float lifeTime = 0f;

    private void OnEnable()
    {
        lifeTime = attackData.lifeTime;
    }

    private void Update()
    {
        Move();

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }

    private void Move()
    {
        transform.position += transform.right * attackData.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(attackData.damage);
            Destroy(gameObject);
        }
    }
}
