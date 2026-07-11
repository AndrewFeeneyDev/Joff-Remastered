/* Written by Andrew Feeney. Script to control fading and loading. */
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFadeLoad : MonoBehaviour
{
    // Fade Variables
    [Header("Fade In/Out")]
    [SerializeField] private GameObject fadeOverlay;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [SerializeField] private float fadeInAlpha;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutAlpha;
    [SerializeField] private float fadeOutDuration;

    [SerializeField] private GameObject menuToClose;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBarMask;

    public static UIFadeLoad Instance { get; private set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        loadingScreen.SetActive(false);
        FadeIn(fadeInAlpha, fadeInDuration);
    }

    // Fade In
    private void FadeIn(float alpha, float duration)
    {
        fadeOverlay.SetActive(true);
        fadeCanvasGroup.DOFade(alpha, duration + 0.1f).SetUpdate(UpdateType.Normal, true);
        Invoke("FadeDisabled", duration + 0.1f);
    }

    // Fade Out
    private void FadeOut(float alpha, float duration)
    {
        fadeOverlay.SetActive(true);
        fadeCanvasGroup.DOFade(alpha, duration + 0.1f).SetUpdate(UpdateType.Normal, true);
    }

    private void FadeDisabled()
    {
        fadeOverlay.SetActive(false);
    }

    // Load Coroutine
    private IEnumerator LoadRoutine(int scene)
    {
        FadeOut(fadeOutAlpha, fadeOutDuration);

        yield return new WaitForSecondsRealtime(fadeOutDuration + 0.1f);

        if (menuToClose)
        {
            menuToClose.SetActive(false);
        }

        loadingScreen.SetActive(true);
        loadingBarMask.gameObject.SetActive(true);
        //FadeIn(fadeInAlpha, fadeInDuration);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float loadingProgress = Mathf.Clamp01(operation.progress * 0.9f);
            loadingBarMask.fillAmount = loadingProgress;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    // Loading Scene Function
    public void ChangeScene(int scene)
    {
        StartCoroutine(LoadRoutine(scene));
    }

}
