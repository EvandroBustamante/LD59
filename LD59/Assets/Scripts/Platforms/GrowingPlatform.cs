using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GrowingPlatform : Platform
{
    public float timeAlive;
    public float timeDead;
    public Transform scaleRef;
    public float scaleUpSpeed;
    public float scaleDownSpeed;

    private float timer;
    private bool isAlive = false;
    private Vector3 originalPos;
    private Vector3 originalScale;
    private Vector3 targetPos;
    private Vector3 targetScale;
    private float targetSpeed;

    private void Awake()
    {
        scaleRef.gameObject.SetActive(false);
        originalPos = transform.position;
        originalScale = transform.lossyScale;

        timer = timeDead;
        targetPos = scaleRef.transform.position;
        targetScale = scaleRef.transform.lossyScale;
        targetSpeed = scaleUpSpeed;
    }

    private void Update()
    {
        if (canWork)
        {
            timer -= Time.deltaTime;

            if (isAlive && timer < 0)
            {
                AudioManager.Instance.PlayMinimizePlatform(gameObject);
                targetPos = originalPos;
                targetScale = originalScale;
                targetSpeed = scaleDownSpeed;
                timer = timeDead;
                isAlive = false;
            }
            else if (!isAlive && timer < 0)
            {
                AudioManager.Instance.PlayMaximizePlatform(gameObject);
                targetPos = scaleRef.transform.position;
                targetScale = scaleRef.transform.lossyScale;
                targetSpeed = scaleUpSpeed;
                timer = timeAlive;
                isAlive = true;
            }

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, targetSpeed * Time.deltaTime), Mathf.Lerp(transform.position.y, targetPos.y, targetSpeed * Time.deltaTime), transform.position.z);
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, targetScale.x, targetSpeed * Time.deltaTime), Mathf.Lerp(transform.localScale.y, targetScale.y, targetSpeed * Time.deltaTime), transform.localScale.z);
        }
    }
}
