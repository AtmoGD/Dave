using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieHelper : MonoBehaviour
{
    public void Die()
    {
        Destroy(gameObject);
    }

    public void DieIn(float _delay)
    {
        Destroy(gameObject, _delay);
    }
}
