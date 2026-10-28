using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public float damageFlashDuration = 0.2f;

    void Start()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateHealthUI();
    }

    // Call this whenever the player takes damage
    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // Already defeated

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"[Player] Took {damageAmount} damage! Current HP: {currentHealth}/{maxHealth}");

        // Update UI
        UpdateHealthUI();

        // Flash sprite RED when hit
        StartCoroutine(DamageFlash());

        // Check for defeat
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
    if (HealthUI.Instance != null)
        {
        HealthUI.Instance.UpdateHearts(currentHealth, maxHealth);
        }
    }

    IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(damageFlashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        Debug.Log("[Player] Gorilla was defeated!");
        
        // TODO: Trigger Game Over screen or reload level
        // Example: GameOverManager.Instance.ShowGameOver();
    }
}