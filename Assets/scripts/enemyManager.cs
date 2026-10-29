using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab; // Drag your Poacher Enemy Prefab here in the Inspector

    public const int BOARD_SIZE = 8;

    [Header("Spawn Settings")]
    public bool spawnOnStart = true;
    public int initialEnemies = 2;    // How many poachers spawn when game starts
    public float spawnInterval = 10f; // Seconds between automatic spawns (10 seconds)
    public int maxEnemies = 5;        // Maximum allowed poachers on board at once

    private float timer = 0f;

    void Start()
    {
        if (spawnOnStart)
        {
            for (int i = 0; i < initialEnemies; i++)
            {
                SpawnEnemyAtRandomPosition();
            }
        }
    }

    void Update()
    {
        // 1. Add time passed since last frame
        timer += Time.deltaTime;

        // 2. Trigger spawn every 10 seconds
        if (timer >= spawnInterval)
        {
            timer = 0f; // Reset timer back to 0

            // 3. Only spawn if we haven't reached the maximum enemy cap
            int currentEnemyCount = FindObjectsByType<PoacherMovement>(FindObjectsSortMode.None).Length;
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemyAtRandomPosition();
                Debug.Log("[EnemySpawner] 10 seconds passed! Spawned a new poacher.");
            }
        }
    }

    // Call this function to pick a random tile and spawn a poacher
    public void SpawnEnemyAtRandomPosition()
    {
        Vector2Int randomGridPos = GetRandomTilePosition();
        SpawnEnemy(randomGridPos);
    }

    // Picks a random tile (0-7) and avoids placing it directly on the player
    Vector2Int GetRandomTilePosition()
    {
        GridPlayerController player = FindFirstObjectByType<GridPlayerController>();
        Vector2Int randomPos;
        int maxAttempts = 20;
        int attempts = 0;

        do
        {
            int x = Random.Range(0, BOARD_SIZE);
            int y = Random.Range(0, BOARD_SIZE);
            randomPos = new Vector2Int(x, y);
            attempts++;
        } 
        while (player != null && randomPos == player.gridPosition && attempts < maxAttempts);

        return randomPos;
    }

    // Spawns an enemy at a specific grid position
    public void SpawnEnemy(Vector2Int gridPosition)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("[EnemySpawner] Enemy Prefab is missing! Drag your enemy prefab into the Inspector.");
            return;
        }

        Vector3 spawnPos = new Vector3(gridPosition.x + 0.5f, gridPosition.y + 0.5f, 0f);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        PoacherMovement poacher = newEnemy.GetComponent<PoacherMovement>();
        if (poacher != null)
        {
            poacher.gridPosition = gridPosition;
            poacher.UpdateWorldPosition(); // Snaps sprite to tile center
        }
    }
}
