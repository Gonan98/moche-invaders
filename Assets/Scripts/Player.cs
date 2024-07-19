using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject powerUpPrefab;
    private AudioSource deathSound;
    private GameObject projectile;
    [SerializeField] private Sprite[] deathSprites;
    private Sprite playerSprite;
    private SpriteRenderer spriteRenderer;
    private bool alive = true;
    private float animationTime = 1f;
    private int animationFrame = 0;
    private bool buffed = false;

    private void Awake()
    {
        deathSound = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSprite = spriteRenderer.sprite;
    }

    void Start()
    {
        InvokeRepeating(nameof(Animate), animationTime, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;

        Vector2 position = transform.position;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= speed * Time.deltaTime;
        }   
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
        }

        Vector2 leftEdge = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 rightEdge = Camera.main.ViewportToWorldPoint(Vector2.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        transform.position = position;

        if (Input.GetKey(KeyCode.Space) && projectile == null)
        {
            Shoot(projectilePrefab);
        }

        if (Input.GetKey(KeyCode.LeftShift) && projectile == null && buffed)
        {
            Shoot(powerUpPrefab);
        }
    }

    private void Animate()
    {
        if (alive) return;

        animationFrame++;
        if (animationFrame >= deathSprites.Length)
            animationFrame = 0;
        
        spriteRenderer.sprite = deathSprites[animationFrame];
    }

    private void Shoot(GameObject prefab)
    {
        projectile = Instantiate(prefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!alive) return;
    
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            Kill();
            GameManager.Instance.OnPlayerKilled();
        }
    }

    public void Revive()
    {
        alive = true;
        spriteRenderer.sprite = playerSprite;
    }

    public void Kill()
    {
        alive = false;
        deathSound.Play();
    }

    public void SetBuff(bool value)
    {
        buffed = value;
    }
}
