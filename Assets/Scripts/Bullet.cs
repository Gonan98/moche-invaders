using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private Vector3 direction;
    private new BoxCollider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        CheckCollision(other);    
    }

    private void CheckCollision(Collider2D other)
    {
        //Barrier barrier = other.gameObject.GetComponent<Barrier>();

        //if (barrier == null || barrier.CheckCollision(collider2D, transform.position))
            Destroy(gameObject);
    }
}
