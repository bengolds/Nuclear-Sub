using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class GameEndController : MonoBehaviour {
    public Light mainLight;
    public GameOverPanel panel;
    public List<GameObject> enableOnOver;
    public Countdown countdown;
    public float dimIntensity;
    public float dimDuration;
    public Ease dimEase;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EndGame(bool launched)
    {
        mainLight.DOIntensity(dimIntensity, dimDuration).SetEase(dimEase);
        DOTween.To(x => RenderSettings.ambientIntensity = x, RenderSettings.ambientIntensity, 0, dimDuration).SetEase(dimEase);
        var buttons = FindObjectsOfType<LitButton>();
        foreach (var button in buttons)
        {
            //Janky way to dim every button but the game quit button.
            if (button.gameObject.GetComponent<GameQuitter>() == null)
            {
                button.OverrideBrightness(0, dimDuration, dimEase);
            }
        }
        panel.GameOver(dimDuration, launched);
        countdown.Stop();
        foreach (var item in enableOnOver)
        {
            item.SetActive(true);
        }
    }
}
