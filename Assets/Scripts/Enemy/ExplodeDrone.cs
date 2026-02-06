using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplodeDrone : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float explosionDelay;

    private Coroutine explodeRoutine;

    private SpriteRenderer sprite;
    private Color startColor;

    public float timer;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        startColor = sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && explodeRoutine == null)
        {
            explodeRoutine = StartCoroutine(ExplodeRoutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CancelExplodeRoutine();
        }
    }

    IEnumerator ExplodeRoutine()
    {

        timer = 0f;

        while (timer < explosionDelay)
        {
            timer += Time.deltaTime;

            float t = timer / explosionDelay;
            sprite.color = Color.Lerp(startColor, Color.white, t);

            yield return null;
        }

        Explode();
    }

    void CancelExplodeRoutine()
    {
        if (timer >= explosionDelay / 2) return;
        if (explodeRoutine != null)
        {
            StopCoroutine(explodeRoutine);
            explodeRoutine = null;
        }

        sprite.color = startColor;
        timer = 0;
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D hit in hits)
        {
            IHealthComponent healthComp = hit.GetComponent<IHealthComponent>();
            if (healthComp != null)
            {
                healthComp.SufferDamange(damage);
            }
        }

        IHealthComponent self = GetComponent<IHealthComponent>();
        self.Die();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}