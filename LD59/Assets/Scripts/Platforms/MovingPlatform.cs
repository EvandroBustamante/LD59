using DG.Tweening;
using System.Collections;
using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(StudioEventEmitter))]
public class MovingPlatform : Platform
{
    public float rotationSpeed;
    public Transform pointA;
    public Transform pointB;
    public float speed;
    public bool isPlatform;

    private bool goingA = false;
    private Transform currentTarget;

    private StudioEventEmitter emitter;

    private void Awake()
    {
        emitter = GetComponent<StudioEventEmitter>();
        transform.localScale = new Vector3(1f, 1f, 1f);
        StartCoroutine(DelayStart());
    }

    private void Update()
    {
        if (canWork)
        {
            if (goingA)
            {
                currentTarget = pointA;

                if (Vector3.Distance(transform.position, pointA.position) < 0.15f)
                {
                    goingA = false;
                }
            }
            else if (!goingA)
            {
                currentTarget = pointB;

                if(Vector3.Distance(transform.position, pointB.position) < 0.15f)
                {
                    goingA = true;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);
        }

    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.5f);

        if (isPlatform)
        {
            emitter.EventReference = AudioManager.Instance.movingPlatform;
            emitter.Play();
            /*AudioManager.Instance.PlayMovingPlatform(gameObject);*/
        }
        else
        {
            emitter.EventReference = AudioManager.Instance.movingSaw;
            emitter.Play();
           /* AudioManager.Instance.PlayMovingSaw(gameObject);*/
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerCharacter>().GetIsGrounded())
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
