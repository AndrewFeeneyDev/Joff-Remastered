using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [Header("High Score")]
    [SerializeField] private int highScore = 0;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private int maxScore = 0;

    [Header("Level Score")]
    [SerializeField] private int score = 0;
    [SerializeField] private TMP_Text scoreText;

    [Header("XYZ Parts")]
    [SerializeField] private int xyzParts = 0;
    [SerializeField] private TMP_Text xyzPartsText;

    public static GameMaster Instance { get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadScore();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (PlayerPrefs.HasKey("Score"))
            {
                PlayerPrefs.DeleteKey("Score");
                score = 0;
            }
            if (PlayerPrefs.HasKey("xyzParts"))
            {
                PlayerPrefs.DeleteKey("xyzParts");
                xyzParts = 0;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", score);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", highScore);
        }

    }

    public void LoadScore()
    {
        PlayerPrefs.GetInt("Score");
        PlayerPrefs.GetInt("HighScore");

        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore}/{maxScore}";
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = $"Score: {score}";
        Debug.Log($"Score: {score}");

        if (score > highScore)
        {
            highScore = score;
            Debug.Log($"HighScore = {highScore}");

            if (highScoreText != null)
            {
                highScoreText.text = $"High Score: {highScore}/{maxScore}";
            }
        }
    }

    public void AddToXYZ(int xyzToAdd)
    {
        xyzParts += xyzToAdd;
        xyzPartsText.text = $"XYZ Parts: {xyzParts}";
        PlayerPrefs.SetInt("xyzParts", xyzParts);
        Debug.Log($"XYZ Parts {xyzParts}");
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        highScore = 0;

        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore}/{maxScore}";
        }
    }
}
