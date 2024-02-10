using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    //[SerializeField] private LivesIcon livesIcon;
    [SerializeField] private GameObject footer;
    [SerializeField] private GameObject playerIcon;
    private Player player;
    private Invaders invaders;
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
            var icon = Instantiate(playerIcon, footer.transform);
            icon.transform.localPosition = new Vector3(i * 5, -1f, 0f);
        }
    }

    private void RemovePlayerIcon()
    {
        if (footer.transform.childCount == 1) return;

        var lastChild = footer.transform.GetChild(footer.transform.childCount - 1);
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
        //player.gameObject.SetActive(true);
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
        invaders.IncreaseSpeed();
        //invaders.IncreaseBulletSpeed();

        SetScore(score + invader.Score);

        if (invaders.GetAliveCount() == 0)
            NewRound();
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);
        RemovePlayerIcon();
        //livesIcon.Remove();
        //player.gameObject.SetActive(false);

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
            OnPlayerKilled(player);
        }
    }
}
