using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private BoxCollider2D[] colliders;
    private void Start() {
        colliders = GetComponents<BoxCollider2D>();
    }

    public void Reset()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeInHierarchy) continue;

                child.gameObject.SetActive(false);
                break;
            }

            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;

                collider.enabled = false;
                break;
            }
        }
    }
}
