using System.Collections;
using UnityEngine;

public class MovedPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    private bool isMoving = false;
    private int i;

    private void Awake()
    {
        transform.position = points[startingPoint].position;
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }

        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    public void StartMoving()
    {
        isMoving = true;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private IEnumerator ChangeParentWithDelay(Transform newParent, GameObject child)
    {
        yield return null; 
        child.transform.SetParent(newParent);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeParentWithDelay(null, other.gameObject));
        }
    }

}
