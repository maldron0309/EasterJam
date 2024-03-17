using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    [SerializeField] private UnityEvent onCollect;
    [SerializeField] private float attractionSpeed = 5f;
    [SerializeField] private float attractionRadius = 2f;
    
    private static GameObject player;
    private bool isAttracted = false;
    private CircleCollider2D attractionCollider;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        attractionCollider = gameObject.AddComponent<CircleCollider2D>();
        attractionCollider.isTrigger = true;
        attractionCollider.radius = attractionRadius;
    }

    void Update()
    {
        if (isAttracted)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attractionSpeed * Time.deltaTime);
            CheckForCollection();
        }
    }

    private void CheckForCollection()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            onCollect.Invoke();
            Destroy(gameObject);
        }
    }

    private void ChangeAttraction(bool state)
    {
        isAttracted = state;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            ChangeAttraction(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            ChangeAttraction(false);
        }
    }
}
