using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huaco : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private float speed = 4f;
    private AudioSource audioSource;
    private float cycleTime = 15f;
    private int score = 500;
    public int Score => score;
    private Vector2 leftDestination;
    private Vector2 rightDestination;
    private int direction = -1;
    private bool spawned;
    private bool alive;
    private float animationTime = 1f;
    private int animationFrame = 0;
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //sprite = spriteRenderer.sprite;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        
        leftDestination = new Vector2(leftEdge.x - 1f, transform.position.y);
        rightDestination = new Vector2(rightEdge.x + 1f, transform.position.y);

        InvokeRepeating(nameof(AnimateSprite), animationTime, 0.25f);
        Despawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned || !alive)
            return;

        if (direction == 1)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    void AnimateSprite()
    {
        if (!alive || !spawned) return;
        animationFrame++;
        if (animationFrame >= sprites.Length)
            animationFrame = 0;
        
        spriteRenderer.sprite = sprites[animationFrame];
    }

    private void Spawn()
    {
        direction *= -1;
        if (direction == 1)
        {
            transform.position = leftDestination;
        }
        else
        {
            transform.position = rightDestination;
        }

        spawned = true;
        alive = true;
        spriteRenderer.sprite = sprites[0];
    }

    public void Despawn()
    {
        spawned = false;
        if (direction == 1)
        {
            transform.position = rightDestination;
        }
        else
        {
            transform.position = leftDestination;
        }
        Invoke(nameof(Spawn), cycleTime);
    }

    private void MoveRight()
    {
        transform.position += speed * Time.deltaTime * Vector3.right;
        if (transform.position.x >= rightDestination.x)
            Despawn();
    }

    private void MoveLeft()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;
        if (transform.position.x <= leftDestination.x)
            Despawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            alive = false;
            spriteRenderer.sprite = deathSprite;
            Invoke(nameof(Kill), 0.2f);
        }
    }

    private void Kill()
    {
        audioSource.Play();
        Despawn();
        GameManager.Instance.OnHuacoKilled(this);
    }

    public void Reset()
    {
        CancelInvoke(nameof(Spawn));
        Despawn();
    }
}
