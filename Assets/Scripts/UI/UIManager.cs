using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Text score;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Image bound;
    [SerializeField] private GameObject playerLives;

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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeUIColor(Color color)
    {
        score.color = color;
        bound.color = color;

        foreach (Transform child in gameOver.transform)
        {
            child.GetComponent<Text>().color = color;
        }

        foreach (Transform child in playerLives.transform)
        {
            child.GetComponent<Image>().color = color;
        }
    }
}
