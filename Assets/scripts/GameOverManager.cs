using UnityEngine;
using UnityEngine.SceneManagement; // 1. Must include to reload scenes

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI Reference")]
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Displays the Game Over screen
    public void ShowGameOver()
    {
         Debug.Log("[showing screen!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Optional: Freeze gameplay so units stop moving
            Time.timeScale = 0f; 
        }
    }

    // Called by the Restart Button to reload the scene
    public void RestartGame()
    {
        // Always reset time scale back to 1.0 before reloading
        Time.timeScale = 1f;

        // Reload the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}