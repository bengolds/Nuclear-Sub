using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PowerTerminal : MonoBehaviour {
	public Material poweredMaterial;
	public bool isPoweredOn;
	public UnityEvent onPowerOn;

	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isPoweredOn) {
			meshRenderer.material = poweredMaterial;
		}
	}

	public void PowerOn() {
		isPoweredOn = true;
		onPowerOn.Invoke ();
	}
}
