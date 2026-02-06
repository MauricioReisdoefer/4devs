using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : MonoBehaviour, IHealthComponent
{
    public float maxHealth => MaxHealth;
    [SerializeField] private float MaxHealth = 5f;

    public float currentHealth => CurrentHealth;
    [SerializeField] private float CurrentHealth;

    [Header("Visual Damage Blink")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float blinkDuration = 0.1f;
    [SerializeField] private Color blinkColor;

    [Header("Death Effects")]
    [SerializeField] private GameObject deathParticlesPrefab;

    private Color baseColor;

    private bool isBlinking;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        baseColor = spriteRenderer.color;
    }

    public void Heal(float health)
    {
        CurrentHealth += health;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
    }

    public void SufferDamange(float damage)
    {
        CurrentHealth -= damage;

        if (!isBlinking)
            StartCoroutine(Blink());

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (deathParticlesPrefab != null)
        {
            GameObject particles =
                Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);

            Destroy(particles, 3f);
        }

        Destroy(gameObject);
    }

    private IEnumerator Blink()
    {
        isBlinking = true;


        spriteRenderer.color = blinkColor;
        yield return new WaitForSeconds(blinkDuration);

        spriteRenderer.color = baseColor;
        yield return new WaitForSeconds(blinkDuration);


        isBlinking = false;
    }
}
