using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour {
    public GameObject mainPanel;
    public Countdown countdown;
    public GameObject targetingCorrectBool;
    public Text scenarioText;
    public Text timeLeftText;
    public string winString;
    public string wrongCityString;
    public string timeUpString;

	// Use this for initialization
	void Start () {
	    
	}       
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GameOver(float fadeDuration, bool launched)
    {
        mainPanel.SetActive(true);

        var graphics = GetComponentsInChildren<Graphic>();

        foreach (var gr in graphics)
        {
            var cr = gr.GetComponent<CanvasRenderer>();
            float targetAlpha = cr.GetAlpha();
            cr.SetAlpha(0);
            gr.CrossFadeAlpha(targetAlpha, 3f, false);
        }

        timeLeftText.text = countdown.GetTimeLeft();
        if (launched)
        {
            if (targetingCorrectBool.GetComponent<IWatchableBool>().boolValue)
            {
                scenarioText.text = winString;
            }
            else
            {
                scenarioText.text = wrongCityString;
            }
        }
        else
        {
            scenarioText.text = timeUpString;
        }
    }   
}
