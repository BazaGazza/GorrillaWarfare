using UnityEngine;

public class GridPlayerController : MonoBehaviour
{
    [Header("Grid Position")]
    public Vector2Int gridPosition = new Vector2Int(0, 0); // Starting cell (0,0)
    public const int BOARD_SIZE = 8;

    void Start()
    {
        UpdateWorldPosition();
    }

    void Update()
    {
        // Listen for single arrow key presses
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMove(new Vector2Int(0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMove(new Vector2Int(0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove(new Vector2Int(-1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMove(new Vector2Int(1, 0));
        }
    }

    void TryMove(Vector2Int direction)
    {
        Vector2Int targetPos = gridPosition + direction;

        // Keep movement strictly inside the 8x8 grid (0 to 7 on X and Y)
        if (targetPos.x >= 0 && targetPos.x < BOARD_SIZE &&
            targetPos.y >= 0 && targetPos.y < BOARD_SIZE)
        {
            gridPosition = targetPos;
            UpdateWorldPosition();
        }
    }

    // Centers the 1x1 sprite inside the tile cell
    void UpdateWorldPosition()
    {
        transform.position = new Vector3(gridPosition.x + 0.5f, gridPosition.y + 0.5f, 0f);
    }
}