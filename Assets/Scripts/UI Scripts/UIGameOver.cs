using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOver;
    [SerializeField] private float gameOverFade;
    [SerializeField] private Button defaultSelector;

    private CanvasGroup goCanvasGroup;

    [Header("Button Fade Settings")]
    [SerializeField] private float buttonFadeSpeed = 0.5f;
    [SerializeField] private CanvasGroup[] buttonsToFade;

    public static UIGameOver Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        goCanvasGroup = gameObject.GetComponentInChildren<CanvasGroup>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameOverRestart()
    {
        Time.timeScale = 1f;
        int scene = SceneManager.GetActiveScene().buildIndex;
        UIFadeLoad.Instance.ChangeScene(scene);
    }

    public void GameOverQuit()
    {
        Time.timeScale = 1f;
        UIFadeLoad.Instance.ChangeScene(0);
    }

    public void GameOver()
    {
        StartCoroutine(Sequence());
        Time.timeScale = 0f;
    }

    private IEnumerator Sequence()
    {
        goCanvasGroup.alpha = 0f;

        foreach (CanvasGroup canvasGroup in buttonsToFade)
        {
            canvasGroup.alpha = 0f;
        }

        gameOver.SetActive(true);
        defaultSelector.Select();

        goCanvasGroup.DOFade(1f, gameOverFade).SetUpdate(UpdateType.Normal, true);

        yield return new WaitForSecondsRealtime(gameOverFade);

        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < buttonsToFade.Length; i++ )
        {
            sequence.Append(buttonsToFade[i].DOFade(1f, buttonFadeSpeed).SetUpdate(UpdateType.Normal, true));
        }

        yield return null;
    }
}
