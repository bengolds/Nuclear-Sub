using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class DialSet : MonoBehaviour {
	public Dial[] dials;
	public string combination;
	public UnityEvent onUnlock;
	private bool unlocked = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Current dial value: " + GetValue ());
		if (!unlocked && GetValue () == combination) {
			Unlock ();
		}
	}

	void Unlock() {
		unlocked = true;
		onUnlock.Invoke ();
	}

	string GetValue() {
		string value = "";
		for (int i = 0; i < dials.Length; i++) {
			value += dials [i].GetValue();
		}
		return value;
	}
}
