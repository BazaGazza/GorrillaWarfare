using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance;

    [Header("Heart Sprites")]
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    [Header("Heart UI References")]
    public List<Image> heartImages; // Drag your 3 Heart UI Images here in Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Updates heart sprites based on current player HP
    public void UpdateHearts(int currentHP, int maxHP)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHP)
            {
                // Show full heart sprite
                heartImages[i].sprite = fullHeartSprite;
                heartImages[i].enabled = true;
            }
            else if (i < maxHP)
            {
                // Show empty heart sprite
                heartImages[i].sprite = emptyHeartSprite;
                heartImages[i].enabled = true;
            }
            else
            {
                // Hide extra heart slots beyond max HP
                heartImages[i].enabled = false;
            }
        }
    }
}