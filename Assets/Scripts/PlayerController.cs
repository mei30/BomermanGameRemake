using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f; // Adjust in Inspector

    private Rigidbody2D rb;
    private Vector2 movement;

    private PlayerControl control;

    private Animator animator;

    public GameObject bombPrefab; // assign in Inspector

    private bool isDead = false;

    void Awake() {
        animator = GetComponent<Animator>();
        control = new PlayerControl();

        // Subscribe to input event
        control.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        control.Player.Move.canceled += ctx => movement = Vector2.zero;

        control.Player.Bomb.performed += ctx => PlaceBomb();

    }

    void PlaceBomb()
    {
        if (bombPrefab == null) return;

        // Snap bomb to grid so it aligns with tiles
        Vector2 spawnPos = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y));

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
    }

    void OnEnable()
    {
        control.Player.Enable();
    }

    void OnDisable()
    {
        control.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        animator.SetBool("IsMoving", movement != Vector2.zero);
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        // Normalize so diagonal movement isn’t faster

        Debug.Log($"Movement: {movement}");

        Vector2 newPosition = rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime;
        if (Vector2.up == movement)
            newPosition.x = Mathf.Round(newPosition.x);
        else if (Vector2.down == movement)
            newPosition.x = Mathf.Round(newPosition.x);
        else if (Vector2.left == movement)
            newPosition.y = Mathf.Round(newPosition.y);
        else if (Vector2.right == movement)
            newPosition.y = Mathf.Round(newPosition.y);

       rb.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        // If flame hits balloon
        if (other.CompareTag("Baloon"))
            OnHitByExplosion();
    }

    public void OnHitByExplosion()
    {
        if (isDead) return; // prevent multiple triggers

        isDead = true;
        animator.SetTrigger("Die"); // play death animation
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // stop movement

        control.Player.Disable();

        // Move player slightly "into" the scene so it renders above flames
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

    public void OnDeathAnimationEnd() {
        Destroy(gameObject); // adjust to animation time
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // If collided with balloon
        if (collision.collider.CompareTag("Baloon"))
        {
            OnHitByExplosion();
        }
    }
}
