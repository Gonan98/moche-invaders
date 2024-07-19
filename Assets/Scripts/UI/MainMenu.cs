using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    private AudioSource startAudio;
    private bool starting = false;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        startAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();   
        }

        if (!starting)
            return;

        timer += Time.deltaTime;
        if (timer >= 0.2f)
        {
            startButton.SetActive(!startButton.activeInHierarchy);
            timer = 0f;
        }
    }

    public void StartGame()
    {
        Invoke(nameof(ChangeScene), 3f);
        starting = true;
        startAudio.Play();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
