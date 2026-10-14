using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public const int BOARD_SIZE = 8;

    // Stores which gorilla/entity is standing on which tile
    private GameObject[,] gridOccupancy = new GameObject[BOARD_SIZE, BOARD_SIZE];

    // 1. Check if a coordinate is within the 8x8 playable area
    public bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < BOARD_SIZE && pos.y >= 0 && pos.y < BOARD_SIZE;
    }

    // 2. Check if a tile is already occupied by an entity
    public bool IsTileOccupied(Vector2Int pos)
    {
        if (!IsWithinBounds(pos)) return true; // Out-of-bounds counts as blocked
        return gridOccupancy[pos.x, pos.y] != null;
    }

    // 3. Move an entity to a new tile if valid
    public bool TryMoveEntity(GameObject entity, Vector2Int currentPos, Vector2Int targetPos)
    {
        if (!IsWithinBounds(targetPos) || IsTileOccupied(targetPos))
        {
            Debug.Log("Invalid move!");
            return false;
        }

        // Clear previous tile and assign entity to new tile
        gridOccupancy[currentPos.x, currentPos.y] = null;
        gridOccupancy[targetPos.x, targetPos.y] = entity;

        return true;
    }
}