using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollateralBullet : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector2[] directions;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Dead");
            for (int i = 0; i < directions.Length; i++)
            {
                float angle = i * 45f;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                GameObject bullet = Instantiate(bulletPrefab, other.transform.position, rotation);
                bullet.GetComponent<Bullet>().Direction = directions[i];
                bullet.GetComponent<Bullet>().SetLifeTime(0.25f);
            }
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerShootedCollateral();    
    }
}
