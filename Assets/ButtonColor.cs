using UnityEngine;
using System.Collections;

public class ButtonColor : MonoBehaviour {
    public Color buttonColor = Color.white;
    public Color markingColor = Color.black;
    public MeshRenderer buttonBase;

    void OnValidate()
    {
        var textMesh = GetComponentInChildren<TextMesh>();
        if (textMesh != null)
        {
            textMesh.color = markingColor;
        }
        if (buttonBase != null)
        {
            var mat = buttonBase.GetComponent<MeshRenderer>().sharedMaterial;
            mat.SetColor("_BaseColor", buttonColor);
            mat.SetColor("_MarkingColor", markingColor);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
