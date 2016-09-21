using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

public class Countdown : MonoBehaviour {
    public float startingMinutes = 60;
    public UnityEvent onCountdownEnd;
    private float startTime;
    private TextMesh textMesh;
    private bool countdownEnded = false;
    private bool stopped = false;
    private float stoppedSecs = float.NaN;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        textMesh.text = GetTimeLeft();
        if (!countdownEnded && GetSecondsLeft() < 0)
        {
            countdownEnded = true;
            onCountdownEnd.Invoke();
        }
    }

    float GetSecondsLeft()
    {
        if (stopped)
        {
            return stoppedSecs;
        }
        else
        {
            return startingMinutes * 60 - (Time.time - startTime);
        }
    }

    public string GetTimeLeft()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(GetSecondsLeft());
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    public void Stop()
    {
        stoppedSecs = GetSecondsLeft();
        stopped = true;
    }
}
