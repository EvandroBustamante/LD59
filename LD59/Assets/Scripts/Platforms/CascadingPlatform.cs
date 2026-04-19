using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CascadingPlatform : Platform
{
    public float delayBetweenActivations;
    public float delayBetweenDeactivations;
    public float timeAlive;
    public float timeDead;
    public List<GameObject> platformsToToggle;

    private float timer;
    private bool isAlive;

    private void Awake()
    {
        timer = timeAlive;
    }

    private void Update()
    {
        if (canWork)
        {
            timer -= Time.deltaTime;

            if (isAlive && timer < 0)
            {
                StartCoroutine(CascadeToggle(false));
                timer = timeDead;
                isAlive = false;
            }
            else if (!isAlive && timer < 0)
            {
                StartCoroutine(CascadeToggle(true));
                timer = timeAlive;
                isAlive = true;
            }
        }
    }

    private IEnumerator CascadeToggle(bool activate)
    {
        if (activate)
        {
            for (int i = 0; i < platformsToToggle.Count; i++)
            {
                platformsToToggle[i].SetActive(true);
                yield return new WaitForSeconds(delayBetweenActivations);
            }
            yield break;
        }
        else
        {
            for (int i = platformsToToggle.Count - 1; i >= 0; i--)
            {
                platformsToToggle[i].SetActive(false);
                yield return new WaitForSeconds(delayBetweenDeactivations);
            }
        }
    }
}
