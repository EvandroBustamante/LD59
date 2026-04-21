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

    //public SpriteRenderer sprite1;
    //public SpriteRenderer spritecas1;
    //public SpriteRenderer spritecas2;
    //public SpriteRenderer spritecas3;

    private float timer;
    private bool isAlive;

    private void Awake()
    {
        timer = timeDead;
        //SetupSpriteSizes();
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

    //private void SetupSpriteSizes()
    //{
    //    sprite1.size = new Vector2(transform.localScale.x, transform.localScale.y);
    //    sprite1.transform.position = transform.position;

    //    if (platformsToToggle[0])
    //    {
    //        spritecas1.size = new Vector2(platformsToToggle[0].transform.lossyScale.x, platformsToToggle[0].transform.lossyScale.y);
    //        spritecas1.transform.position = platformsToToggle[0].transform.position;
    //    }

    //    if (platformsToToggle[1])
    //    {
    //        spritecas2.size = new Vector2(platformsToToggle[1].transform.lossyScale.x, platformsToToggle[1].transform.lossyScale.y);
    //        spritecas2.transform.position = platformsToToggle[1].transform.position;
    //    }

    //    if (platformsToToggle[2])
    //    {
    //        spritecas3.size = new Vector2(platformsToToggle[2].transform.lossyScale.x, platformsToToggle[2].transform.lossyScale.y);
    //        spritecas3.transform.position = platformsToToggle[2].transform.position;
    //    }
    //}

    private IEnumerator CascadeToggle(bool activate)
    {
        if (activate)
        {
            for (int i = 0; i < platformsToToggle.Count; i++)
            {
                platformsToToggle[i].SetActive(true);
                AudioManager.Instance.PlayCascadingPlatform(platformsToToggle[i]);
                yield return new WaitForSeconds(delayBetweenActivations);
            }
            yield break;
        }
        else
        {
            for (int i = platformsToToggle.Count - 1; i >= 0; i--)
            {
                platformsToToggle[i].SetActive(false);
                AudioManager.Instance.PlayCascadingPlatform(platformsToToggle[i]);
                yield return new WaitForSeconds(delayBetweenDeactivations);
            }
        }
    }
}
