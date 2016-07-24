using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {
	private SteamVR_Controller.Device device;

	// Use this for initialization
	void Start () {
		var index = GetComponentInParent<SteamVR_TrackedObject> ().index;
		device = SteamVR_Controller.Input ((int)index);
	}
	
	// Update is called once per frame
	void Update () {
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("Trigger clicked");
		}
	}
}
