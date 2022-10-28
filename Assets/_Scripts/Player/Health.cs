using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        RestoreHealth();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;

        Debug.Log("Dead -" + this.gameObject);
        gameObject.SetActive(false);
        isDead = true;
    }

    private void RestoreHealth() => currentHealth = maxHealth;
}
