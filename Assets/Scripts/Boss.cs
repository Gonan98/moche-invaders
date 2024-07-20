using DG.Tweening;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float maxHealth = 20;
    [SerializeField] private float currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    private Vector3 initScale;
    private Vector3 growScale;
    private Vector3 initPosition;

    private bool invencible = true;
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        growScale = new Vector3(0.75f,0.75f);
        initScale = transform.localScale;
        currentHealth = maxHealth;
        //Invoke(nameof(Grow), 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (invencible) return;

            currentHealth--;
        }
    }

    void Init()
    {
        GetComponent<BossAI>().enabled = true;
        GetComponent<BossAI>().Reset();
        invencible = false;
    }

    void Grow()
    {
        transform.DOMoveY(2.75f, 1f);
        transform.DOScale(growScale, 1).OnComplete(Init);
    }

    public void Reduce()
    {
        transform.DOMoveY(3.35f, 1f);
        transform.DOMoveX(Camera.main.transform.position.x, 1f);
        transform.DOScale(initScale, 1).OnComplete(GameManager.Instance.OnBossPhase4);
    }

    public float GetPercentHealth()
    {
        return currentHealth / maxHealth;
    }

    public void SetInvincibility(bool value)
    {
        invencible = value;
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        transform.position = initPosition;
        transform.localScale = initScale;
        invencible = true;
    }

    private void OnEnable() {
        GetComponent<BossAI>().enabled = false;
        Invoke(nameof(Grow), 2f);
    }
}
