using UnityEngine;
using System.Collections.Generic;

public class WireEnd : MonoBehaviour {
	public WireEnd otherEnd;
	private PowerTerminal connectedTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isPowered() {
		return connectedTo != null && connectedTo.isPoweredOn;
	}

	void OnCollisionStay(Collision collision) {
		var powerTerminal = collision.gameObject.GetComponent<PowerTerminal> ();
		if (powerTerminal != null) {
			connectedTo = powerTerminal;
			if (otherEnd.isPowered ()) {
				connectedTo.PowerOn ();
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		var powerTerminal = collision.gameObject.GetComponent<PowerTerminal> ();
		if (powerTerminal == connectedTo) {
			connectedTo = null;
		}
	}
}
