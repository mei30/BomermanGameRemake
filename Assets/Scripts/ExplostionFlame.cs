using UnityEngine;

public class ExplosionFlame : MonoBehaviour
{
    public float duration = 0.4f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>()?.OnHitByExplosion();
        }

        else if (collision.CompareTag("Bomb"))
        {
            // Chain reaction
            collision.GetComponent<Bomb>()?.CancelInvoke();
            collision.GetComponent<Bomb>()?.Invoke("Explode", 0f);
        }

        else if (collision.CompareTag("Brick"))
        {
            Debug.Log("Brick");
            collision.GetComponent<BrickTile>()?.OnHitByExplosion();
        }

        else if (collision.CompareTag("Baloon"))
        {
            Debug.Log("Brick");
            collision.GetComponent<Baloon>()?.OnHitByExplosion();
        }
    }

}