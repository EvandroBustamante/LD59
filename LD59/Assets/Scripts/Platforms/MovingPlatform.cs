using System.Collections;
using UnityEngine;

public class MovingPlatform : Platform
{
    public Transform pointA;
    public Transform pointB;
    public float speed;

    private bool goingA = false;
    private Transform currentTarget;

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
                }
            }
            else if (!goingA)
            {
                currentTarget = pointB;

                if(Vector3.Distance(transform.position, pointB.position) < 0.5f)
                {
                    goingA = true;
                }
            }
            
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, currentTarget.transform.position.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, currentTarget.transform.position.y, speed * Time.deltaTime), transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
