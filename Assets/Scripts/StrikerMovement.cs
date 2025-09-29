using UnityEngine;

public class StrikerMovement : MonoBehaviour
{

    //[Header("Positioning")]
    //public float minX = -1.5f;      // Left limit of launch zone
    //public float maxX = 1.5f;      // Right limit of launch zone
    //public float launchY = -3.5f;   // Fixed Y line of launch zone

    //[Header("Shot Settings")]
    //public float maxForce = 10f;       // Max shooting force
    //public float maxDragDistance = 2f; // Limit drag distance

    //private Rigidbody2D rb;
    //private bool isPlacing = true;   // true = positioning phase
    //private bool isDragging = false; // true = aiming/dragging phase
    //private Vector2 dragStart;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    rb.gravityScale = 0;
    //    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    //    // Start at center of launch line
    //    transform.position = new Vector2(0f, launchY);
    //}

    //void Update()
    //{
    //    Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    // ===== 1. Positioning Phase =====
    //    if (isPlacing)
    //    {
    //        if (Input.GetMouseButton(0))
    //        {
    //            // Move only along X axis within launch zone
    //            float clampedX = Mathf.Clamp(mouseWorld.x, minX, maxX);
    //            transform.position = new Vector2(clampedX, launchY);
    //        }
    //        // Right-click OR press Space to lock position and start aiming
    //        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
    //        {
    //            isPlacing = false;
    //        }
    //    }
    //    // ===== 2. Aiming / Shooting Phase =====
    //    else
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            dragStart = mouseWorld;
    //            isDragging = true;
    //            rb.linearVelocity = Vector2.zero;
    //        }

    //        if (isDragging && Input.GetMouseButtonUp(0))
    //        {
    //            isDragging = false;
    //            Vector2 dragVector = dragStart - mouseWorld;

    //            // Limit drag distance
    //            if (dragVector.magnitude > maxDragDistance)
    //                dragVector = dragVector.normalized * maxDragDistance;

    //            rb.AddForce(dragVector * maxForce, ForceMode2D.Impulse);

    //            // After shot you can reset if needed
    //            // StartCoroutine(ResetForNextTurn());
    //        }
    //    }
    //}

    //// Optional reset for next turn
    //public void ResetStriker()
    //{
    //    rb.linearVelocity = Vector2.zero;
    //    transform.position = new Vector2(0f, launchY);
    //    isPlacing = true;
    //    isDragging = false;
    //}




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
