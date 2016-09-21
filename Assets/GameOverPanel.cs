using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameOverPanel : MonoBehaviour {
    public CanvasRenderer mainPanel;
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
        mainPanel.gameObject.SetActive(true);
        DOTween.To(x => mainPanel.SetAlpha(x), 0, 1f, fadeDuration);
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
