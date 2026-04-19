using System.Collections;
using UnityEngine;

public class MovingPlatform : Platform
{
    public Transform pointA;
    public Transform pointB;
    public float speed;
    public float worldDelay;

    private bool canMove = false;
    private bool goingA = false;
    private Transform currentTarget;

    private void Start()
    {
        StartCoroutine(WaitWorldDelay());
    }

    private void Update()
    {
        if (canMove)
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

    private IEnumerator WaitWorldDelay()
    {
        yield return new WaitForSeconds(worldDelay);
        canMove = true;
    }
}
