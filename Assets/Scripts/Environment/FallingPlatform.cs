using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Platform Hover Settings")]
    private float hoverStart;
    [SerializeField] private float hoverEnd = 1f;
    [SerializeField] private float hoverDuration = 1f;
    private Tween platformHover;


    [Header("Platform Fall Settings")]
    private float shakeStart;
    [SerializeField] private float shakeEnd = 1f;
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float fallDelay = 1f;
    private Tween platformShake;

    private Rigidbody2D rb2d;
    private bool hasFallen = false;

    private ParticleSystem platformEffects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        platformEffects = gameObject.GetComponentInChildren<ParticleSystem>();

        hoverStart = rb2d.position.y;
        shakeStart = rb2d.position.x;

        IdlePlatform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IdlePlatform()
    {
        platformHover = rb2d.DOMoveY(hoverStart + hoverEnd, hoverDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(UpdateType.Fixed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasFallen)
        {
            StartCoroutine(FallSequence());
        }
    }

    private IEnumerator FallSequence()
    {
        if (platformHover != null && platformHover.IsActive())
        {
            platformHover.Kill();
        }

        platformShake = rb2d.DOMoveX(shakeStart + shakeEnd, shakeDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(UpdateType.Fixed);

        yield return new WaitForSeconds(fallDelay);

        platformShake.Kill();
        rb2d.gravityScale = 1f;
        rb2d.constraints = RigidbodyConstraints2D.None;
        platformEffects.Stop();
        hasFallen = true;

        yield return null;
    }
}
