using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class PowerTerminal : HasWatchableBool {
	public Material poweredMaterial;
    public bool startsPoweredOn;
    //Just serves as an alias for the watchable bool.
	public bool isPoweredOn
    {
        get { return boolValue; }
        set { boolValue = value; }
    }
	public UnityEvent onPowerOn;

	private MeshRenderer meshRenderer;
    
    // Use this for initialization
    void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
        SilentSetBoolValue(startsPoweredOn);
	}
	
	// Update is called once per frame
	void Update () {
		if (isPoweredOn && meshRenderer != null) {
			meshRenderer.material = poweredMaterial;
		}
	}

	public void PowerOn() {
		isPoweredOn = true;
		onPowerOn.Invoke ();
	}
}
