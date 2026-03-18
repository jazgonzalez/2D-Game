using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Distance the platform will move from its start position")]
    public Vector2 finishOffset = new Vector2(0, 5f); // Example: moves 5 units up
    public float speed = 2.0f;
    public float waitTime = 1.0f; // Time to wait at each end

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 targetPos;
    private float nextMoveTime;

    void Start()
    {
        // Store the initial position where you placed the platform in the scene
        startPos = transform.position;
        // Calculate the destination based on the offset
        endPos = startPos + (Vector3)finishOffset;
        
        targetPos = endPos;
    }

    void Update()
    {
        // Only move if the current time has passed the waiting threshold
        if (Time.time >= nextMoveTime)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        // Interpolate position towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Check if the platform reached the destination
        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            // Swap target between Start and End
            targetPos = (targetPos == endPos) ? startPos : endPos;
            
            // Set the delay before moving again
            nextMoveTime = Time.time + waitTime;
        }
    }

    // --- PHYSICS INTERACTION ---
    // Make the player a child of the platform so they move together without sliding
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    // Remove the parent-child relationship when the player jumps or leaves
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }


}