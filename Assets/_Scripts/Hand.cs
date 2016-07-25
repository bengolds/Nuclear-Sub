using UnityEngine;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
	public SteamVR_Controller.Device device;
	private SteamVR_RenderModel model;

	// Use this for initialization
	void Start () {
		var index = GetComponentInParent<SteamVR_TrackedObject> ().index;
		device = SteamVR_Controller.Input ((int)index);
		model = transform.parent.GetComponentInChildren<SteamVR_RenderModel> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetVisible(bool visible) {
		model.gameObject.SetActive (visible);
	}
}
