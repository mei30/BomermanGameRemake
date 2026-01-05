using UnityEngine;

public class BrickTile : MonoBehaviour
{
    private bool isBreak = false;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHitByExplosion()
    {
        if (isBreak) return; // prevent multiple triggers

        isBreak = true;
        animator.SetTrigger("Die"); // play death animation
    }

    public void OnBrickDestroyed()
    {
        Destroy(gameObject); // adjust to animation time
    }
}
