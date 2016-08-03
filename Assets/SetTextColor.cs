using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetTextColor : MonoBehaviour {

	// Use this for initialization
	void OnValidate () {
        var text = GetComponentInChildren<TextMesh>();
        text.color = GetComponent<MeshRenderer>().material.GetColor("_MarkingColor");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
