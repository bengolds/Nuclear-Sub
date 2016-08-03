using UnityEngine;
using System.Collections;

public class ButtonColor : MonoBehaviour {
    public Color buttonColor = Color.white;
    public Color markingColor = Color.black;

    void OnValidate()
    {
        GetComponentInChildren<TextMesh>().color = markingColor;
        var mat = GetComponent<MeshRenderer>().sharedMaterial;
        mat.SetColor("_BaseColor", buttonColor);
        mat.SetColor("_MarkingColor", markingColor);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
