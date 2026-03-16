using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed=2f;
    private Transform currentTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //the enemy starts moving towards B
        currentTarget=pointB;
    }

    // Update is called once per frame
    void Update()
    {
        //move the enemy towards the current target
        transform.position = Vector2.MoveTowards(transform.position,currentTarget.position, speed*Time.deltaTime);
        //check if the enemy reach point B
        if(Vector2.Distance(transform.position,currentTarget.position) < 0.1f)
        {
            //switch targets
            if(currentTarget == pointB)
            {
                currentTarget = pointA;
                Flip(180f); //face left
            }
            //if it didnt reach the target b
            else
            {
                currentTarget = pointB;
                Flip(0f); // Face right
            }
        }
    }
    void Flip(float rotationY)
    {
        // Rotate the enemy to face the correct direction
        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }
}
