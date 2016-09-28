using UnityEngine;
using System.Collections.Generic;

public class Tesseract : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision coll)
    {
        var powerTerm = coll.gameObject.GetComponentInParent<PowerTerminal>();
        if (powerTerm != null)
        {
            powerTerm.PowerOn();
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        var dialSet = coll.gameObject.GetComponentInParent<DialSet>();
        if (dialSet != null)
        {
            Debug.Log("DialSet");
            dialSet.MagicUnlock();
        }
    }
}
