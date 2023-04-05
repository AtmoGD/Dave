using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CinemachineShaker;

public class ShadowBallController : MonoBehaviour
{
    [SerializeField] private ShadowGunData data = null;
    [SerializeField] private ShakeOptions shakeOptions = null;
    [SerializeField] private GameObject diePrefab = null;

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

    public void UpdateBaseDamage(float _damage, GameObject _sender)
    {
        baseDamage = _damage;
        Sender = _sender;
    }

    private void Move()
    {
        transform.position += transform.right * data.bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!Sender || !nekromancer || Sender == _collision.gameObject || nekromancer.gameObject == _collision.gameObject)
        {
            return;
        }

        ShadowBallController shadowBallController = _collision.gameObject.GetComponent<ShadowBallController>();
        if (shadowBallController != null)
        {
            return;
        }

        Tower tower = _collision.GetComponent<Tower>();
        if (tower != null && _collision.isTrigger)
        {
            return;
        }

        IDamagable damagable = _collision.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.TakeDamage((int)(baseDamage * data.bulletDamage * damageMultiplier), Sender);
        }

        Die();
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (Sender == _other.gameObject || nekromancer.gameObject == _other.gameObject)
        {
            return;
        }

        ShadowBallController shadowBallController = _other.gameObject.GetComponent<ShadowBallController>();
        if (shadowBallController != null)
        {
            return;
        }

        IDamagable damagable = _other.collider.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.TakeDamage((int)(baseDamage * data.bulletDamage * damageMultiplier), Sender);
        }

        Die();
    }

    private void Die()
    {
        Vector2 position = transform.position;
        Vector2 camPosition = Camera.main.transform.position;
        float distance = Vector2.Distance(position, camPosition);

        CineShaker.Instance.Shake(new Shake(shakeOptions, distance));

        GameObject die = Instantiate(diePrefab, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
