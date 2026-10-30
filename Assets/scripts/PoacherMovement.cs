using System.Collections;
using UnityEngine;

public class PoacherMovement : MonoBehaviour
{
    [Header("Grid Position & Limits")]
    public Vector2Int gridPosition; // Auto-detected on Start from initial placement
    public const int BOARD_SIZE = 8;
    public int sightRange = 3; // Number of tiles ahead the poacher can scan

    [Header("Movement & Sliding")]
    public float moveSpeed = 5f;        // Speed of the smooth tile-sliding animation
    public float moveInterval = 3.0f;    // Seconds between random steps when wandering
    public float attackDelay = 1.0f;     // Seconds to wait after lighting up red tile

    private float timer = 0f;
    private bool isAttacking = false;
    private Coroutine moveCoroutine;
    private GridPlayerController player;

    void Start()
    {
        // 1. Find the player unit in the scene
        player = FindFirstObjectByType<GridPlayerController>();

        // 2. Read starting grid position from initial transform
        Vector3 worldPos = transform.position;
        gridPosition = new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));

        // 3. Snap precisely to tile center on spawn
        transform.position = GetWorldPosition(gridPosition);
    }

    void Update()
    {
        // Pause wandering if poacher is currently telegraphing an attack or sliding
        if (isAttacking) return;

        // 1. First, check for line-of-sight to the player
        Vector2Int sightDirection = CheckSightline();

        if (sightDirection != Vector2Int.zero)
        {
            // Player spotted! Begin attack wind-up sequence
            StartCoroutine(PrepareAndAttack(sightDirection));
        }
        else
        {
            // 2. Player not seen -> Count down to next random wander step
            timer += Time.deltaTime;

            if (timer >= moveInterval)
            {
                timer = 0f;
                MoveRandomly();
            }
        }
    }

    // Scans in 4 cardinal directions (Up, Down, Left, Right) up to sightRange
    Vector2Int CheckSightline()
    {
        if (player == null) return Vector2Int.zero;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            for (int step = 1; step <= sightRange; step++)
            {
                Vector2Int checkPos = gridPosition + (dir * step);

                // Stop scanning if line-of-sight goes out of board bounds
                if (checkPos.x < 0 || checkPos.x >= BOARD_SIZE || checkPos.y < 0 || checkPos.y >= BOARD_SIZE)
                    break;

                // Player spotted!
                if (checkPos == player.gridPosition)
                {
                    return dir;
                }
            }
        }

        return Vector2Int.zero;
    }

    // Telegraphs attack, waits 1 second, then checks if player failed to move away
    IEnumerator PrepareAndAttack(Vector2Int attackDirection)
    {
    isAttacking = true;

    // 1. Highlight all 3 tiles along the attack line of sight
    for (int i = 1; i <= sightRange; i++)
    {
        Vector2Int targetTile = gridPosition + (attackDirection * i);

        if (targetTile.x >= 0 && targetTile.x < BOARD_SIZE &&
            targetTile.y >= 0 && targetTile.y < BOARD_SIZE)
        {
            if (TileHighlightManager.Instance != null)
            {
                TileHighlightManager.Instance.HighlightTileRed(targetTile);
            }
        }
    }

    // 2. Flash the tiles rapidly during the attack delay (1 second)
    if (TileHighlightManager.Instance != null)
    {
        yield return StartCoroutine(TileHighlightManager.Instance.FlashHighlights(attackDelay, 0.15f));
    }
    else
    {
        yield return new WaitForSeconds(attackDelay);
    }

    // 3. Check if player is sitting on ANY of the 3 targeted tiles
    bool playerHit = false;
    for (int i = 1; i <= sightRange; i++)
    {
        Vector2Int targetTile = gridPosition + (attackDirection * i);
        if (player != null && player.gridPosition == targetTile)
        {
            playerHit = true;
            break;
        }
    }

    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.poacherTelegraphSound);

        
    }
    if (playerHit)
    {
        Debug.Log("[Poacher] Player caught in attack line! Dealing damage!");
        // TODO: Call player damage logic
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1); // Deals 1 damage
        }
    }
    else
    {
        Debug.Log("[Poacher] Attack missed! Player dodged out of the line.");
    }

    // 4. Clear all red tile highlights
    AudioManager.Instance.PlaySFX(AudioManager.Instance.playerHitSound);

    
    if (TileHighlightManager.Instance != null)
    {
        TileHighlightManager.Instance.ClearHighlight();
    }

    timer = 0f;
    isAttacking = false;
    }

    void MoveRandomly()
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        Vector2Int randomDir = directions[Random.Range(0, directions.Length)];

        TryMove(randomDir);
    }

    void TryMove(Vector2Int direction)
    {
        Vector2Int targetPos = gridPosition + direction;

        // Keep move within 8x8 grid bounds
        if (targetPos.x >= 0 && targetPos.x < BOARD_SIZE &&
            targetPos.y >= 0 && targetPos.y < BOARD_SIZE)
        {
            gridPosition = targetPos;
            UpdateWorldPosition();
        }
    }

    // Calculates centered tile world position (+0.5f offset)
    Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0f);
    }

    // Triggers smooth gliding animation to current gridPosition
    public void UpdateWorldPosition()
    {
        Vector3 targetWorldPos = GetWorldPosition(gridPosition);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(SmoothSlide(targetWorldPos));
    }

    // Glides sprite frame-by-frame toward target tile
    IEnumerator SmoothSlide(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
    }
}