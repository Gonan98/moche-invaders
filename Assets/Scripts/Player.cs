using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject bulletPrefab;
    private GameObject bullet;
    [SerializeField] private Sprite[] deathSprites;
    private Sprite playerSprite;
    private SpriteRenderer spriteRenderer;
    private bool alive = true;
    private float animationTime = 1f;
    private int animationFrame = 0;
    // Start is called before the first frame update

    private void Awake()
    {
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
        position.x = Mathf.Clamp(position.x, leftEdge.x + 1, rightEdge.x - 1);

        transform.position = position;

        if (Input.GetKey(KeyCode.Space) && bullet == null)
        {
            Shoot();
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

    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!alive) return;
    
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            alive = false;
            GameManager.Instance.OnPlayerKilled(this);
        }
    }

    public void Revive()
    {
        alive = true;
        spriteRenderer.sprite = playerSprite;
    }
}
