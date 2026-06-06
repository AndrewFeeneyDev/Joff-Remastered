/* Written by Andrew Feeney. Applies DOTween effects to buttons */
using DG.Tweening;
using UnityEngine;

public class UIButtonSlave : MonoBehaviour
{
    // Punch Variables
    [SerializeField] private Transform punchTransform;
    [HideInInspector] public Vector3 punchScale;
    [HideInInspector] public float punchDuration;
    [HideInInspector] public int punchVibrato;
    [HideInInspector] public float punchElasticity;
    [HideInInspector] public Ease punchEase;
    private Tween punchTween;

    // Scale Variables
    [SerializeField] private Transform scaleTransform;
    [HideInInspector] public Vector3 scaleUp;
    [HideInInspector] public float scaleUpDuration;
    [HideInInspector] public Vector3 scaleDown;
    [HideInInspector] public float scaleDownDuration;
    [HideInInspector] public Ease scaleEase;
    private Tween scaleUpTween;
    private Tween scaleDownTween;

    // Punch Effect
    public void PunchEffect()
    {
        punchTween = punchTransform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElasticity).SetEase(punchEase).SetUpdate(UpdateType.Normal, true);
        Invoke("PunchRewind", punchDuration);
    }

    // Scale Up Effect
    public void ScaleUpEffect()
    {
        scaleUpTween = scaleTransform.DOScale(scaleUp, scaleUpDuration).SetEase(scaleEase).SetUpdate(UpdateType.Normal, true);
    }

    // Scale Down Effect
    public void ScaleDownEffect()
    {
        scaleTransform.DOScale(scaleDown, scaleDownDuration).SetEase(scaleEase).SetUpdate(UpdateType.Normal, true);
    }

    // Punch Rewind
    private void PunchRewind()
    {
        punchTween.Rewind();
    } 
}
