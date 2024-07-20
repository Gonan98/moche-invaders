using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableBoss()
    {
        boss.SetActive(false);
    }

    public void Abduct()
    {
        animator.SetBool("abduct", true);
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
