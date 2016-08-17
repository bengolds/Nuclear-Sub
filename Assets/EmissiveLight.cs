using UnityEngine;
using System.Collections.Generic;

public class EmissiveLight : MonoBehaviour {
    [ColorUsage(false, true, 0, 100, 1/8, 3)]
    public Color color;
    public float frequency;
    private Color onColor;
    private Renderer emRenderer;

	// Use this for initialization
	void Start () {
        emRenderer = GetComponent<Renderer>();
        onColor = emRenderer.material.GetColor("_EmissionColor");
	}
	
	// Update is called once per frame
	void Update () {
        //TODO MAKE THIS PROPER MATH EH
        float t = Mathf.Sin(frequency * Time.time);
        Color currColor = Color.Lerp(Color.black, color, t);
        DynamicGI.SetEmissive(emRenderer, currColor);
        emRenderer.sharedMaterial.SetColor("_EmissionColor", currColor); 
	}
}
