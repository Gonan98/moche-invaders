using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            //gameObject.SetActive(false);
            var parent = transform.parent;
            foreach (Transform child in parent)
            {
                if (!child.gameObject.activeInHierarchy) continue;

                child.gameObject.SetActive(false);
                break;
            }
        }
    }
}
