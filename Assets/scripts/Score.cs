using UnityEngine;
using TMPro; // 1. Must include the TextMeshPro namespace

public class GameTimer : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI timerText; // Drag your TextMeshPro UI component here in the Inspector

    private float elapsedTime = 0f;
    private bool isTimerRunning = true;

    void Start()
    {
        // Auto-get component if attached directly to the TextMeshPro GameObject
        if (timerText == null)
        {
            timerText = GetComponent<TextMeshProUGUI>();
        }
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            // Add the time passed since the last frame
            elapsedTime += Time.deltaTime;

            
            // Option B: If you just want total seconds passed (e.g., 65s)
            timerText.text = "Time: " + Mathf.FloorToInt(elapsedTime).ToString();
        }
    }

    // Public functions to control the timer if needed later
    public void PauseTimer() => isTimerRunning = false;
    public void ResumeTimer() => isTimerRunning = true;
    public void ResetTimer() => elapsedTime = 0f;
}