using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBallController : MonoBehaviour
{
    [SerializeField] private ShadowGunData data = null;

    [SerializeField] private Nekromancer nekromancer = null;
    public Nekromancer Nekromancer { get { return nekromancer; } set { nekromancer = value; } }
    [SerializeField] private float damageMultiplier = 1f;
    public float DamageMultiplier { get { return damageMultiplier; } set { damageMultiplier = value; } }
    private float baseDamage = 1f;

    private float lifeTime = 0f;

    public GameObject Sender { get; set; } = null;

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

    public void UpdateBaseDamage(float _damage)
    {
        baseDamage = _damage;
    }

    private void Move()
    {
        transform.position += transform.right * data.bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable != null && Sender != collision.gameObject)
        {
            damagable.TakeDamage((int)(baseDamage * data.bulletDamage * damageMultiplier), Sender);
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
