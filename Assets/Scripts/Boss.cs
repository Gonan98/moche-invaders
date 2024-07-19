using DG.Tweening;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float maxHealth = 30;
    [SerializeField] private float currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        Invoke(nameof(Grow), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            currentHealth -= 1;
        }
    }

    void Grow()
    {
        transform.DOMoveY(2.75f, 1f);
        transform.DOScale(new Vector3(0.75f,0.75f), 1).OnComplete(() => GetComponent<BossAI>().enabled = true);
    }
}
