using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Antenna : MonoBehaviour
{
    [Tooltip("Index 0 is always the first signal enabled")] public List<AntennaSignal> unlockableSignals;
    float maxWeakRadius;
    float maxStrongRadius;
    float speed = 0.8f;
    GameObject antenaTemp;

    static float t = 0.0f;


    public void SignalToEnable(int index)
    {
        
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


    }

    private void Update()
    {
        antenaTemp.GetComponent<AntennaSignal>().weakSignal.GetComponent<CircleCollider2D>().radius = Mathf.Lerp(0, maxWeakRadius, t);
        antenaTemp.GetComponent<AntennaSignal>().strongSignal.GetComponent<CircleCollider2D>().radius = Mathf.Lerp(0, maxStrongRadius, t);

        t += speed * Time.deltaTime;

    }
}
