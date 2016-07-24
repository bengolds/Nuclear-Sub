using UnityEngine;
using System.Collections.Generic;

public class DialSet : MonoBehaviour {
	public Dial[] dials;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Current dial value: " + GetValue ());
	}

	string GetValue() {
		string value = "";
		for (int i = 0; i < dials.Length; i++) {
			value += dials [i].value.ToString ();
		}
		return value;
	}
}
