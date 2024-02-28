using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

public class Invaders : MonoBehaviour
{
    [SerializeField]
    private Invader[] prefabs;
    [SerializeField]
    private float paddingX = 2f;
    [SerializeField]
    private float paddingY = 2f;
    private int rows = 5;
    private int columns = 9;
    private float speed = 0.2f;
    private int rowIndex = 0;
    private Vector3 direction = Vector3.right;
    private Vector3 initialPosition;
    private Vector3[] initialPositions;
    [SerializeField]
    private Bullet bulletPrefab;
    private float bulletSpawnRate = 1f;
    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private float timer = 0f;
    private float timeCheck = 0.1f;
    private readonly float minTimeCheck = 0.0075f;
    private float maxTimeCheck;
    private bool horizontal = true;
    private bool allMoved = false;
    private void Awake() 
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        initialPosition = transform.position;
        initialPositions = new Vector3[rows * columns];
        maxTimeCheck = timeCheck;

        float width = paddingX * (columns - 1);
        float height = paddingY * (rows - 1);

        Vector2 center = new(-width * 0.5f, -height * 0.5f);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var invader = Instantiate(prefabs[i], transform);
                invader.transform.localPosition = new Vector3(center.x + (j * paddingX), center.y + (i * paddingY), 0f);
                int index = i * columns + j;
                initialPositions[index] = invader.transform.localPosition;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(Attack), bulletSpawnRate, bulletSpawnRate);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer >= timeCheck)
        {
            if (horizontal)
            {
                MoveHorizontal();
                if (allMoved)
                    CheckInvadersMovement();
            }
            else
            {
                MoveVertical();
            }
            timer = 0f;
        }
    }

    private void MoveHorizontal()
    {
        for (int columnIndex = 0; columnIndex < columns; columnIndex++)
        {
            var child = transform.GetChild(rowIndex * columns + columnIndex);
            child.Translate(speed * direction);
        }

        rowIndex++;
        if (rowIndex >= rows)
        {
            rowIndex = 0;
            allMoved = true;
        }
    }

    private void MoveVertical()
    {
        direction = new Vector3(-direction.x, 0f, 0f);

        for (int columnIndex = 0; columnIndex < columns; columnIndex++)
        {
            var child = transform.GetChild(rowIndex * columns + columnIndex);
            child.Translate(0f, -0.5f, 0f);
        }

        rowIndex++;
        if (rowIndex >= rows)
        {
            rowIndex = 0;
            horizontal = true;
        }
    }

    private void CheckInvadersMovement()
    {
        foreach (Transform invader in transform)
        {
            if(!invader.gameObject.activeInHierarchy)
                continue;

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1))
            {
                horizontal = false;
                rowIndex = 0;
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1))
            {
                horizontal = false;
                rowIndex = 0;
                break;
            }
        }
        allMoved = false;
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
        timeCheck = maxTimeCheck;
        rowIndex = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                var invader = transform.GetChild(index);
                invader.localPosition = initialPositions[index];
                invader.gameObject.SetActive(true);
            }
        }
    }

    public void DecreaseTimer()
    {
        int invadersAlive = GetAliveCount();
        float slope = (maxTimeCheck - minTimeCheck)/(rows * columns - 1);
        timeCheck = slope * (invadersAlive - 1) + minTimeCheck;
    }

    public void IncreaseBulletSpeed(Bullet bullet)
    {
        int invadersAlive = GetAliveCount();
        float slope = (bullet.MaxSpeed - bullet.MiniumSpeed)/(1 - rows * columns);
        float result = slope * (invadersAlive - 1) + bullet.MaxSpeed;
        bullet.SetSpeed(result);
    }
}
