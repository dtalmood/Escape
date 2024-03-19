using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameTimer : MonoBehaviour
{
    float startTime;
    float currentTime;
    float elapsedTime;
    TMP_Text timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.realtimeSinceStartup;
        timerDisplay = GetComponentInChildren<TMP_Text>();   
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.realtimeSinceStartup;
        elapsedTime = (currentTime - startTime);
        timerDisplay.text = ConvertSecondsToTimeString((int)elapsedTime);
    }

    static string ConvertSecondsToTimeString(int t)
    {
        int hours = t / 3600;
        int minutes = (t % 3600) / 60;
        int seconds = t % 60;

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}
