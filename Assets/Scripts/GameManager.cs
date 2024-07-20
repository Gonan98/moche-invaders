using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winnerUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject bound;
    [SerializeField] private GameObject playerLiveIcon;
    [SerializeField] private GameObject playerProjectile;
    [SerializeField] private GameObject collateralImage;
    [SerializeField] private GameObject collateralProjectile;
    [SerializeField] private Color primaryColor;
    [SerializeField] private Color secondaryColor;
    private Player player;
    private Invaders invaders;
    private Huaco huaco;
    private Barrier[] barriers;
    private UFO ufo;
    private int score;
    private int lives;
    private bool win = false;
    bool isBossStage = false;
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
        ufo = FindObjectOfType<UFO>();

        NewGame();
    }

    private void Update()
    {
        if ((lives <= 0 || win) && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }


    private void NewGame()
    {
        win = false;
        ufo.gameObject.SetActive(false);
        UIManager.Instance.ChangeUIColor(primaryColor);
        player.ChangeColor(primaryColor);
        Camera.main.backgroundColor = secondaryColor;
        gameOverUI.SetActive(false);
        winnerUI.SetActive(false);
        bound.SetActive(true);
        SetScore(0);
        SetLives(5);
        GeneratePlayerIcons();
        NewRound();
        //BossStage();
        //Respawn();
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
        var projectiles = FindObjectsOfType<Bullet>();

        foreach (Bullet projectile in projectiles)
            Destroy(projectile.gameObject);

        huaco.Reset();
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        foreach (var barrier in barriers)
        {
            barrier.gameObject.SetActive(true);
            barrier.Reset();
        }


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
        bound.SetActive(false);
        invaders.gameObject.SetActive(false);

        if (!isBossStage) return;

        ResetBoss();
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

    private void BossStage()
    {
        isBossStage = true;
        player.SetBuff(false);
        collateralImage.SetActive(false);
        invaders.gameObject.SetActive(false);
        huaco.gameObject.SetActive(false);
        foreach (var barrier in barriers)
            barrier.gameObject.SetActive(false);
        
        ufo.gameObject.SetActive(true);
    }

    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        invaders.DecreaseTimer();

        SetScore(score + invader.Score);

        if (invaders.GetAliveCount() == 0)
            BossStage();
    }

    public void OnHuacoKilled(Huaco huaco)
    {
        SetScore(score + huaco.Score);
        player.SetBuff(true);
        collateralImage.SetActive(true);
    }

    public void OnPlayerKilled()
    {
        SetLives(lives - 1);
        RemovePlayerIcon();

        if (lives > 0)
            Invoke(nameof(Respawn), 2f);
        else
            GameOver();
    }

    public void OnPlayerShootedCollateral()
    {
        player.SetBuff(false);
        collateralImage.SetActive(false);
    }

    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            player.Kill();
            SetLives(0);
            GameOver();
        }
    }

    public void OnBossPhase4()
    {
        Camera.main.backgroundColor = primaryColor;
        player.ChangeColor(secondaryColor);
        UIManager.Instance.ChangeUIColor(secondaryColor);
    }

    public void OnBossDefeated()
    {
        win = true;
        winnerUI.SetActive(true);
        ResetBoss();
    }

    public void ResetBoss()
    {
        var boss = FindObjectOfType<Boss>();
        boss.Reset();
        boss.gameObject.SetActive(false);
        isBossStage = false;
    }
}
