using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthComponent
{
    public float maxHealth { get; }
    public float currentHealth { get; }

    public void SufferDamange(float damage);
    public void Die();
    public void Heal(float health);
}