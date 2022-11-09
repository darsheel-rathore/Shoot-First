using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPun
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Color fullHealthColor;
    [SerializeField] private Color zeroHealthColor;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    private float currentHealth = 100;
    private bool isDead = false;


    private void Start()
    {
        //RestoreHealth();
        SetHealthUI();
    }


    [PunRPC]
    public void TakeDamage(float damageAmount)
    {
        UpdateHealthRPC(damageAmount);
    }

    private void UpdateHealthRPC(float damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);

        SetHealthUI();

        if (currentHealth <= 0) Die();
    }

    private void SetHealthUI()
    {
        healthSlider.value = currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / maxHealth);
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
