using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

public class Countdown : MonoBehaviour {
    public int startingMinutes = 60;
    public UnityEvent onCountdownEnd;
    private float startTime;
    private TextMesh textMesh;
    private bool countdownEnded = false;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        textMesh.text = GetTimeLeft();
        if (countdownEnded && GetSecondsLeft() < 0)
        {
            countdownEnded = true;
            onCountdownEnd.Invoke();
        }
    }

    float GetSecondsLeft()
    {
        return startingMinutes * 60 - (Time.time - startTime);
    }

    public string GetTimeLeft()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(GetSecondsLeft());
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}
