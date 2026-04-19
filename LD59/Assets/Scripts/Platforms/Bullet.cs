using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speedX;
    public float speedY;

    void Update()
    {
        transform.Translate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this);
        }
    }
}
