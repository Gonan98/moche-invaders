using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
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

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        invaders.SetSpeed(NewSpeed(invaders.GetAliveCount()));
        SetScore(score + invader.Score);

        if (invaders.GetAliveCount() == 0)
            NewRound();
    }

    private void NewGame()
    {
        gameOverUI.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        foreach (var barrier in barriers)
        {
            barrier.Reset();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector2 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
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
        livesText.text = this.lives.ToString();
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);
        player.gameObject.SetActive(false);

        if (lives > 0)
            Invoke(nameof(NewRound), 1f);
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

    private float NewSpeed(int invadersCount)
    {
        float slope = (invaders.MaxSpeed - invaders.MiniumSpeed)/(1 - invaders.Rows * invaders.Columns);
        float result = slope * (invadersCount - 1) + invaders.MaxSpeed;
        Debug.Log(result);
        return result;
    }
}
