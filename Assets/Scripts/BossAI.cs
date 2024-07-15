using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3
    };

    private Boss boss;
    [SerializeField] private float speed = 4f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform[] spawnBullets;
    [SerializeField] private GameObject arm;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float spriteWidth;
    private Vector3 leftEdge;
    private Vector3 rightEdge;
    [SerializeField] BossPhase currentPhase;
    private bool isShooting = false;
    private Vector3 initScale;

    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
        boss = GetComponent<Boss>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        //InvokeRepeating(nameof(Shoot), 0f, 1f);
        spriteWidth = spriteRenderer.bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        Move();
        Perform();
    }

    private void CheckHealth()
    {
        if (boss.CurrentHealth > boss.MaxHealth * 0.75f)
        {
            if (currentPhase != BossPhase.Phase1)
            {
                currentPhase = BossPhase.Phase1;
                animator.SetBool("attacking", false);
            }
        }
        else if (boss.CurrentHealth > boss.MaxHealth * 0.5f)
        {
            if (currentPhase != BossPhase.Phase2)
            {
                currentPhase = BossPhase.Phase2;
                speed *= -1;

                if (speed < 0)
                    transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1f, 1f, 1f));

                transform.DOMoveX(Camera.main.transform.position.x, 1f)
                    .OnComplete(() => {
                        animator.SetBool("attacking", true);
                    });
            }
        }
        else
        {
            if (currentPhase != BossPhase.Phase3)
            {
                currentPhase = BossPhase.Phase3;
                speed *= -1;
                animator.SetBool("attacking", false);
                transform.DOMoveX(Camera.main.transform.position.x, 1f).OnComplete(() => {
                        transform.localScale = initScale;
                        StartCoroutine(MultiShootCoroutine());
                    });
            }
        }
    }

    void Move()
    {
        Vector3 position = transform.position;
        position.x += speed * Time.deltaTime;
        transform.position = position;

        if (position.x - spriteWidth < leftEdge.x || position.x + spriteWidth > rightEdge.x)
        {
            speed *= -1;
            OnChangeDirection();
        }
    }

    void Perform()
    {
        switch (currentPhase)
        {
            case BossPhase.Phase1:
                if (!isShooting)
                    StartCoroutine(ShootCoroutine());
                break;

            case BossPhase.Phase2:

                break;

            case BossPhase.Phase3:           
                break;

            default:
                Debug.Log("Nothing Happens...");
                break;
        }
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;
        while(currentPhase == BossPhase.Phase1)
        {
            if (Random.value <= 0.5f)
            {
                Shoot();
            }
            yield return new WaitForSeconds(1f);
        }
        isShooting = false;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, spawnBullets[0].position, spawnBullets[0].rotation);
    }

    IEnumerator MultiShootCoroutine()
    {
        isShooting = true;
        while(currentPhase == BossPhase.Phase3)
        {
            if (Random.value <= 0.75f)
            {
                MultiShoot();
            }
            yield return new WaitForSeconds(1f);
        }
        isShooting = false;
    }

    void MultiShoot()
    {
        foreach (Transform child in spawnBullets)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                var bullet = Instantiate(bulletPrefab, child.position, child.rotation);
                bullet.Direction = child.localPosition.normalized;

                child.gameObject.SetActive(false);    
            }
        }
    }

    public void EnableArm()
    {
        arm.SetActive(true);
    }

    public void DisableArm()
    {
        arm.SetActive(false);
    }

    public void Attack()
    {
        if (currentPhase != BossPhase.Phase2) return;

        animator.SetBool("attacking", true);
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1f, 1f, 1f));
    }

    private void OnChangeDirection()
    {
        if (currentPhase != BossPhase.Phase2) return;
        animator.SetBool("attacking", false);
    }
}
