using UnityEngine;

public class StrikerMovement : MonoBehaviour
{
    [Header("Player 1 Zone")]
    public Transform player1Spawn;   // Empty object for Y reference
    public float p1MinX = -1.5f;
    public float p1MaxX = 1.5f;

    [Header("Player 2 Zone")]
    public Transform player2Spawn;
    public float p2MinX = -1.5f;
    public float p2MaxX = 1.5f;

    [Header("Shot Settings")]
    public float maxForce = 10f;
    public float maxDragDistance = 2f;

    private Rigidbody2D rb;
    private bool isPlacing = true;
    private bool isDragging = false;
    private Vector2 dragStart;
    private int currentPlayer = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        MoveToSpawn(player1Spawn);
    }

    void Update()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ===== Positioning Phase =====
        if (isPlacing)
        {
            if (Input.GetMouseButton(0))
            {
                // Choose limits based on current player
                float minX = (currentPlayer == 1) ? p1MinX : p2MinX;
                float maxX = (currentPlayer == 1) ? p1MaxX : p2MaxX;
                float launchY = (currentPlayer == 1) ? player1Spawn.position.y
                                                     : player2Spawn.position.y;

                // Clamp X inside launch zone
                float clampedX = Mathf.Clamp(mouseWorld.x, minX, maxX);
                transform.position = new Vector2(clampedX, launchY);
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                isPlacing = false;
            }
        }
        // ===== Drag & Shoot =====
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragStart = mouseWorld;
                isDragging = true;
                rb.linearVelocity = Vector2.zero;
            }

            if (isDragging && Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                Vector2 dragVector = dragStart - mouseWorld;
                if (dragVector.magnitude > maxDragDistance)
                    dragVector = dragVector.normalized * maxDragDistance;

                rb.AddForce(dragVector * maxForce, ForceMode2D.Impulse);
            }
        }
    }

    // ===== Public Reset =====
    public void ResetForNextPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        currentPlayer = (currentPlayer == 1) ? 2 : 1;

        MoveToSpawn(currentPlayer == 1 ? player1Spawn : player2Spawn);
        isPlacing = true;
        isDragging = false;
    }

    private void MoveToSpawn(Transform spawn)
    {
        if (spawn != null)
            transform.position = spawn.position;
        else
            Debug.LogWarning("CarromStriker: Missing spawn point!");
    }
}

//using Fusion;
//using UnityEngine;

//public class StrikerMovement : NetworkBehaviour
//{
//    [Header("Player 1 Zone")]
//    public Transform player1Spawn;
//    public float p1MinX = -1.5f;
//    public float p1MaxX = 1.5f;

//    [Header("Player 2 Zone")]
//    public Transform player2Spawn;
//    public float p2MinX = -1.5f;
//    public float p2MaxX = 1.5f;

//    [Header("Shot Settings")]
//    public float maxForce = 10f;
//    public float maxDragDistance = 2f;

//    private Rigidbody2D rb;
//    private bool isPlacing = true;
//    private bool isDragging = false;
//    private Vector2 dragStart;

//    [Networked] private int CurrentPlayer { get; set; } = 1; // 1 = host, 2 = joiner

//    public override void Spawned()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.gravityScale = 0;
//        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

//        if (Object.HasStateAuthority)
//            MoveToSpawn(player1Spawn);
//    }

//    void Update()
//    {
//        // Only allow the current player to control the striker
//        if (!HasInputAuthority) return;

//        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        // ===== Positioning Phase =====
//        if (isPlacing)
//        {
//            if (Input.GetMouseButton(0))
//            {
//                float minX = (CurrentPlayer == 1) ? p1MinX : p2MinX;
//                float maxX = (CurrentPlayer == 1) ? p1MaxX : p2MaxX;
//                float launchY = (CurrentPlayer == 1) ? player1Spawn.position.y : player2Spawn.position.y;

//                float clampedX = Mathf.Clamp(mouseWorld.x, minX, maxX);
//                transform.position = new Vector2(clampedX, launchY);
//            }

//            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
//                isPlacing = false;
//        }
//        else
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                dragStart = mouseWorld;
//                isDragging = true;
//                rb.linearVelocity = Vector2.zero;
//            }

//            if (isDragging && Input.GetMouseButtonUp(0))
//            {
//                isDragging = false;
//                Vector2 dragVector = dragStart - mouseWorld;
//                if (dragVector.magnitude > maxDragDistance)
//                    dragVector = dragVector.normalized * maxDragDistance;

//                rb.AddForce(dragVector * maxForce, ForceMode2D.Impulse);

//                // End turn after shot (Host decides)
//                if (Object.HasStateAuthority)
//                    Invoke(nameof(EndTurn), 2f);
//            }
//        }
//    }

//    private void EndTurn()
//    {
//        if (!Object.HasStateAuthority) return;

//        // Switch player
//        CurrentPlayer = (CurrentPlayer == 1) ? 2 : 1;

//        // Respawn striker at correct player spawn
//        MoveToSpawn(CurrentPlayer == 1 ? player1Spawn : player2Spawn);

//        // Reset flags
//        isPlacing = true;
//        isDragging = false;

//        // Update InputAuthority
//        UpdateInputAuthority();
//    }

//    private void UpdateInputAuthority()
//    {
//        if (Runner == null) return;

//        PlayerRef newPlayer = (CurrentPlayer == 1) ? Runner.LocalPlayer : Runner.ActivePlayers[1];
//        Object.AssignInputAuthority(newPlayer);
//    }

//    private void MoveToSpawn(Transform spawn)
//    {
//        if (spawn != null)
//            transform.position = spawn.position;
//        else
//            Debug.LogWarning("CarromStriker: Missing spawn point!");
//    }
//}