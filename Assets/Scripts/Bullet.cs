using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 8f;
    private float minimumSpeed;
    private float maxSpeed = 12f;
    public float MiniumSpeed => minimumSpeed;
    public float MaxSpeed => maxSpeed;
    [SerializeField]
    private Vector3 direction;

    private void Awake() 
    {
        minimumSpeed = speed;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
