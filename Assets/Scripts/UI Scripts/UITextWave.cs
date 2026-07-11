using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UITextWave : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool loop = false;
    [SerializeField] private float waveHeight = 1.0f;
    [SerializeField] private float waveDuration = 0.2f;
    [SerializeField] private float letterDelay = 0.05f;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    [Header("Letters")]
    [SerializeField] private GameObject[] letters;
    private Vector3[] originalPositions;

    
    private void Awake()
    {
        originalPositions = new Vector3[letters.Length];

        for (int i = 0; i < letters.Length; i++)
        {
            originalPositions[i] = letters[i].transform.localPosition;
        }
}
    

    private void Start()
    {
        if (playOnStart)
        {
            WaveText();
        }
    }

    public void WaveText()
    {
        for (int i = 0; i < letters.Length; i++) 
        {
            Transform letter = letters[i].transform;

            letter.DOKill(true);


            if (loop == true)
            {
                letter.DOLocalMoveY(originalPositions[i].y + waveHeight, waveDuration, true).SetDelay(i * letterDelay).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Normal, true);
            }
            else
            {
                letter.DOLocalMoveY(originalPositions[i].y + waveHeight, waveDuration, true).SetDelay(i * letterDelay).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Normal, true);
            }
        }
    }    
}

