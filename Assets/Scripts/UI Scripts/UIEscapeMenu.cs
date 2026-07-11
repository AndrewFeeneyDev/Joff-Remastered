using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEscapeMenu : MonoBehaviour
{
    [Header("Escape Menu Settings")]
    [SerializeField] private GameObject escMenu;
    [SerializeField] private float escFadeSpeed = 1f;
    [SerializeField] private Button defaultSelector;

    private CanvasGroup escCanvasGroup;

    [Header("Button Fade Settings")]
    [SerializeField] private float buttonFadeSpeed = 0.5f;
    [SerializeField] private CanvasGroup[] buttonsToFade;
    private bool isOpening = false;
    private bool isClosing = false;

    private PlayerInputs playerInputs;
    private InputAction escapeAction;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    void Start()
    {
        escCanvasGroup = escMenu.GetComponentInChildren<CanvasGroup>();
        escCanvasGroup.alpha = 0f;

        escMenu.SetActive(false);
    }

    private void OnEnable()
    {
        escapeAction = playerInputs.Player.Escape;
        escapeAction.Enable();
        escapeAction.performed += OnEscape;
    }

    private void OnDisable()
    {
        escapeAction = playerInputs.Player.Escape;
        escapeAction.Disable();
        escapeAction.performed -= OnEscape;
    }

    private void OnEscape(InputAction.CallbackContext context)
    {
        if (isOpening || isClosing)
        {
            return;
        }

        if (!escMenu.activeSelf)
        {
            StartCoroutine(OpenEscMenu());
        }
        else
        {
            StartCoroutine(CloseEscMenu());
        }
    }

    void Update()
    {
        
    }

    public void Resume()
    {
        StartCoroutine(CloseEscMenu());
    }

    public void Restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        UIFadeLoad.Instance.ChangeScene(scene);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        UIFadeLoad.Instance.ChangeScene(0);
        Time.timeScale = 1f;
    }

    private IEnumerator OpenEscMenu()
    {
        isOpening = true;
        Time.timeScale = 0f;

        foreach (CanvasGroup canvasGroup in buttonsToFade)
        {
            canvasGroup.alpha = 0f;
        }

        escCanvasGroup.alpha = 0f;

        escMenu.SetActive(true);
        defaultSelector.Select();
        escCanvasGroup.DOFade(1f, escFadeSpeed).SetUpdate(UpdateType.Normal, true);

        yield return new WaitForSecondsRealtime(escFadeSpeed);

        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < buttonsToFade.Length; i++)
        {
            sequence.Append(buttonsToFade[i].DOFade(1f, buttonFadeSpeed).SetUpdate(UpdateType.Normal, true));
        }

        yield return new WaitForSecondsRealtime(buttonsToFade.Length * buttonFadeSpeed);

        isOpening = false;

        yield return null;
    }

    private IEnumerator CloseEscMenu()
    {
        isClosing = true;

        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        for (int i = buttonsToFade.Length - 1; i >= 0; i--)
        {
            sequence.Append(buttonsToFade[i].DOFade(0f, buttonFadeSpeed).SetUpdate(UpdateType.Normal, true));
        }

        yield return new WaitForSecondsRealtime(buttonsToFade.Length * buttonFadeSpeed);

        escCanvasGroup.DOFade(0f, escFadeSpeed).SetUpdate(UpdateType.Normal, true);

        yield return new WaitForSecondsRealtime(escFadeSpeed);

        escMenu.SetActive(false);
        isClosing = false;
        Time.timeScale = 1f;

        yield return null;
    }
}
