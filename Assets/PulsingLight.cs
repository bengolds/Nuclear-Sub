using UnityEngine;
using System.Collections.Generic;

public class PulsingLight : MonoBehaviour {

    public float frequency;
    private Color onColor;
    private Renderer emRenderer;

	// Use this for initialization
	void Start () {
        emRenderer = GetComponent<Renderer>();
        onColor = emRenderer.material.GetColor("_EmissionColor");
        Debug.Log(onColor);
	}
	
	// Update is called once per frame
	void Update () {
        //TODO MAKE THIS PROPER MATH EH
        float t = Mathf.Sin(frequency * Time.time);
        Color currColor = Color.LerpUnclamped(Color.black, onColor, t);
        DynamicGI.SetEmissive(emRenderer, currColor); 
	}
}
