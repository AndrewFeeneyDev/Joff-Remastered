using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CoinLogic : MonoBehaviour
{
    private CircleCollider2D collider;
    private Rigidbody2D rb2d;
    private SpriteRenderer renderer;
    private Light2D light;

    [SerializeField] private ParticleSystem particleIdle;
    [SerializeField] private ParticleSystem particleBurst;

    [Header("Idle Bounce Effect")]
    [SerializeField] private float idleEnd = 1f;
    [SerializeField] private float idleDuration = 1f;
    private float idleStart;
    private Tween coinIdle;

    [Header("Collision Effect")]
    [SerializeField] private float effectForce = 10f;
    [SerializeField] private float effectDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = gameObject.GetComponent<CircleCollider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        light = gameObject.GetComponentInChildren<Light2D>();

        idleStart = rb2d.position.y;
        IdleCoin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Coin());
        }
    }

    private IEnumerator Coin()
    {
        collider.enabled = false;
        coinIdle.Kill();
        particleIdle.Stop();
        light.intensity = 0f;

        rb2d.AddForceY(effectForce);
        rb2d.gravityScale = 1;

        renderer.DOFade(0, effectDuration);

        yield return new WaitUntil(() => rb2d.linearVelocity.y > 0);

        yield return new WaitUntil(() => rb2d.linearVelocity.y <= 0);
        
        particleBurst.Play();

        yield return new WaitForSeconds(particleBurst.main.duration);

        Destroy(gameObject);
    }

    private void IdleCoin()
    {
        coinIdle = rb2d.DOMoveY(idleStart + idleEnd, idleDuration).SetLoops(-1, LoopType.Yoyo).SetUpdate(UpdateType.Fixed);
    }
}
