using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameEndController : MonoBehaviour {
    public Light mainLight;
    public float dimIntensity;
    public float dimDuration;
    public Ease dimEase;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EndGame()
    {
        mainLight.DOIntensity(dimIntensity, dimDuration).SetEase(dimEase);
        DOTween.To(x => RenderSettings.ambientIntensity = x, RenderSettings.ambientIntensity, 0, dimDuration).SetEase(dimEase);
    }
}
