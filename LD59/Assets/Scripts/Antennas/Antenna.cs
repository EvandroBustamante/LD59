using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using System;

public class Antenna : MonoBehaviour
{
    [Tooltip("Index 0 is always the first signal enabled")] public List<AntennaSignal> unlockableSignals;
    float maxWeakRadius;
    float maxStrongRadius;
    float speed = 0.8f;
    GameObject antenaTemp;

    private float t = 0.0f;
    private bool disable = false;
    private int lastIndex = 0;

    public void SignalToEnable(int index)
    {
        t = 0f;
        foreach (var signal in unlockableSignals)
        {
            signal.gameObject.SetActive(false);
        }

        antenaTemp = unlockableSignals[index].gameObject;
        antenaTemp.SetActive(true);
        maxWeakRadius = antenaTemp.GetComponent<AntennaSignal>().weakSignal.GetComponent<CircleCollider2D>().radius;
        maxStrongRadius = antenaTemp.GetComponent<AntennaSignal>().strongSignal.GetComponent<CircleCollider2D>().radius;

        antenaTemp.GetComponent<AntennaSignal>().weakSignal.GetComponent<CircleCollider2D>().radius = 0;
        antenaTemp.GetComponent<AntennaSignal>().strongSignal.GetComponent<CircleCollider2D>().radius = 0;

        lastIndex = index;
    }

    public void DisableAllSignals()
    {
        Debug.Log("here1");
        antenaTemp = unlockableSignals[lastIndex].gameObject;
        disable = true;
        t = 0;
    }

    private void Update()
    {
        if (t < 1 && antenaTemp)
        {
            if (!disable)
            {
                antenaTemp.GetComponent<AntennaSignal>().weakSignal.GetComponent<CircleCollider2D>().radius = Mathf.Lerp(0, maxWeakRadius, t);
                antenaTemp.GetComponent<AntennaSignal>().strongSignal.GetComponent<CircleCollider2D>().radius = Mathf.Lerp(0, maxStrongRadius, t);
            }
            
            if (disable)
            {
                Debug.Log("here");
                antenaTemp.GetComponent<AntennaSignal>().weakSignal.GetComponent<CircleCollider2D>().radius = Mathf.Lerp(maxWeakRadius, 0, t);
                antenaTemp.GetComponent<AntennaSignal>().strongSignal.GetComponent<CircleCollider2D>().radius = Mathf.Lerp(maxStrongRadius, 0, t);

                SpriteRenderer srWeak = antenaTemp.GetComponent<AntennaSignal>().weakSignal.GetComponent<SpriteRenderer>();
                SpriteRenderer srStrong = antenaTemp.GetComponent<AntennaSignal>().strongSignal.GetComponent<SpriteRenderer>();
                srWeak.color = new Color(srWeak.color.r, srWeak.color.g, srWeak.color.b, Mathf.Lerp(1, 0, t));
                srStrong.color = new Color(srStrong.color.r, srStrong.color.g, srStrong.color.b, Mathf.Lerp(1, 0, t));
            }

            t += speed * Time.deltaTime;
        }
    }

    
}
