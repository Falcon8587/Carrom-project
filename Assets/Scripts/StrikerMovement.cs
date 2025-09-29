using UnityEngine;

public class StrikerMovement : MonoBehaviour
{
    //public float maxForce = 10f;       // Max shooting force
    //public float maxDragDistance = 2f; // Limit how far you can pull back

    //private Rigidbody2D rb;
    //private Vector2 startPos;
    //private Vector2 dragStart;
    //private bool isDragging = false;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    startPos = transform.position; // starting position of striker
    //}

    //void Update()
    //{
    //    // Start Drag
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        if (Vector2.Distance(mouseWorld, transform.position) < 0.5f) // click near striker
    //        {
    //            isDragging = true;
    //            dragStart = mouseWorld;
    //            rb.linearVelocity = Vector2.zero;  // stop any current movement
    //        }
    //    }

    //    // While Dragging
    //    if (isDragging && Input.GetMouseButton(0))
    //    {
    //        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2 dragVector = mouseWorld - startPos;

    //        // Limit drag distance
    //        if (dragVector.magnitude > maxDragDistance)
    //            dragVector = dragVector.normalized * maxDragDistance;

    //        // Move striker opposite to drag direction
    //        transform.position = startPos + dragVector;
    //    }

    //    // Release to Shoot
    //    if (isDragging && Input.GetMouseButtonUp(0))
    //    {
    //        isDragging = false;
    //        Vector2 force = (startPos - (Vector2)transform.position) * maxForce;
    //        rb.AddForce(force, ForceMode2D.Impulse);

    //        // Reset striker position after shot if needed
    //        // StartCoroutine(ResetAfterDelay());
    //    }
    //}

    //// Optional Reset (call after pocket or foul)
    //public void ResetStriker()
    //{
    //    rb.linearVelocity = Vector2.zero;
    //    transform.position = startPos;
    //}


    [Header("Positioning")]
    public float minX = -1.5f;      // Left limit of launch zone
    public float maxX = 1.5f;      // Right limit of launch zone
    public float launchY = -3.5f;   // Fixed Y line of launch zone

    [Header("Shot Settings")]
    public float maxForce = 10f;       // Max shooting force
    public float maxDragDistance = 2f; // Limit drag distance

    private Rigidbody2D rb;
    private bool isPlacing = true;   // true = positioning phase
    private bool isDragging = false; // true = aiming/dragging phase
    private Vector2 dragStart;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // Start at center of launch line
        transform.position = new Vector2(0f, launchY);
    }

    void Update()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ===== 1. Positioning Phase =====
        if (isPlacing)
        {
            if (Input.GetMouseButton(0))
            {
                // Move only along X axis within launch zone
                float clampedX = Mathf.Clamp(mouseWorld.x, minX, maxX);
                transform.position = new Vector2(clampedX, launchY);
            }
            // Right-click OR press Space to lock position and start aiming
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                isPlacing = false;
            }
        }
        // ===== 2. Aiming / Shooting Phase =====
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

                // Limit drag distance
                if (dragVector.magnitude > maxDragDistance)
                    dragVector = dragVector.normalized * maxDragDistance;

                rb.AddForce(dragVector * maxForce, ForceMode2D.Impulse);

                // After shot you can reset if needed
                // StartCoroutine(ResetForNextTurn());
            }
        }
    }

    // Optional reset for next turn
    public void ResetStriker()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector2(0f, launchY);
        isPlacing = true;
        isDragging = false;
    }
}
