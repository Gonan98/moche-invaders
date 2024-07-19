using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnArriveComplete()
    {
        boss.SetActive(true);
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
