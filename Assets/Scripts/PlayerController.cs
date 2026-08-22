using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f; // Adjust in Inspector

    [Header("Events")]
    [SerializeField] private GameEvent onPlayerDied;

    [Header("Audio")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource spawnBombAudioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip bombPlaceClip;
    [SerializeField] private AudioClip deathClip;

    private Rigidbody2D rb;
    private Vector2 movement;

    private PlayerControl control;

    private Animator animator;

    public GameObject bombPrefab; // assign in Inspector

    private bool isDead = false;

    void Awake() {
        animator = GetComponent<Animator>();
        control = new PlayerControl();

        Debug.Log("PlayerController: Awake called, initializing audio sources");

        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }

        if (footstepAudioSource == null)
        {
            footstepAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (sfxAudioSource == null)
        {
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }

        footstepAudioSource.loop = true;
        footstepAudioSource.playOnAwake = false;
        footstepAudioSource.clip = footstepClip;

        sfxAudioSource.playOnAwake = false;

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

        Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        PlayBombPlaceSound(spawnPos);
    }

    void OnEnable()
    {
        Debug.Log("PlayerController: Enabling player controls");
        control.Player.Enable();
    }

    void OnDisable()
    {
        StopFootsteps();
        control.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Debug.Log("PlayerController: Setting camera target to player");
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isDead)
        {
            StopFootsteps();
            return;
        }

        animator.SetBool("IsMoving", movement != Vector2.zero);
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        // Normalize so diagonal movement isn’t faster

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

        if (movement != Vector2.zero)
        {
            PlayFootsteps();
        }
        else
        {
            StopFootsteps();
        }
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
        StopFootsteps();
        PlayDeathSound();
        animator.SetTrigger("Die"); // play death animation
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // stop movement

        control.Player.Disable();

        // Move player slightly "into" the scene so it renders above flames
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }

    public void OnDeathAnimationEnd() {
        onPlayerDied?.Raise();
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

    private void PlayFootsteps()
    {
        if (footstepAudioSource == null || footstepClip == null)
        {
            return;
        }

        if (footstepAudioSource.clip != footstepClip)
        {
            footstepAudioSource.clip = footstepClip;
        }

        if (!footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
    }

    private void StopFootsteps()
    {
        if (footstepAudioSource != null && footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }
    }

    private void PlayDeathSound()
    {
        if (sfxAudioSource == null || deathClip == null)
        {
            return;
        }

        sfxAudioSource.PlayOneShot(deathClip);
    }

    private void PlayBombPlaceSound(Vector2 position)
    {
        if (bombPlaceClip == null)
        {
            return;
        }

        spawnBombAudioSource.PlayOneShot(bombPlaceClip);
    }
}
