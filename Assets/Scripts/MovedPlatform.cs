using UnityEngine;

public class MovPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;
    private bool movingTowardsB = true;

    void Update()
    {
        if (movingTowardsB)
        {
            MoveTowards(pointB);
        }
        else
        {
            MoveTowards(pointA);
        }
    }

    void MoveTowards(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            movingTowardsB = !movingTowardsB;
        }
    }
}
