using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesIcon : MonoBehaviour
{
    [SerializeField] private GameObject IconPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var icon = Instantiate(IconPrefab, transform);
            icon.transform.localPosition = new Vector2(i * 32f, 0);
        }
    }

    public void Remove()
    {
        if (transform.childCount == 0) return;

        var lastChild = transform.GetChild(transform.childCount - 1);
        Destroy(lastChild.gameObject);
    }
}
