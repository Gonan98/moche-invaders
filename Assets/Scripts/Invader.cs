using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField]
    private Sprite[] animationSprites;
    [SerializeField]
    private Sprite deathSprite;
    private SpriteRenderer spriteRenderer;
    private float animationTime = 1f;
    private int animationFrame = 0;
    private bool alive = true;
    private int score = 10;
    public int Score => score;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        InvokeRepeating(nameof(AnimateSprite), animationTime, 0.5f);
    }

    void AnimateSprite()
    {
        if (!alive) return;
        animationFrame++;
        if (animationFrame >= animationSprites.Length)
            animationFrame = 0;
        
        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            spriteRenderer.sprite = deathSprite;
            alive = false;
            Invoke(nameof(Kill), 0.2f);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            GameManager.Instance.OnBoundaryReached();
        }
    }

    private void Kill()
    {
        GameManager.Instance.OnInvaderKilled(this);
    }

    private void OnEnable() {
        gameObject.layer = LayerMask.NameToLayer("Invader");
        alive = true;
        spriteRenderer.sprite = animationSprites[0];
    }
}
