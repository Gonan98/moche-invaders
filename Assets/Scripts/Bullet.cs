using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private float minimumSpeed;
    private float maxSpeed = 10f;
    public float MiniumSpeed => minimumSpeed;
    public float MaxSpeed => maxSpeed;
    private float lifeTime = 0f;
    private float timeCheck = 0f;
    [SerializeField]
    public Vector3 Direction;

    private void Awake() 
    {
        minimumSpeed = speed;    
    }

    private void Start()
    {
        timeCheck = 0f;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Direction;
        if (lifeTime > 0f)
        {
            if (timeCheck <= lifeTime)
            {
                timeCheck += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Dead")) return;

        Destroy(gameObject);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }
}
