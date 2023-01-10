using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStart : MonoBehaviour
{
    [SerializeField] private float delayMin = 0f;
    [SerializeField] private float delayMax = 0f;

    private float delay = 0f;

    private void Start()
    {
        delay = Random.Range(delayMin, delayMax);
        StartCoroutine(Delay());

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
