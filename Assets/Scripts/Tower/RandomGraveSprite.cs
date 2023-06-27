using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RandomSpriteData
{
    [SerializeField] public Sprite sprite = null;
    [SerializeField] public Vector2 size = Vector2.zero;
}

public class RandomGraveSprite : MonoBehaviour
{
    [SerializeField] private List<RandomSpriteData> sprites = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        int random = UnityEngine.Random.Range(0, sprites.Count);
        spriteRenderer.sprite = sprites[random].sprite;
        spriteRenderer.transform.localScale = sprites[random].size;
    }
}
