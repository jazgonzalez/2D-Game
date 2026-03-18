using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    
    private float targetX; // The specific X coordinate we are moving towards
    private float posXA;   // Fixed X position for Point A
    private float posXB;   // Fixed X position for Point B
    private bool movingToB = true;

    void Start()
    {
        // Capture the initial world positions so they don't "move" with the enemy
        if (pointA != null && pointB != null)
        {
            posXA = pointA.position.x;
            posXB = pointB.position.x;
        }
        else 
        {
            Debug.LogError("Please assign Point A and Point B in the Inspector!");
        }

        // Set the first target coordinate
        targetX = posXB;
    }

    void Update()
    {
        // Move only on the X axis towards the target coordinate
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), step);

        // Check if the enemy reached the target X coordinate
        if (Mathf.Abs(transform.position.x - targetX) < 0.1f)
        {
            // Switch target coordinate
            if (movingToB)
            {
                targetX = posXA;
                movingToB = false;
            }
            else
            {
                targetX = posXB;
                movingToB = true;
            }
        }

        // Face the correct direction based on the current target
        HandleFlip();
    }

    void HandleFlip()
    {
        if (targetX > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Face right
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Face left
        }
    }
}