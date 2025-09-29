using UnityEngine;

public class StrikerMovement : MonoBehaviour
{
    public float maxForce = 10f;       // Max shooting force
    public float maxDragDistance = 2f; // Limit how far you can pull back

    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 dragStart;
    private bool isDragging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position; // starting position of striker
    }

    void Update()
    {
        // Start Drag
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mouseWorld, transform.position) < 0.5f) // click near striker
            {
                isDragging = true;
                dragStart = mouseWorld;
                rb.linearVelocity = Vector2.zero;  // stop any current movement
            }
        }

        // While Dragging
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragVector = mouseWorld - startPos;

            // Limit drag distance
            if (dragVector.magnitude > maxDragDistance)
                dragVector = dragVector.normalized * maxDragDistance;

            // Move striker opposite to drag direction
            transform.position = startPos + dragVector;
        }

        // Release to Shoot
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Vector2 force = (startPos - (Vector2)transform.position) * maxForce;
            rb.AddForce(force, ForceMode2D.Impulse);

            // Reset striker position after shot if needed
            // StartCoroutine(ResetAfterDelay());
        }
    }

    // Optional Reset (call after pocket or foul)
    public void ResetStriker()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPos;
    }
}
