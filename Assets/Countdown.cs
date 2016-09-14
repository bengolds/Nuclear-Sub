using UnityEngine;
using System.Collections;
using System;

public class Countdown : MonoBehaviour {
    public int startingMinutes = 60;
    private float startTime;
    private TextMesh textMesh;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        float timeLeft = startingMinutes * 60 - (Time.time - startTime);
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
        textMesh.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }
}
