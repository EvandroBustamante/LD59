using UnityEngine;

public class Shooter : Platform
{
    public Transform pointA;
    public Transform pointB;
    public float speed;
    [Tooltip("Time the platform wait when it reaches the edges before resuming patrol")] public float waitTime;
    public GameObject bulletProjectile;
    public float bulletInterval;
    public float bulletSpeedX;
    public float bulletSpeedY;
    public float bulletLifetime;

    private bool goingA = false;
    private bool waiting = false;
    private Transform currentTarget;
    private float waitingTimer;
    private float bulletTimer;

    private void Awake()
    {
        bulletTimer = bulletInterval;
    }

    private void Update()
    {
        if (canWork)
        {
            if (goingA)
            {
                currentTarget = pointA;

                if (Vector3.Distance(transform.position, pointA.position) < 0.5f)
                {
                    goingA = false;
                    waiting = true;
                    waitingTimer = waitTime;
                }
            }
            else if (!goingA)
            {
                currentTarget = pointB;

                if (Vector3.Distance(transform.position, pointB.position) < 0.5f)
                {
                    goingA = true;
                    waiting = true;
                    waitingTimer = waitTime;
                }
            }

            if (!waiting)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, currentTarget.transform.position.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, currentTarget.transform.position.y, speed * Time.deltaTime), transform.position.z);
            }
            else
            {
                waitingTimer -= Time.deltaTime;

                if(waitingTimer < 0)
                {
                    waiting = false;
                }
            }

            //bullet:
            bulletTimer -= Time.deltaTime;

            if(bulletTimer < 0)
            {
                GameObject newBullet = Instantiate(bulletProjectile, transform.position, Quaternion.identity);
                newBullet.GetComponent<Bullet>().speedX = bulletSpeedX;
                newBullet.GetComponent<Bullet>().speedY = bulletSpeedY;
                Destroy(newBullet, bulletLifetime);
                bulletTimer = bulletInterval;
            }
        }
    }
}
