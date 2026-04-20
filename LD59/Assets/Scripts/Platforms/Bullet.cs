using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speedX;
    public float speedY;

    public LayerMask groundLayer;

    void Update()
    {
        transform.Translate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0);

        bool hitGround = Physics2D.OverlapCircle(transform.position, .0025f, groundLayer);
        if (hitGround) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
