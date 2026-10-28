using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlightManager : MonoBehaviour
{
    public static TileHighlightManager Instance;

    [Header("Highlight Prefab")]
    public GameObject highlightPrefab;

    private List<GameObject> activeHighlights = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void HighlightTileRed(Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, -0.1f);
        GameObject highlight = Instantiate(highlightPrefab, worldPos, Quaternion.identity);
        activeHighlights.Add(highlight);
    }

    // Toggles active highlights on and off rapidly to create a flashing hazard effect
    public IEnumerator FlashHighlights(float totalDuration, float flashInterval = 0.15f)
    {
        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < totalDuration)
        {
            isVisible = !isVisible;
            foreach (GameObject highlight in activeHighlights)
            {
                if (highlight != null)
                    highlight.SetActive(isVisible);
            }

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        // Ensure all highlights are visible right before the strike
        foreach (GameObject highlight in activeHighlights)
        {
            if (highlight != null)
                highlight.SetActive(true);
        }
    }

    public void ClearHighlight()
    {
        foreach (GameObject highlight in activeHighlights)
        {
            Destroy(highlight);
        }
        activeHighlights.Clear();
    }
}