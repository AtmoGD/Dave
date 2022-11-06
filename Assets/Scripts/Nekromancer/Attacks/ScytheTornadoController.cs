using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheTornadoController : MonoBehaviour
{
    [SerializeField] private ScytheTornado attackData = null;

    private float rotated = 0f;

    private void Update()
    {
        Rotate();

        if (rotated >= attackData.rotations * 360f)
            Destroy(gameObject);
    }

    private void Rotate()
    {
        float rotationAngle = attackData.rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotationAngle);
        rotated += Mathf.Abs(rotationAngle);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        IDamagable damagable = _collision.GetComponent<IDamagable>();
        if (damagable != null)
        {
            Debug.Log("Damagable");
            damagable.TakeDamage(attackData.damage);
        }
    }
}
