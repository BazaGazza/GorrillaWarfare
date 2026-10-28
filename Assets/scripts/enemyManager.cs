using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab; // Drag your Enemy Unit Prefab here in Inspector

    public const int BOARD_SIZE = 8;

    // Call this function when you want to spawn an enemy
    public void SpawnEnemy(Vector2Int gridPosition)
    {
        // 1. Calculate the centered world position (+0.5f offset)
        Vector3 spawnPos = new Vector3(gridPosition.x + 0.5f, gridPosition.y + 0.5f, 0f);

        // 2. Instantiate (create) the enemy GameObject on the board
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // 3. Set the enemy's grid position if it has a movement script attached
        GridPlayerController enemyMovement = newEnemy.GetComponent<GridPlayerController>();
        if (enemyMovement != null)
        {
            enemyMovement.gridPosition = gridPosition;
        }
    }
}