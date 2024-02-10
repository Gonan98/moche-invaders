using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [SerializeField]
    private Invader[] prefabs;
    [SerializeField]
    private float paddingX = 2f;
    [SerializeField]
    private float paddingY = 2f;
    [SerializeField]
    private int rows = 6;
    [SerializeField]
    private int columns = 9;
    private float speed = 0.25f;
    private float minimumSpeed;
    private float maxSpeed;
    public int Rows => rows;
    public int Columns => columns;
    public float MaxSpeed => maxSpeed;
    public float MiniumSpeed => minimumSpeed;
    private Vector3 direction = Vector3.right;
    private Vector3 initialPosition;
    [SerializeField]
    private Bullet bulletPrefab;
    private readonly float bulletSpawnRate = 1f;

    private void Awake() 
    {
        minimumSpeed = speed;
        maxSpeed = 3f;
        initialPosition = transform.position;

        float width = paddingX * (columns - 1);
        float height = paddingY * (rows - 1);

        Vector2 center = new(-width * 0.5f, -height * 0.5f);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var invader = Instantiate(prefabs[i], transform);
                invader.transform.localPosition = new Vector3(center.x + (j * paddingX), center.y + (i * paddingY), 0f);
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(Attack), bulletSpawnRate, bulletSpawnRate);    
    }

    private void Update() {
        transform.position += speed * Time.deltaTime * direction;
        var leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        var rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if(!invader.gameObject.activeInHierarchy)
                continue;

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        direction = new Vector3(-direction.x, 0f, 0f);

        Vector3 position = transform.position;
        position.y -= 0.5f;
        transform.position = position;
    }

    private void Attack()
    {
        int invadersAlive = GetAliveCount();
        if (invadersAlive == 0) return;

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) continue;

            if (Random.value < (1f / invadersAlive))
            {
                var bullet = Instantiate(bulletPrefab, invader.position, Quaternion.identity);
                IncreaseBulletSpeed(bullet);
            }
        }
    }

    public int GetAliveCount()
    {
        int count = 0;
        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
                count++;
        }
        return count;
    }

    public void ResetInvaders()
    {
        direction = Vector2.right;
        transform.position = initialPosition;
        speed = minimumSpeed;

        foreach (Transform invader in transform)
            invader.gameObject.SetActive(true);
    }

    public void IncreaseSpeed()
    {
        int invadersAlive = GetAliveCount();
        float slope = (maxSpeed - minimumSpeed)/(1 - rows * columns);
        speed = slope * (invadersAlive - 1) + maxSpeed;
    }

    public void IncreaseBulletSpeed(Bullet bullet)
    {
        int invadersAlive = GetAliveCount();
        float slope = (bullet.MaxSpeed - bullet.MiniumSpeed)/(1 - rows * columns);
        float result = slope * (invadersAlive - 1) + bullet.MaxSpeed;
        bullet.SetSpeed(result);
    }
}
