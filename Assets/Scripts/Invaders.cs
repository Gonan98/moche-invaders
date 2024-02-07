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
    private int rows = 3;
    [SerializeField]
    private int columns = 11;
    private float speed = 1f;
    private Vector3 direction = Vector3.right;

    private void Awake() 
    {
        float width = paddingX * (columns - 1);
        float height = paddingY * (rows - 1);

        Vector2 center = new Vector2(-width / 2, -height / 2);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var invader = Instantiate(prefabs[i], transform);
                invader.transform.localPosition = new Vector3(center.x + (j * paddingX), center.y + (i * paddingY), 0f);
            }
        }
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
        position.y -= 1f;
        transform.position = position;
    }
}
