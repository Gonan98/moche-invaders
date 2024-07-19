using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private GameObject breaker;
    private AudioSource audioSource;
    private BoxCollider2D[] colliders;
    private void Awake() {
        colliders = GetComponents<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Reset()
    {

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
        
        breaker.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            audioSource.Play();
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeInHierarchy) continue;

                ShowBreaker(child);
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

    private void ShowBreaker(Transform transform)
    {
        breaker.SetActive(true);
        breaker.transform.position = transform.position;
        breaker.transform.Translate(new Vector3(0f, 0.1f, 0f));

        Invoke(nameof(HideBreaker), 0.2f);
    }

    private void HideBreaker()
    {
        breaker.SetActive(false);
    }
}
