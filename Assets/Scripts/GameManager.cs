using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject bound;
    [SerializeField] private GameObject playerLiveIcon;
    private Player player;
    private Invaders invaders;
    private Huaco huaco;
    private Barrier[] barriers;
    private int score;
    private int lives;
    public int Score => score;
    public int Lives => lives;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        huaco = FindObjectOfType<Huaco>();
        barriers = FindObjectsOfType<Barrier>(); 

        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
            NewGame();
    }


    private void NewGame()
    {
        gameOverUI.SetActive(false);
        SetScore(0);
        SetLives(3);
        GeneratePlayerIcons();
        NewRound();
    }

    private void GeneratePlayerIcons()
    {
        for (int i = 0; i < lives; i++)
        {
            var icon = Instantiate(playerLiveIcon, bound.transform);
            icon.transform.localPosition = new Vector3(i * 48, 0f, 0f);
        }
    }

    private void RemovePlayerIcon()
    {
        if (bound.transform.childCount == 0) return;

        var lastChild = bound.transform.GetChild(bound.transform.childCount - 1);
        Destroy(lastChild.gameObject);
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        foreach (var barrier in barriers)
        {
            barrier.Reset();
        }

        var projectiles = FindObjectsOfType<Bullet>();

        foreach (Bullet projectile in projectiles)
            Destroy(projectile.gameObject);

        Respawn();
    }

    private void Respawn()
    {
        Vector2 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.Revive();
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        invaders.DecreaseTimer();

        SetScore(score + invader.Score);

        if (invaders.GetAliveCount() == 0)
            NewRound();
    }

    public void OnHuacoKilled(Huaco huaco)
    {
        SetScore(score + huaco.Score);
    }

    public void OnPlayerKilled()
    {
        SetLives(lives - 1);
        RemovePlayerIcon();

        if (lives > 0)
            Invoke(nameof(NewRound), 2f);
        else
            GameOver();
    }

    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayerKilled();
        }
    }
}
