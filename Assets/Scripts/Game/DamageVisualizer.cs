using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText = null;

    public void SetText(string _text)
    {
        damageText.text = _text;
    }

    public void SetColor(Color _color)
    {
        damageText.color = _color;
    }

    public void SetPosition(Vector3 _position)
    {
        transform.position = _position;
    }

    public void SetRotation(Quaternion _rotation)
    {
        transform.rotation = _rotation;
    }

    public void SetScale(Vector3 _scale)
    {
        transform.localScale = _scale;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
