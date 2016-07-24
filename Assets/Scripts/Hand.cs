using UnityEngine;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
	private SteamVR_Controller.Device device;
	private List<Collider> colliders;

	// Use this for initialization
	void Start () {
		var index = GetComponentInParent<SteamVR_TrackedObject> ().index;
		device = SteamVR_Controller.Input ((int)index);

		colliders = new List<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
		}

		foreach (var coll in colliders) {
			Debug.Log (coll.name);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (!colliders.Contains (other)) {
			colliders.Add (other);
		}
	}

	void OnTriggerExit(Collider other) {
		if (colliders.Contains (other)) {
			colliders.Remove (other);
		}
	}		
}
