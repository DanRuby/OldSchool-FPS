using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float maxHealth;

    private float currentHealth;
    public static event System.Action<float> PlayerHealthChanged;
    public static event System.Action PlayerHit;
    public static event System.Action PlayerDied;

    private CharacterMovement characterMovement;
    private const float TOLERANCE=0.2f;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        currentHealth = maxHealth;
        PlayerHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        PlayerHit?.Invoke();
        currentHealth -= damage;
        PlayerHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
            PlayerDied?.Invoke();
    }

    public bool Heal(float heal)
    {
        if (Math.Abs(currentHealth - maxHealth) < TOLERANCE)
            return true;
        currentHealth=Mathf.Clamp(currentHealth+heal,0,maxHealth);
        PlayerHealthChanged?.Invoke(currentHealth);

        return false;
    }

    public void TakeDamage(float damage, float expForce, float expRadius, Vector3 expPosition)
    {
        TakeDamage(damage);
        characterMovement.AddExpForce(expForce, expRadius, expPosition);
    }
}
