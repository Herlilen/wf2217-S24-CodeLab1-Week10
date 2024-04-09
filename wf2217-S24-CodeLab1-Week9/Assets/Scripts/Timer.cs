using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool stop;
    public float timeRemaining = 10f; // Total time for the countdown timer
    public TextMeshProUGUI timer;

    private void Start()
    {
        stop = false;
    }

    void Update()
    {
        // Check if time is remaining
        if (timeRemaining > 0f)
        {
            // Decrease the time remaining by the time passed since the last frame
            if (!stop)
            {
                timeRemaining -= Time.deltaTime;
            }

            // Optional: Format the timeRemaining as minutes and seconds
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else if (timeRemaining <= 0)
        {
            timeRemaining = 0;
        }
    }
}
