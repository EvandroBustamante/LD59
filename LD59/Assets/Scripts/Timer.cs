using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [HideInInspector] public float elapsedTime;
    [HideInInspector] public bool pauseTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseTime)
        {
            elapsedTime += Time.deltaTime;
        }

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int miliseconds = Mathf.FloorToInt((elapsedTime * 100f) % 100f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);

      
    }


}
