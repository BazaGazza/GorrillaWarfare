using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance;

    [Header("Heart UI References")]
    public List<Image> heartImages = new List<Image>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateHearts(int currentHP, int maxHP)
    {
        // 1. Check if the list was assigned in the Inspector
        if (heartImages == null || heartImages.Count == 0)
        {
            Debug.LogError("[HealthUI] 'heartImages' list is empty in the Inspector! Drag your heart UI images into the list.");
            return;
        }

        Debug.Log($"[HealthUI] Updating UI -> Current HP: {currentHP}. Total Heart Objects in list: {heartImages.Count}");

        for (int i = 0; i < heartImages.Count; i++)
        {
            bool shouldBeActive = (i < currentHP);

            if (heartImages[i] != null)
            {
                // Disable/Enable the image
                heartImages[i].gameObject.SetActive(shouldBeActive);
                Debug.Log($"[HealthUI] Heart index {i} set to active: {shouldBeActive}");
            }
            else
            {
                Debug.LogWarning($"[HealthUI] Element at index {i} in heartImages list is NULL!");
            }
        }
    }
}