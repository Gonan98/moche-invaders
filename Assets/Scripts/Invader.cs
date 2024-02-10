using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField]
    private Sprite[] animationSprites;
    [SerializeField]
    private Sprite deathSprite;
    [SerializeField]
    private float animationTime;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    private bool alive = true;
    private int score = 10;
    public int Score => score;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        InvokeRepeating(nameof(AnimateSprite), animationFrame, animationTime);
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
        alive = true;
        spriteRenderer.sprite = animationSprites[0];
    }
}
