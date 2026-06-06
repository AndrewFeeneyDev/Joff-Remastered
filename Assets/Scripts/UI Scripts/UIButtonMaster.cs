/* Written by Andrew Feeney. Sets the punch and scale effects for all buttons using UIButtonSlave.cs */
using DG.Tweening;
using UnityEngine;

public class UIButtonMaster : MonoBehaviour
{
    // Punch Variables
    [Header("Button Punch Effect")]
    [SerializeField] private Vector3 buttonPunchScale;
    [SerializeField] private float buttonPunchDuration;
    [SerializeField] private int buttonPunchVibrato;
    [SerializeField] private float buttonPunchElasticity;
    [SerializeField] private Ease buttonPunchEase;

    // Scale Variables
    [Header("Button Scale Effect")]
    [SerializeField] private Vector3 buttonScaleUp;
    [SerializeField] private float buttonScaleUpDuration;
    [SerializeField] private Vector3 buttonScaleDown;
    [SerializeField] private float buttonScaleDownDuration;
    [SerializeField] private Ease buttonScaleEase;

    private void Awake()
    {
        foreach (var effect in GetComponentsInChildren<UIButtonSlave>())
        {
            effect.punchScale = buttonPunchScale;
            effect.punchDuration = buttonPunchDuration;
            effect.punchVibrato = buttonPunchVibrato;
            effect.punchElasticity = buttonPunchElasticity;
            effect.punchEase = buttonPunchEase;
            effect.scaleUp = buttonScaleUp;
            effect.scaleUpDuration = buttonScaleUpDuration;
            effect.scaleDown = buttonScaleDown;
            effect.scaleDownDuration = buttonScaleDownDuration;
            effect.scaleEase = buttonScaleEase;
        }
    }
}
