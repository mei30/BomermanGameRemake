using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fuseTime = 4f; // seconds before explosion

    private Animator animator;

    public GameObject explosionCenter;
    public GameObject explosionHorizontal;
    public GameObject explosionVertical;
    public GameObject explosionEndUp, explosionEndDown, explosionEndLeft, explosionEndRight;
    public int range = 3;

    public LayerMask obstacleLayer; // walls + bricks

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Start countdown when bomb is placed
        Invoke(nameof(Explode), fuseTime);
    }

    void Explode()
    {
        // TODO: show explosion effect
        // Spawn center
        Instantiate(explosionCenter, transform.position, Quaternion.identity);

        ExplodeInDirection(Vector2.up, explosionVertical, explosionEndUp);
        ExplodeInDirection(Vector2.down, explosionVertical, explosionEndDown);
        ExplodeInDirection(Vector2.left, explosionHorizontal, explosionEndLeft);
        ExplodeInDirection(Vector2.right, explosionHorizontal, explosionEndRight);

        Destroy(gameObject);

    }

    void ExplodeInDirection(Vector2 dir, GameObject middlePrefab, GameObject endPrefab)
    {
        for (int i = 1; i <= range; i++)
        {
            Vector2 pos = (Vector2)transform.position + dir * i;

            Collider2D hit = Physics2D.OverlapBox(pos, Vector2.one * 0.8f, 0f, obstacleLayer);

            if (hit == null)
            {
                // TODO: add collision logic (walls, etc.)
                if (i < range)
                    Instantiate(middlePrefab, pos, middlePrefab.transform.rotation);
                else
                    Instantiate(endPrefab, pos, endPrefab.transform.rotation);
            }
            else if (hit.CompareTag("Brick"))
            {
                Instantiate(endPrefab, pos, endPrefab.transform.rotation);
                break;
            }
            else if (hit.CompareTag("Wall"))
                break;
        }
    }
}