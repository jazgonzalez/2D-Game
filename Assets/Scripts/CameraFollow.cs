using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //the pig character
    //map boundaries to show on the camera
    public float minY;
    public float maxY;
    public float minX;
    public float maxX;

    private void LateUpdate()
    {
        //get the target's current position
        float targetX=target.position.x;
        float targetY=target.position.y;

        // Clamp the camera position so it doesn't go beyond the specified limits
        float clampX= Mathf.Clamp(targetX,minX,maxX);
        float clampY= Mathf.Clamp(targetY,minY,maxY);

        //update the cameras position
        transform.position = new Vector3( clampX, clampY,transform.position.z);
    }
}
