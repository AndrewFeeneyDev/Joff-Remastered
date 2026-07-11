using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth = 5;
    [SerializeField] private float immunityTimer = 0.15f;
    [SerializeField] private Image healthBar;
    [SerializeField] private Sprite[] healthBars;
    private bool isImmune = false;
    private bool isDead = false;
    private SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealth();

        if (healthBars.Length < 6)
        {
            Debug.Log("Need to assign all health bars to PlayerHealth.cs");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.CompareTag("Enemy") && !isImmune)
            {
                currentHealth --;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                DamageFlash();
                UpdateHealth();
                StartCoroutine(Immunity());
            }
            if (collision.CompareTag("Void"))
            {
                currentHealth = 0;
                DamageFlash();
                UpdateHealth();
            }
            if (collision.CompareTag("Health"))
            {
                currentHealth = maxHealth;
                UpdateHealth();
            }
        }
    }

    private void UpdateHealth()
    {
        switch (currentHealth)
        {
            case 0:
                healthBar.sprite = healthBars[0];
                Death();
                break;
            case 1:
                healthBar.sprite = healthBars[1];
                break;
            case 2:
                healthBar.sprite = healthBars[2];
                break;
            case 3:
                healthBar.sprite = healthBars[3];
                break;
            case 4:
                healthBar.sprite = healthBars[4];
                break;
            case 5:
                healthBar.sprite = healthBars[5];
                break;
        }
    }

    private void Death()
    {
        isDead = true;
        StartCoroutine(DeathSequence());

        // GameOverUI
    }

    private IEnumerator DeathSequence()
    {
        Debug.Log("Player Dead");

        yield return new WaitForSeconds(0.2f);

        PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerMovement.enabled = false;

        // Death Anim

        UIGameOver.Instance.GameOver();

        yield return null;
    }

    private IEnumerator Immunity()
    {
        isImmune = true;

        yield return new WaitForSeconds(immunityTimer);

        isImmune = false;

        yield return null;
    }

    private void DamageFlash()
    {
        playerSprite.DOKill();

        Sequence flash = DOTween.Sequence();

        flash.Append(playerSprite.DOColor(Color.red, 0.05f));
        flash.Append(playerSprite.DOColor(Color.white, 0.05f));
        flash.Append(playerSprite.DOColor(Color.red, 0.05f));
        flash.Append(playerSprite.DOColor(Color.white, 0.05f));
    }
}
