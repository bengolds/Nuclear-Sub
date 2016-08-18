using UnityEngine;
using System.Collections.Generic;

public class EmissiveLight : MonoBehaviour {
    [ColorUsage(false, true, 0, 100, 1/8, 3)]
    public Color maxEmissionColor;

    [Range(0, 1)]
    public float brightness;
    private Renderer emRenderer;

	// Use this for initialization
	void Start () {
        emRenderer = GetComponent<Renderer>();
        var startColor = emRenderer.material.GetColor("_EmissionColor");
        //emRenderer.material = Instantiate(emRenderer.sharedMaterial);
        if (startColor == Color.black)
        {
            Debug.LogError("Your light needs to start with some emission.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        Color currColor = Color.Lerp(Color.black, maxEmissionColor, brightness);
        DynamicGI.SetEmissive(emRenderer, currColor);
        emRenderer.sharedMaterial.SetColor("_EmissionColor", currColor); 
	}
}
