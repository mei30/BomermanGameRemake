using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Baloon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isDead = false;

    public float moveSpeed = 2f;         // Movement speed
    public float changeDirTime = 2f;

    private Animator animator;
    private Vector2 moveDir;
    private float timer;

    private Rigidbody2D rb;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PickNewDirection();
        
        // Trigger enemy spawn event
        // EventManager.Instance.TriggerEnemySpawn(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Vector2 newPosition = rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
        if (Vector2.up == moveDir)
            newPosition.x = Mathf.Round(newPosition.x);
        else if (Vector2.down == moveDir)
            newPosition.x = Mathf.Round(newPosition.x);
        else if (Vector2.left == moveDir)
            newPosition.y = Mathf.Round(newPosition.y);
        else if (Vector2.right == moveDir)
            newPosition.y = Mathf.Round(newPosition.y);

        rb.MovePosition(newPosition);
        
        // Trigger enemy movement event (only when position actually changes)
        if (Vector2.Distance(rb.position, newPosition) > 0.01f)
        {
            GameEvents.SafeInvoke(GameEvents.OnEnemyMove, gameObject, newPosition);
        }

        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            PickNewDirection();
        }
    }

    public void OnHitByExplosion()
    {
        if (isDead) return; // prevent multiple triggers

        isDead = true;
        animator.SetTrigger("Die"); // play death animation
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // stop movement

        // Move enemy slightly "into" the scene so it renders above flames
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        
        // Trigger enemy death event
        EventManager.Instance.TriggerEnemyDeath(gameObject);
    }

    public void OnDeathAnimationEnd() {
        Destroy(gameObject); // adjust to animation time
    }

    void PickNewDirection()
    {
        // Pick random direction (up, down, left, right)
        int choice = Random.Range(0, 4);
        switch (choice)
        {
            case 0: moveDir = Vector2.up; break;
            case 1: moveDir = Vector2.down; break;
            case 2: moveDir = Vector2.left; break;
            case 3: moveDir = Vector2.right; break;
        }
        timer = changeDirTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead) return;

        // If hits a wall/brick → pick new direction
        if (other.collider.CompareTag("Wall") || other.collider.CompareTag("Brick"))
        {
            moveDir = -moveDir;
        }
    }
}
